/*
** SGI FREE SOFTWARE LICENSE B (Version 2.0, Sept. 18, 2008) 
** Copyright (C) 2011 Silicon Graphics, Inc.
** All Rights Reserved.
**
** Permission is hereby granted, free of charge, to any person obtaining a copy
** of this software and associated documentation files (the "Software"), to deal
** in the Software without restriction, including without limitation the rights
** to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
** of the Software, and to permit persons to whom the Software is furnished to do so,
** subject to the following conditions:
** 
** The above copyright notice including the dates of first publication and either this
** permission notice or a reference to http://oss.sgi.com/projects/FreeB/ shall be
** included in all copies or substantial portions of the Software. 
**
** THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
** INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
** PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL SILICON GRAPHICS, INC.
** BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
** TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
** OR OTHER DEALINGS IN THE SOFTWARE.
** 
** Except as contained in this notice, the name of Silicon Graphics, Inc. shall not
** be used in advertising or otherwise to promote the sale, use or other dealings in
** this Software without prior written authorization from Silicon Graphics, Inc.
*/
/*
** Original Author: Eric Veach, July 1994.
** libtess2: Mikko Mononen, http://code.google.com/p/libtess2/.
** LibTessDotNet: Remi Gillig, https://github.com/speps/LibTessDotNet
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AgOpenGPS
{
    public partial class Tess
    {
        private readonly IPool _pool;
        private Mesh _mesh;
        private Vec3 _sUnit;
        private Vec3 _tUnit;

        private double _bminX, _bminY, _bmaxX, _bmaxY;

        private Dict<ActiveRegion> _dict;
        private PriorityQueue<MeshUtils.Vertex> _pq;
        private MeshUtils.Vertex _event;


        public const int Undef = ~0;

        public double SentinelCoord = 4e30f;


        /// <summary>
        /// Vertices of the tessellated mesh.
        /// </summary>
        public Vec3[] Vertices { get; private set; }
        /// <summary>
        /// Number of vertices in the tessellated mesh.
        /// </summary>
        public int VertexCount { get; private set; }

        public int[] Elements { get; private set; }
        /// <summary>
        /// Number of elements in the tessellated mesh.
        /// </summary>
        public int ElementCount { get; private set; }

        public Tess(IList<Vec3> vertices)
        {
            _bminX = _bminY = _bmaxX = _bmaxY = 0;

            _pool = new DefaultPool();

            _mesh = null;
            Vertices = null;
            VertexCount = 0;
            Elements = null;
            ElementCount = 0;
            AddContourInternal(vertices);
            Tessellate();
        }

        private void ProjectPolygon()
        {
            _sUnit[2] = 0;
            _sUnit[0] = 1;
            _sUnit[1] = 0;

            _tUnit[2] = 0;
            _tUnit[0] = -0;
            _tUnit[1] = 1;

            // Project the vertices onto the sweep plane
            for (var v = _mesh._vHead._next; v != _mesh._vHead; v = v._next)
            {
                Vec3.Dot(ref v._coords, ref _sUnit, out v._s);
                Vec3.Dot(ref v._coords, ref _tUnit, out v._t);
            }

            // Compute ST bounds.
            bool first = true;
            for (var v = _mesh._vHead._next; v != _mesh._vHead; v = v._next)
            {
                if (first)
                {
                    _bminX = _bmaxX = v._s;
                    _bminY = _bmaxY = v._t;
                    first = false;
                }
                else
                {
                    if (v._s < _bminX) _bminX = v._s;
                    if (v._s > _bmaxX) _bmaxX = v._s;
                    if (v._t < _bminY) _bminY = v._t;
                    if (v._t > _bmaxY) _bmaxY = v._t;
                }
            }
        }

        /// <summary>
        /// TessellateMonoRegion( face ) tessellates a monotone region
        /// (what else would it do??)  The region must consist of a single
        /// loop of half-edges (see mesh.h) oriented CCW.  "Monotone" in this
        /// case means that any vertical line intersects the interior of the
        /// region in a single interval.  
        /// 
        /// Tessellation consists of adding interior edges (actually pairs of
        /// half-edges), to split the region into non-overlapping triangles.
        /// 
        /// The basic idea is explained in Preparata and Shamos (which I don't
        /// have handy right now), although their implementation is more
        /// complicated than this one.  The are two edge chains, an upper chain
        /// and a lower chain.  We process all vertices from both chains in order,
        /// from right to left.
        /// 
        /// The algorithm ensures that the following invariant holds after each
        /// vertex is processed: the untessellated region consists of two
        /// chains, where one chain (say the upper) is a single edge, and
        /// the other chain is concave.  The left vertex of the single edge
        /// is always to the left of all vertices in the concave chain.
        /// 
        /// Each step consists of adding the rightmost unprocessed vertex to one
        /// of the two chains, and forming a fan of triangles from the rightmost
        /// of two chain endpoints.  Determining whether we can add each triangle
        /// to the fan is a simple orientation test.  By making the fan as large
        /// as possible, we restore the invariant (check it yourself).
        /// </summary>
        private void TessellateMonoRegion(MeshUtils.Face face)
        {
            // All edges are oriented CCW around the boundary of the region.
            // First, find the half-edge whose origin vertex is rightmost.
            // Since the sweep goes from left to right, face->anEdge should
            // be close to the edge we want.
            var up = face._anEdge;
            Debug.Assert(up._Lnext != up && up._Lnext._Lnext != up);

            while (Geom.VertLeq(up.Dst, up._Org)) up = up.Lprev;
            while (Geom.VertLeq(up._Org, up.Dst)) up = up._Lnext;

            var lo = up.Lprev;

            while (up._Lnext != lo)
            {
                if (Geom.VertLeq(up.Dst, lo._Org))
                {
                    // up.Dst is on the left. It is safe to form triangles from lo.Org.
                    // The EdgeGoesLeft test guarantees progress even when some triangles
                    // are CW, given that the upper and lower chains are truly monotone.
                    while (lo._Lnext != up && (Geom.EdgeGoesLeft(lo._Lnext)
                        || Geom.EdgeSign(lo._Org, lo.Dst, lo._Lnext.Dst) <= 0.0f))
                    {
                        lo = _mesh.Connect(_pool, lo._Lnext, lo)._Sym;
                    }
                    lo = lo.Lprev;
                }
                else
                {
                    // lo.Org is on the left.  We can make CCW triangles from up.Dst.
                    while (lo._Lnext != up && (Geom.EdgeGoesRight(up.Lprev)
                        || Geom.EdgeSign(up.Dst, up._Org, up.Lprev._Org) >= 0.0f))
                    {
                        up = _mesh.Connect(_pool, up, up.Lprev)._Sym;
                    }
                    up = up._Lnext;
                }
            }

            // Now lo.Org == up.Dst == the leftmost vertex.  The remaining region
            // can be tessellated in a fan from this leftmost vertex.
            Debug.Assert(lo._Lnext != up);
            while (lo._Lnext._Lnext != up)
            {
                lo = _mesh.Connect(_pool, lo._Lnext, lo)._Sym;
            }
        }

        /// <summary>
        /// TessellateInterior( mesh ) tessellates each region of
        /// the mesh which is marked "inside" the polygon. Each such region
        /// must be monotone.
        /// </summary>
        private void TessellateInterior()
        {
            MeshUtils.Face f, next;
            for (f = _mesh._fHead._next; f != _mesh._fHead; f = next)
            {
                // Make sure we don't try to tessellate the new triangles.
                next = f._next;
                if (f._inside)
                {
                    TessellateMonoRegion(f);
                }
            }
        }


        private void OutputPolymesh(int polySize)
        {
            MeshUtils.Vertex v;
            MeshUtils.Face f;
            MeshUtils.Edge edge;
            int maxFaceCount = 0;
            int maxVertexCount = 0;
            int faceVerts, i;

            // Mark unused
            for (v = _mesh._vHead._next; v != _mesh._vHead; v = v._next)
                v._n = Undef;

            // Create unique IDs for all vertices and faces.
            for (f = _mesh._fHead._next; f != _mesh._fHead; f = f._next)
            {
                f._n = Undef;
                if (!f._inside) continue;

                var area = MeshUtils.FaceArea(f);
                if (Math.Abs(area) < double.Epsilon)
                {
                    continue;
                }

                edge = f._anEdge;
                faceVerts = 0;
                do
                {
                    v = edge._Org;
                    if (v._n == Undef)
                    {
                        v._n = maxVertexCount;
                        maxVertexCount++;
                    }
                    faceVerts++;
                    edge = edge._Lnext;
                }
                while (edge != f._anEdge);

                f._n = maxFaceCount;
                ++maxFaceCount;
            }

            ElementCount = maxFaceCount;
            Elements = new int[maxFaceCount * polySize];

            VertexCount = maxVertexCount;
            Vertices = new Vec3[VertexCount];

            // Output vertices.
            for (v = _mesh._vHead._next; v != _mesh._vHead; v = v._next)
            {
                if (v._n != Undef)
                {
                    // Store coordinate
                    Vertices[v._n] = v._coords;
                }
            }

            // Output indices.
            int elementIndex = 0;
            for (f = _mesh._fHead._next; f != _mesh._fHead; f = f._next)
            {
                if (!f._inside) continue;

                var area = MeshUtils.FaceArea(f);
                if (Math.Abs(area) < double.Epsilon)
                {
                    continue;
                }


                // Store polygon
                edge = f._anEdge;
                faceVerts = 0;
                do
                {
                    v = edge._Org;
                    Elements[elementIndex++] = v._n;
                    faceVerts++;
                    edge = edge._Lnext;
                } while (edge != f._anEdge);
                // Fill unused.
                for (i = faceVerts; i < polySize; ++i)
                {
                    Elements[elementIndex++] = Undef;
                }
            }
        }


        private double SignedArea(IList<Vec3> vertices)
        {
            double area = 0.0f;

            for (int i = 0; i < vertices.Count; i++)
            {
                var v0 = vertices[i];
                var v1 = vertices[(i + 1) % vertices.Count];

                area += v0.easting * v1.northing;
                area -= v0.northing * v1.easting;
            }

            return 0.5f * area;
        }

        public void AddContourInternal(IList<Vec3> vertices)
        {
            if (_mesh == null)
            {
                _mesh = _pool.Get<Mesh>();
            }

            bool reverse = SignedArea(vertices) < 0.0f;


            MeshUtils.Edge e = null;
            for (int i = 0; i < vertices.Count; ++i)
            {
                if (e == null)
                {
                    e = _mesh.MakeEdge(_pool);
                    _mesh.Splice(_pool, e, e._Sym);
                }
                else
                {
                    // Create a new vertex and edge which immediately follow e
                    // in the ordering around the left face.
                    _mesh.SplitEdge(_pool, e);
                    e = e._Lnext;
                }

                int index = reverse ? vertices.Count - 1 - i : i;
                // The new vertex is now e._Org.
                e._Org._coords = vertices[index];

                // The winding of an edge says how the winding number changes as we
                // cross from the edge's right face to its left face.  We add the
                // vertices in such an order that a CCW contour will add +1 to
                // the winding number of the region inside the contour.
                e._winding = 1;
                e._Sym._winding = -1;
            }
        }

        public void Tessellate()
        {
            Vertices = null;
            Elements = null;

            if (_mesh == null)
            {
                return;
            }

            // Determine the polygon normal and project vertices onto the plane
            // of the polygon.
            ProjectPolygon();

            // ComputeInterior computes the planar arrangement specified
            // by the given contours, and further subdivides this arrangement
            // into regions.  Each region is marked "inside" if it belongs
            // to the polygon.
            // Each interior region is guaranteed be monotone.
            ComputeInterior();

            TessellateInterior();

            OutputPolymesh(3);

            _pool.Return(_mesh);
            _mesh = null;
        }

        internal class ActiveRegion : IPooled<ActiveRegion>
        {
            internal MeshUtils.Edge _eUp;
            internal Dict<ActiveRegion>.Node _nodeUp;
            internal int _windingNumber;
            internal bool _inside, _sentinel, _dirty, _fixUpperEdge;

            public void Init(IPool pool)
            {
            }

            public void Reset(IPool pool)
            {
                _eUp = null;
                _nodeUp = null;
                _windingNumber = 0;
                _inside = false;
                _sentinel = false;
                _dirty = false;
                _fixUpperEdge = false;
            }
        }

        private ActiveRegion RegionBelow(ActiveRegion reg)
        {
            return reg._nodeUp._prev._key;
        }

        private ActiveRegion RegionAbove(ActiveRegion reg)
        {
            return reg._nodeUp._next._key;
        }

        /// <summary>
        /// Both edges must be directed from right to left (this is the canonical
        /// direction for the upper edge of each region).
        /// 
        /// The strategy is to evaluate a "t" value for each edge at the
        /// current sweep line position, given by tess->event. The calculations
        /// are designed to be very stable, but of course they are not perfect.
        /// 
        /// Special case: if both edge destinations are at the sweep event,
        /// we sort the edges by slope (they would otherwise compare equally).
        /// </summary>
        private bool EdgeLeq(ActiveRegion reg1, ActiveRegion reg2)
        {
            var e1 = reg1._eUp;
            var e2 = reg2._eUp;

            if (e1.Dst == _event)
            {
                if (e2.Dst == _event)
                {
                    // Two edges right of the sweep line which meet at the sweep event.
                    // Sort them by slope.
                    if (Geom.VertLeq(e1._Org, e2._Org))
                    {
                        return Geom.EdgeSign(e2.Dst, e1._Org, e2._Org) <= 0.0f;
                    }
                    return Geom.EdgeSign(e1.Dst, e2._Org, e1._Org) >= 0.0f;
                }
                return Geom.EdgeSign(e2.Dst, _event, e2._Org) <= 0.0f;
            }
            if (e2.Dst == _event)
            {
                return Geom.EdgeSign(e1.Dst, _event, e1._Org) >= 0.0f;
            }

            // General case - compute signed distance *from* e1, e2 to event
            var t1 = Geom.EdgeEval(e1.Dst, _event, e1._Org);
            var t2 = Geom.EdgeEval(e2.Dst, _event, e2._Org);
            return (t1 >= t2);
        }

        private void DeleteRegion(ActiveRegion reg)
        {
            if (reg._fixUpperEdge)
            {
                // It was created with zero winding number, so it better be
                // deleted with zero winding number (ie. it better not get merged
                // with a real edge).
                Debug.Assert(reg._eUp._winding == 0);
            }
            reg._eUp._activeRegion = null;
            _dict.Remove(reg._nodeUp);
            _pool.Return(reg);
        }

        /// <summary>
        /// Replace an upper edge which needs fixing (see ConnectRightVertex).
        /// </summary>
        private void FixUpperEdge(ActiveRegion reg, MeshUtils.Edge newEdge)
        {
            Debug.Assert(reg._fixUpperEdge);
            _mesh.Delete(_pool, reg._eUp);
            reg._fixUpperEdge = false;
            reg._eUp = newEdge;
            newEdge._activeRegion = reg;
        }

        private ActiveRegion TopLeftRegion(ActiveRegion reg)
        {
            var org = reg._eUp._Org;

            // Find the region above the uppermost edge with the same origin
            do
            {
                reg = RegionAbove(reg);
            } while (reg._eUp._Org == org);

            // If the edge above was a temporary edge introduced by ConnectRightVertex,
            // now is the time to fix it.
            if (reg._fixUpperEdge)
            {
                var e = _mesh.Connect(_pool, RegionBelow(reg)._eUp._Sym, reg._eUp._Lnext);
                FixUpperEdge(reg, e);
                reg = RegionAbove(reg);
            }

            return reg;
        }

        private ActiveRegion AddRegionBelow(ActiveRegion regAbove, MeshUtils.Edge eNewUp)
        {
            var regNew = _pool.Get<ActiveRegion>();

            regNew._eUp = eNewUp;
            regNew._nodeUp = _dict.InsertBefore(regAbove._nodeUp, regNew);
            regNew._fixUpperEdge = false;
            regNew._sentinel = false;
            regNew._dirty = false;

            eNewUp._activeRegion = regNew;

            return regNew;
        }

        private void ComputeWinding(ActiveRegion reg)
        {
            reg._windingNumber = RegionAbove(reg)._windingNumber + reg._eUp._winding;
            reg._inside = reg._windingNumber > 0;
        }

        private void FinishRegion(ActiveRegion reg)
        {
            var e = reg._eUp;
            var f = e._Lface;

            f._inside = reg._inside;
            f._anEdge = e;
            DeleteRegion(reg);
        }

        private MeshUtils.Edge FinishLeftRegions(ActiveRegion regFirst, ActiveRegion regLast)
        {
            var regPrev = regFirst;
            var ePrev = regFirst._eUp;

            while (regPrev != regLast)
            {
                regPrev._fixUpperEdge = false;	// placement was OK
                var reg = RegionBelow(regPrev);
                var e = reg._eUp;
                if (e._Org != ePrev._Org)
                {
                    if (!reg._fixUpperEdge)
                    {
                        // Remove the last left-going edge.  Even though there are no further
                        // edges in the dictionary with this origin, there may be further
                        // such edges in the mesh (if we are adding left edges to a vertex
                        // that has already been processed).  Thus it is important to call
                        // FinishRegion rather than just DeleteRegion.
                        FinishRegion(regPrev);
                        break;
                    }
                    // If the edge below was a temporary edge introduced by
                    // ConnectRightVertex, now is the time to fix it.
                    e = _mesh.Connect(_pool, ePrev.Lprev, e._Sym);
                    FixUpperEdge(reg, e);
                }

                // Relink edges so that ePrev.Onext == e
                if (ePrev._Onext != e)
                {
                    _mesh.Splice(_pool, e.Oprev, e);
                    _mesh.Splice(_pool, ePrev, e);
                }
                FinishRegion(regPrev); // may change reg.eUp
                ePrev = reg._eUp;
                regPrev = reg;
            }

            return ePrev;
        }

        private void AddRightEdges(ActiveRegion regUp, MeshUtils.Edge eFirst, MeshUtils.Edge eLast, MeshUtils.Edge eTopLeft, bool cleanUp)
        {
            bool firstTime = true;

            var e = eFirst; do
            {
                Debug.Assert(Geom.VertLeq(e._Org, e.Dst));
                AddRegionBelow(regUp, e._Sym);
                e = e._Onext;
            } while (e != eLast);

            // Walk *all* right-going edges from e.Org, in the dictionary order,
            // updating the winding numbers of each region, and re-linking the mesh
            // edges to match the dictionary ordering (if necessary).
            if (eTopLeft == null)
            {
                eTopLeft = RegionBelow(regUp)._eUp.Rprev;
            }

            ActiveRegion regPrev = regUp, reg;
            var ePrev = eTopLeft;
            while (true)
            {
                reg = RegionBelow(regPrev);
                e = reg._eUp._Sym;
                if (e._Org != ePrev._Org) break;

                if (e._Onext != ePrev)
                {
                    // Unlink e from its current position, and relink below ePrev
                    _mesh.Splice(_pool, e.Oprev, e);
                    _mesh.Splice(_pool, ePrev.Oprev, e);
                }
                // Compute the winding number and "inside" flag for the new regions
                reg._windingNumber = regPrev._windingNumber - e._winding;
                reg._inside = reg._windingNumber > 0;

                // Check for two outgoing edges with same slope -- process these
                // before any intersection tests (see example in tessComputeInterior).
                regPrev._dirty = true;
                if (!firstTime && CheckForRightSplice(regPrev))
                {
                    Geom.AddWinding(e, ePrev);
                    DeleteRegion(regPrev);
                    _mesh.Delete(_pool, ePrev);
                }
                firstTime = false;
                regPrev = reg;
                ePrev = e;
            }
            regPrev._dirty = true;
            Debug.Assert(regPrev._windingNumber - e._winding == reg._windingNumber);

            if (cleanUp)
            {
                // Check for intersections between newly adjacent edges.
                WalkDirtyRegions(regPrev);
            }
        }

        private void SpliceMergeVertices(MeshUtils.Edge e1, MeshUtils.Edge e2)
        {
            _mesh.Splice(_pool, e1, e2);
        }


        private bool CheckForRightSplice(ActiveRegion regUp)
        {
            var regLo = RegionBelow(regUp);
            var eUp = regUp._eUp;
            var eLo = regLo._eUp;

            if (Geom.VertLeq(eUp._Org, eLo._Org))
            {
                if (Geom.EdgeSign(eLo.Dst, eUp._Org, eLo._Org) > 0.0f)
                {
                    return false;
                }

                // eUp.Org appears to be below eLo
                if (!Geom.VertEq(eUp._Org, eLo._Org))
                {
                    // Splice eUp._Org into eLo
                    _mesh.SplitEdge(_pool, eLo._Sym);
                    _mesh.Splice(_pool, eUp, eLo.Oprev);
                    regUp._dirty = regLo._dirty = true;
                }
                else if (eUp._Org != eLo._Org)
                {
                    // merge the two vertices, discarding eUp.Org
                    _pq.Remove(eUp._Org._pqHandle);
                    SpliceMergeVertices(eLo.Oprev, eUp);
                }
            }
            else
            {
                if (Geom.EdgeSign(eUp.Dst, eLo._Org, eUp._Org) < 0.0f)
                {
                    return false;
                }

                // eLo.Org appears to be above eUp, so splice eLo.Org into eUp
                RegionAbove(regUp)._dirty = regUp._dirty = true;
                _mesh.SplitEdge(_pool, eUp._Sym);
                _mesh.Splice(_pool, eLo.Oprev, eUp);
            }
            return true;
        }

        private bool CheckForLeftSplice(ActiveRegion regUp)
        {
            var regLo = RegionBelow(regUp);
            var eUp = regUp._eUp;
            var eLo = regLo._eUp;

            Debug.Assert(!Geom.VertEq(eUp.Dst, eLo.Dst));

            if (Geom.VertLeq(eUp.Dst, eLo.Dst))
            {
                if (Geom.EdgeSign(eUp.Dst, eLo.Dst, eUp._Org) < 0.0f)
                {
                    return false;
                }

                // eLo.Dst is above eUp, so splice eLo.Dst into eUp
                RegionAbove(regUp)._dirty = regUp._dirty = true;
                var e = _mesh.SplitEdge(_pool, eUp);
                _mesh.Splice(_pool, eLo._Sym, e);
                e._Lface._inside = regUp._inside;
            }
            else
            {
                if (Geom.EdgeSign(eLo.Dst, eUp.Dst, eLo._Org) > 0.0f)
                {
                    return false;
                }

                // eUp.Dst is below eLo, so splice eUp.Dst into eLo
                regUp._dirty = regLo._dirty = true;
                var e = _mesh.SplitEdge(_pool, eLo);
                _mesh.Splice(_pool, eUp._Lnext, eLo._Sym);
                e.Rface._inside = regUp._inside;
            }
            return true;
        }

        private bool CheckForIntersect(ActiveRegion regUp)
        {
            var regLo = RegionBelow(regUp);
            var eUp = regUp._eUp;
            var eLo = regLo._eUp;
            var orgUp = eUp._Org;
            var orgLo = eLo._Org;
            var dstUp = eUp.Dst;
            var dstLo = eLo.Dst;

            Debug.Assert(!Geom.VertEq(dstLo, dstUp));
            Debug.Assert(Geom.EdgeSign(dstUp, _event, orgUp) <= 0.0f);
            Debug.Assert(Geom.EdgeSign(dstLo, _event, orgLo) >= 0.0f);
            Debug.Assert(orgUp != _event && orgLo != _event);
            Debug.Assert(!regUp._fixUpperEdge && !regLo._fixUpperEdge);

            if (orgUp == orgLo)
            {
                // right endpoints are the same
                return false;
            }

            var tMinUp = Math.Min(orgUp._t, dstUp._t);
            var tMaxLo = Math.Max(orgLo._t, dstLo._t);
            if (tMinUp > tMaxLo)
            {
                // t ranges do not overlap
                return false;
            }

            throw new InvalidOperationException("should not be invalid");
        }

        private void WalkDirtyRegions(ActiveRegion regUp)
        {
            var regLo = RegionBelow(regUp);
            MeshUtils.Edge eUp, eLo;

            while (true)
            {
                // Find the lowest dirty region (we walk from the bottom up).
                while (regLo._dirty)
                {
                    regUp = regLo;
                    regLo = RegionBelow(regLo);
                }
                if (!regUp._dirty)
                {
                    regLo = regUp;
                    regUp = RegionAbove(regUp);
                    if (regUp == null || !regUp._dirty)
                    {
                        // We've walked all the dirty regions
                        return;
                    }
                }
                regUp._dirty = false;
                eUp = regUp._eUp;
                eLo = regLo._eUp;

                if (eUp.Dst != eLo.Dst)
                {
                    // Check that the edge ordering is obeyed at the Dst vertices.
                    if (CheckForLeftSplice(regUp))
                    {

                        // If the upper or lower edge was marked fixUpperEdge, then
                        // we no longer need it (since these edges are needed only for
                        // vertices which otherwise have no right-going edges).
                        if (regLo._fixUpperEdge)
                        {
                            DeleteRegion(regLo);
                            _mesh.Delete(_pool, eLo);
                            regLo = RegionBelow(regUp);
                            eLo = regLo._eUp;
                        }
                        else if (regUp._fixUpperEdge)
                        {
                            DeleteRegion(regUp);
                            _mesh.Delete(_pool, eUp);
                            regUp = RegionAbove(regLo);
                            eUp = regUp._eUp;
                        }
                    }
                }
                if (eUp._Org != eLo._Org)
                {
                    if (eUp.Dst != eLo.Dst
                        && !regUp._fixUpperEdge && !regLo._fixUpperEdge
                        && (eUp.Dst == _event || eLo.Dst == _event))
                    {
                        // When all else fails in CheckForIntersect(), it uses tess._event
                        // as the intersection location. To make this possible, it requires
                        // that tess._event lie between the upper and lower edges, and also
                        // that neither of these is marked fixUpperEdge (since in the worst
                        // case it might splice one of these edges into tess.event, and
                        // violate the invariant that fixable edges are the only right-going
                        // edge from their associated vertex).
                        if (CheckForIntersect(regUp))
                        {
                            // WalkDirtyRegions() was called recursively; we're done
                            return;
                        }
                    }
                    else
                    {
                        // Even though we can't use CheckForIntersect(), the Org vertices
                        // may violate the dictionary edge ordering. Check and correct this.
                        CheckForRightSplice(regUp);
                    }
                }
                if (eUp._Org == eLo._Org && eUp.Dst == eLo.Dst)
                {
                    // A degenerate loop consisting of only two edges -- delete it.
                    Geom.AddWinding(eLo, eUp);
                    DeleteRegion(regUp);
                    _mesh.Delete(_pool, eUp);
                    regUp = RegionAbove(regLo);
                }
            }
        }

        private void ConnectRightVertex(ActiveRegion regUp, MeshUtils.Edge eBottomLeft)
        {
            var eTopLeft = eBottomLeft._Onext;
            var regLo = RegionBelow(regUp);
            var eUp = regUp._eUp;
            var eLo = regLo._eUp;
            bool degenerate = false;

            if (eUp.Dst != eLo.Dst)
            {
                CheckForIntersect(regUp);
            }

            // Possible new degeneracies: upper or lower edge of regUp may pass
            // through vEvent, or may coincide with new intersection vertex
            if (Geom.VertEq(eUp._Org, _event))
            {
                _mesh.Splice(_pool, eTopLeft.Oprev, eUp);
                regUp = TopLeftRegion(regUp);
                eTopLeft = RegionBelow(regUp)._eUp;
                FinishLeftRegions(RegionBelow(regUp), regLo);
                degenerate = true;
            }
            if (Geom.VertEq(eLo._Org, _event))
            {
                _mesh.Splice(_pool, eBottomLeft, eLo.Oprev);
                eBottomLeft = FinishLeftRegions(regLo, null);
                degenerate = true;
            }
            if (degenerate)
            {
                AddRightEdges(regUp, eBottomLeft._Onext, eTopLeft, eTopLeft, true);
                return;
            }

            // Non-degenerate situation -- need to add a temporary, fixable edge.
            // Connect to the closer of eLo.Org, eUp.Org.
            MeshUtils.Edge eNew;
            if (Geom.VertLeq(eLo._Org, eUp._Org))
            {
                eNew = eLo.Oprev;
            }
            else
            {
                eNew = eUp;
            }
            eNew = _mesh.Connect(_pool, eBottomLeft.Lprev, eNew);

            // Prevent cleanup, otherwise eNew might disappear before we've even
            // had a chance to mark it as a temporary edge.
            AddRightEdges(regUp, eNew, eNew._Onext, eNew._Onext, false);
            eNew._Sym._activeRegion._fixUpperEdge = true;
            WalkDirtyRegions(regUp);
        }

        private void ConnectLeftDegenerate(ActiveRegion regUp, MeshUtils.Vertex vEvent)
        {
            var e = regUp._eUp;
            if (Geom.VertEq(e._Org, vEvent))
            {
                // e.Org is an unprocessed vertex - just combine them, and wait
                // for e.Org to be pulled from the queue
                // C# : in the C version, there is a flag but it was never implemented
                // the vertices are before beginning the tessellation
                throw new InvalidOperationException("Vertices should have been merged before");
            }

            if (!Geom.VertEq(e.Dst, vEvent))
            {
                // General case -- splice vEvent into edge e which passes through it
                _mesh.SplitEdge(_pool, e._Sym);
                if (regUp._fixUpperEdge)
                {
                    // This edge was fixable -- delete unused portion of original edge
                    _mesh.Delete(_pool, e._Onext);
                    regUp._fixUpperEdge = false;
                }
                _mesh.Splice(_pool, vEvent._anEdge, e);
                SweepEvent(vEvent);	// recurse
                return;
            }

            // See above
            throw new InvalidOperationException("Vertices should have been merged before");
        }

        private void ConnectLeftVertex(MeshUtils.Vertex vEvent)
        {
            var tmp = _pool.Get<ActiveRegion>();

            // Get a pointer to the active region containing vEvent
            tmp._eUp = vEvent._anEdge._Sym;
            var regUp = _dict.Find(tmp).Key;
            _pool.Return(tmp);
            var regLo = RegionBelow(regUp);
            if (regLo == null)
            {
                // This may happen if the input polygon is coplanar.
                return;
            }
            var eUp = regUp._eUp;
            var eLo = regLo._eUp;

            // Try merging with U or L first
            if (Geom.EdgeSign(eUp.Dst, vEvent, eUp._Org) == 0.0f)
            {
                ConnectLeftDegenerate(regUp, vEvent);
                return;
            }

            // Connect vEvent to rightmost processed vertex of either chain.
            // e._Dst is the vertex that we will connect to vEvent.
            var reg = Geom.VertLeq(eLo.Dst, eUp.Dst) ? regUp : regLo;

            if (regUp._inside || reg._fixUpperEdge)
            {
                MeshUtils.Edge eNew;
                if (reg == regUp)
                {
                    eNew = _mesh.Connect(_pool, vEvent._anEdge._Sym, eUp._Lnext);
                }
                else
                {
                    eNew = _mesh.Connect(_pool, eLo.Dnext, vEvent._anEdge)._Sym;
                }
                if (reg._fixUpperEdge)
                {
                    FixUpperEdge(reg, eNew);
                }
                else
                {
                    ComputeWinding(AddRegionBelow(regUp, eNew));
                }
                SweepEvent(vEvent);
            }
            else
            {
                // The new vertex is in a region which does not belong to the polygon.
                // We don't need to connect this vertex to the rest of the mesh.
                AddRightEdges(regUp, vEvent._anEdge, vEvent._anEdge, null, true);
            }
        }

        private void SweepEvent(MeshUtils.Vertex vEvent)
        {
            _event = vEvent;

            // Check if this vertex is the right endpoint of an edge that is
            // already in the dictionary. In this case we don't need to waste
            // time searching for the location to insert new edges.
            var e = vEvent._anEdge;
            while (e._activeRegion == null)
            {
                e = e._Onext;
                if (e == vEvent._anEdge)
                {
                    // All edges go right -- not incident to any processed edges
                    ConnectLeftVertex(vEvent);
                    return;
                }
            }

            // Processing consists of two phases: first we "finish" all the
            // active regions where both the upper and lower edges terminate
            // at vEvent (ie. vEvent is closing off these regions).
            // We mark these faces "inside" or "outside" the polygon according
            // to their winding number, and delete the edges from the dictionary.
            // This takes care of all the left-going edges from vEvent.
            var regUp = TopLeftRegion(e._activeRegion);
            var reg = RegionBelow(regUp);
            var eTopLeft = reg._eUp;
            var eBottomLeft = FinishLeftRegions(reg, null);

            // Next we process all the right-going edges from vEvent. This
            // involves adding the edges to the dictionary, and creating the
            // associated "active regions" which record information about the
            // regions between adjacent dictionary edges.
            if (eBottomLeft._Onext == eTopLeft)
            {
                // No right-going edges -- add a temporary "fixable" edge
                ConnectRightVertex(regUp, eBottomLeft);
            }
            else
            {
                AddRightEdges(regUp, eBottomLeft._Onext, eTopLeft, eTopLeft, true);
            }
        }

        private void AddSentinel(double smin, double smax, double t)
        {
            var e = _mesh.MakeEdge(_pool);
            e._Org._s = smax;
            e._Org._t = t;
            e.Dst._s = smin;
            e.Dst._t = t;
            _event = e.Dst; // initialize it

            var reg = _pool.Get<ActiveRegion>();
            reg._eUp = e;
            reg._windingNumber = 0;
            reg._inside = false;
            reg._fixUpperEdge = false;
            reg._sentinel = true;
            reg._dirty = false;
            reg._nodeUp = _dict.Insert(reg);
        }

        private void InitEdgeDict()
        {
            _dict = new Dict<ActiveRegion>(EdgeLeq);

            AddSentinel(-SentinelCoord, SentinelCoord, -SentinelCoord);
            AddSentinel(-SentinelCoord, SentinelCoord, +SentinelCoord);
        }

        private void DoneEdgeDict()
        {
            int fixedEdges = 0;

            ActiveRegion reg;
            while ((reg = _dict.Min().Key) != null)
            {
                // At the end of all processing, the dictionary should contain
                // only the two sentinel edges, plus at most one "fixable" edge
                // created by ConnectRightVertex().
                if (!reg._sentinel)
                {
                    Debug.Assert(reg._fixUpperEdge);
                    Debug.Assert(++fixedEdges == 1);
                }
                Debug.Assert(reg._windingNumber == 0);
                DeleteRegion(reg);
            }

            _dict = null;
        }

        private void RemoveDegenerateEdges()
        {
            MeshUtils.Edge eHead = _mesh._eHead, e, eNext, eLnext;

            for (e = eHead._next; e != eHead; e = eNext)
            {
                eNext = e._next;
                eLnext = e._Lnext;

                if (Geom.VertEq(e._Org, e.Dst) && e._Lnext._Lnext != e)
                {
                    // Zero-length edge, contour has at least 3 edges

                    SpliceMergeVertices(eLnext, e);	// deletes e.Org
                    _mesh.Delete(_pool, e); // e is a self-loop
                    e = eLnext;
                    eLnext = e._Lnext;
                }
                if (eLnext._Lnext == e)
                {
                    // Degenerate contour (one or two edges)

                    if (eLnext != e)
                    {
                        if (eLnext == eNext || eLnext == eNext._Sym)
                        {
                            eNext = eNext._next;
                        }
                        _mesh.Delete(_pool, eLnext);
                    }
                    if (e == eNext || e == eNext._Sym)
                    {
                        eNext = eNext._next;
                    }
                    _mesh.Delete(_pool, e);
                }
            }
        }

        private void InitPriorityQ()
        {
            MeshUtils.Vertex vHead = _mesh._vHead, v;
            int vertexCount = 0;

            for (v = vHead._next; v != vHead; v = v._next)
            {
                vertexCount++;
            }
            // Make sure there is enough space for sentinels.
            vertexCount += 8;

            _pq = new PriorityQueue<MeshUtils.Vertex>(vertexCount, Geom.VertLeq);

            vHead = _mesh._vHead;
            for (v = vHead._next; v != vHead; v = v._next)
            {
                v._pqHandle = _pq.Insert(v);
                if (v._pqHandle._handle == PQHandle.Invalid)
                {
                    throw new InvalidOperationException("PQHandle should not be invalid");
                }
            }
            _pq.Init();
        }

        private void DonePriorityQ()
        {
            _pq = null;
        }

        private void RemoveDegenerateFaces()
        {
            MeshUtils.Face f, fNext;
            MeshUtils.Edge e;

            for (f = _mesh._fHead._next; f != _mesh._fHead; f = fNext)
            {
                fNext = f._next;
                e = f._anEdge;
                Debug.Assert(e._Lnext != e);

                if (e._Lnext._Lnext == e)
                {
                    // A face with only two edges
                    Geom.AddWinding(e._Onext, e);
                    _mesh.Delete(_pool, e);
                }
            }
        }

        protected void ComputeInterior()
        {
            // Each vertex defines an event for our sweep line. Start by inserting
            // all the vertices in a priority queue. Events are processed in
            // lexicographic order, ie.
            // 
            // e1 < e2  iff  e1.x < e2.x || (e1.x == e2.x && e1.y < e2.y)
            RemoveDegenerateEdges();
            InitPriorityQ();
            RemoveDegenerateFaces();
            InitEdgeDict();

            MeshUtils.Vertex v, vNext;
            while ((v = _pq.ExtractMin()) != null)
            {
                while (true)
                {
                    vNext = _pq.Minimum();
                    if (vNext == null || !Geom.VertEq(vNext, v))
                    {
                        break;
                    }

                    // Merge together all vertices at exactly the same location.
                    // This is more efficient than processing them one at a time,
                    // simplifies the code (see ConnectLeftDegenerate), and is also
                    // important for correct handling of certain degenerate cases.
                    // For example, suppose there are two identical edges A and B
                    // that belong to different contours (so without this code they would
                    // be processed by separate sweep events). Suppose another edge C
                    // crosses A and B from above. When A is processed, we split it
                    // at its intersection point with C. However this also splits C,
                    // so when we insert B we may compute a slightly different
                    // intersection point. This might leave two edges with a small
                    // gap between them. This kind of error is especially obvious
                    // when using boundary extraction (BoundaryOnly).
                    vNext = _pq.ExtractMin();
                    SpliceMergeVertices(v._anEdge, vNext._anEdge);
                }
                SweepEvent(v);
            }

            DoneEdgeDict();
            DonePriorityQ();

            RemoveDegenerateFaces();
        }
    }

    internal class Dict<TValue> where TValue : class
    {
        public class Node
        {
            internal TValue _key;
            internal Node _prev, _next;

            public TValue Key { get { return _key; } }
        }

        public delegate bool LessOrEqual(TValue lhs, TValue rhs);

        private readonly LessOrEqual _leq;
        readonly Node _head;

        public Dict(LessOrEqual leq)
        {
            _leq = leq;

            _head = new Node { _key = null };
            _head._prev = _head;
            _head._next = _head;
        }

        public Node Insert(TValue key)
        {
            return InsertBefore(_head, key);
        }

        public Node InsertBefore(Node node, TValue key)
        {
            do
            {
                node = node._prev;
            } while (node._key != null && !_leq(node._key, key));

            var newNode = new Node { _key = key };
            newNode._next = node._next;
            node._next._prev = newNode;
            newNode._prev = node;
            node._next = newNode;

            return newNode;
        }

        public Node Find(TValue key)
        {
            var node = _head;
            do
            {
                node = node._next;
            } while (node._key != null && !_leq(key, node._key));
            return node;
        }

        public Node Min()
        {
            return _head._next;
        }

        public void Remove(Node node)
        {
            node._next._prev = node._prev;
            node._prev._next = node._next;
        }
    }

    internal static class Geom
    {

        public static bool VertCCW(MeshUtils.Vertex u, MeshUtils.Vertex v, MeshUtils.Vertex w)
        {
            return (u._s * (v._t - w._t) + v._s * (w._t - u._t) + w._s * (u._t - v._t)) >= 0.0f;
        }
        public static bool VertEq(MeshUtils.Vertex lhs, MeshUtils.Vertex rhs)
        {
            return lhs._s == rhs._s && lhs._t == rhs._t;
        }
        public static bool VertLeq(MeshUtils.Vertex lhs, MeshUtils.Vertex rhs)
        {
            return (lhs._s < rhs._s) || (lhs._s == rhs._s && lhs._t <= rhs._t);
        }

        /// <summary>
        /// Given three vertices u,v,w such that VertLeq(u,v) && VertLeq(v,w),
        /// evaluates the t-coord of the edge uw at the s-coord of the vertex v.
        /// Returns v->t - (uw)(v->s), ie. the signed distance from uw to v.
        /// If uw is vertical (and thus passes through v), the result is zero.
        /// 
        /// The calculation is extremely accurate and stable, even when v
        /// is very close to u or w.  In particular if we set v->t = 0 and
        /// let r be the negated result (this evaluates (uw)(v->s)), then
        /// r is guaranteed to satisfy MIN(u->t,w->t) <= r <= MAX(u->t,w->t).
        /// </summary>
        public static double EdgeEval(MeshUtils.Vertex u, MeshUtils.Vertex v, MeshUtils.Vertex w)
        {
            Debug.Assert(VertLeq(u, v) && VertLeq(v, w));

            var gapL = v._s - u._s;
            var gapR = w._s - v._s;

            if (gapL + gapR > 0.0f)
            {
                if (gapL < gapR)
                {
                    return (v._t - u._t) + (u._t - w._t) * (gapL / (gapL + gapR));
                }
                else
                {
                    return (v._t - w._t) + (w._t - u._t) * (gapR / (gapL + gapR));
                }
            }
            /* vertical line */
            return 0;
        }

        /// <summary>
        /// Returns a number whose sign matches EdgeEval(u,v,w) but which
        /// is cheaper to evaluate. Returns > 0, == 0 , or < 0
        /// as v is above, on, or below the edge uw.
        /// </summary>
        public static double EdgeSign(MeshUtils.Vertex u, MeshUtils.Vertex v, MeshUtils.Vertex w)
        {
            Debug.Assert(VertLeq(u, v) && VertLeq(v, w));

            var gapL = v._s - u._s;
            var gapR = w._s - v._s;

            if (gapL + gapR > 0.0f)
            {
                return (v._t - w._t) * gapL + (v._t - u._t) * gapR;
            }
            /* vertical line */
            return 0;
        }

        public static bool TransLeq(MeshUtils.Vertex lhs, MeshUtils.Vertex rhs)
        {
            return (lhs._t < rhs._t) || (lhs._t == rhs._t && lhs._s <= rhs._s);
        }

        public static double TransEval(MeshUtils.Vertex u, MeshUtils.Vertex v, MeshUtils.Vertex w)
        {
            Debug.Assert(TransLeq(u, v) && TransLeq(v, w));

            var gapL = v._t - u._t;
            var gapR = w._t - v._t;

            if (gapL + gapR > 0.0f)
            {
                if (gapL < gapR)
                {
                    return (v._s - u._s) + (u._s - w._s) * (gapL / (gapL + gapR));
                }
                else
                {
                    return (v._s - w._s) + (w._s - u._s) * (gapR / (gapL + gapR));
                }
            }
            /* vertical line */
            return 0;
        }

        public static double TransSign(MeshUtils.Vertex u, MeshUtils.Vertex v, MeshUtils.Vertex w)
        {
            Debug.Assert(TransLeq(u, v) && TransLeq(v, w));

            var gapL = v._t - u._t;
            var gapR = w._t - v._t;

            if (gapL + gapR > 0.0f)
            {
                return (v._s - w._s) * gapL + (v._s - u._s) * gapR;
            }
            /* vertical line */
            return 0;
        }

        public static bool EdgeGoesLeft(MeshUtils.Edge e)
        {
            return VertLeq(e.Dst, e._Org);
        }

        public static bool EdgeGoesRight(MeshUtils.Edge e)
        {
            return VertLeq(e._Org, e.Dst);
        }

        public static double VertL1dist(MeshUtils.Vertex u, MeshUtils.Vertex v)
        {
            return Math.Abs(u._s - v._s) + Math.Abs(u._t - v._t);
        }

        public static void AddWinding(MeshUtils.Edge eDst, MeshUtils.Edge eSrc)
        {
            eDst._winding += eSrc._winding;
            eDst._Sym._winding += eSrc._Sym._winding;
        }

        public static double Interpolate(double a, double x, double b, double y)
        {
            if (a < 0.0f)
            {
                a = 0.0f;
            }
            if (b < 0.0f)
            {
                b = 0.0f;
            }
            return ((a <= b) ? ((b == 0.0f) ? ((x + y) / 2.0f)
                    : (x + (y - x) * (a / (a + b))))
                    : (y + (x - y) * (b / (a + b))));
        }

        static void Swap(ref MeshUtils.Vertex a, ref MeshUtils.Vertex b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }

        /// <summary>
        /// Given edges (o1,d1) and (o2,d2), compute their point of intersection.
        /// The computed point is guaranteed to lie in the intersection of the
        /// bounding rectangles defined by each edge.
        /// </summary>
        public static void EdgeIntersect(MeshUtils.Vertex o1, MeshUtils.Vertex d1, MeshUtils.Vertex o2, MeshUtils.Vertex d2, MeshUtils.Vertex v)
        {
            // This is certainly not the most efficient way to find the intersection
            // of two line segments, but it is very numerically stable.
            // 
            // Strategy: find the two middle vertices in the VertLeq ordering,
            // and interpolate the intersection s-value from these.  Then repeat
            // using the TransLeq ordering to find the intersection t-value.

            if (!VertLeq(o1, d1)) { Swap(ref o1, ref d1); }
            if (!VertLeq(o2, d2)) { Swap(ref o2, ref d2); }
            if (!VertLeq(o1, o2)) { Swap(ref o1, ref o2); Swap(ref d1, ref d2); }

            if (!VertLeq(o2, d1))
            {
                // Technically, no intersection -- do our best
                v._s = (o2._s + d1._s) / 2.0f;
            }
            else if (VertLeq(d1, d2))
            {
                // Interpolate between o2 and d1
                var z1 = EdgeEval(o1, o2, d1);
                var z2 = EdgeEval(o2, d1, d2);
                if (z1 + z2 < 0.0f)
                {
                    z1 = -z1;
                    z2 = -z2;
                }
                v._s = Interpolate(z1, o2._s, z2, d1._s);
            }
            else
            {
                // Interpolate between o2 and d2
                var z1 = EdgeSign(o1, o2, d1);
                var z2 = -EdgeSign(o1, d2, d1);
                if (z1 + z2 < 0.0f)
                {
                    z1 = -z1;
                    z2 = -z2;
                }
                v._s = Interpolate(z1, o2._s, z2, d2._s);
            }

            // Now repeat the process for t

            if (!TransLeq(o1, d1)) { Swap(ref o1, ref d1); }
            if (!TransLeq(o2, d2)) { Swap(ref o2, ref d2); }
            if (!TransLeq(o1, o2)) { Swap(ref o1, ref o2); Swap(ref d1, ref d2); }

            if (!TransLeq(o2, d1))
            {
                // Technically, no intersection -- do our best
                v._t = (o2._t + d1._t) / 2.0f;
            }
            else if (TransLeq(d1, d2))
            {
                // Interpolate between o2 and d1
                var z1 = TransEval(o1, o2, d1);
                var z2 = TransEval(o2, d1, d2);
                if (z1 + z2 < 0.0f)
                {
                    z1 = -z1;
                    z2 = -z2;
                }
                v._t = Interpolate(z1, o2._t, z2, d1._t);
            }
            else
            {
                // Interpolate between o2 and d2
                var z1 = TransSign(o1, o2, d1);
                var z2 = -TransSign(o1, d2, d1);
                if (z1 + z2 < 0.0f)
                {
                    z1 = -z1;
                    z2 = -z2;
                }
                v._t = Interpolate(z1, o2._t, z2, d2._t);
            }
        }
    }

    internal class Mesh : IPooled<Mesh>
    {
        internal MeshUtils.Vertex _vHead;
        internal MeshUtils.Face _fHead;
        internal MeshUtils.Edge _eHead, _eHeadSym;

        public void Init(IPool pool)
        {
            var v = _vHead = pool.Get<MeshUtils.Vertex>();
            var f = _fHead = pool.Get<MeshUtils.Face>();

            var pair = MeshUtils.EdgePair.Create(pool);
            var e = _eHead = pair._e;
            var eSym = _eHeadSym = pair._eSym;

            v._next = v._prev = v;
            v._anEdge = null;

            f._next = f._prev = f;
            f._anEdge = null;
            f._trail = null;
            f._marked = false;
            f._inside = false;

            e._next = e;
            e._Sym = eSym;
            e._Onext = null;
            e._Lnext = null;
            e._Org = null;
            e._Lface = null;
            e._winding = 0;
            e._activeRegion = null;

            eSym._next = eSym;
            eSym._Sym = e;
            eSym._Onext = null;
            eSym._Lnext = null;
            eSym._Org = null;
            eSym._Lface = null;
            eSym._winding = 0;
            eSym._activeRegion = null;
        }

        public void Reset(IPool pool)
        {
            MeshUtils.Face fNext;
            for (MeshUtils.Face f = _fHead; f._next != null; f = fNext)
            {
                fNext = f._next;
                pool.Return(f);
            }

            MeshUtils.Vertex vNext;
            for (MeshUtils.Vertex v = _vHead; v._next != null; v = vNext)
            {
                vNext = v._next;
                pool.Return(v);
            }

            MeshUtils.Edge eNext;
            for (MeshUtils.Edge e = _eHead; e._next != null; e = eNext)
            {
                eNext = e._next;
                pool.Return(e._Sym);
                pool.Return(e);
            }

            _vHead = null;
            _fHead = null;
            _eHead = _eHeadSym = null;
        }

        /// <summary>
        /// Creates one edge, two vertices and a loop (face).
        /// The loop consists of the two new half-edges.
        /// </summary>
        public MeshUtils.Edge MakeEdge(IPool pool)
        {
            var e = MeshUtils.MakeEdge(pool, _eHead);

            MeshUtils.MakeVertex(pool, e, _vHead);
            MeshUtils.MakeVertex(pool, e._Sym, _vHead);
            MeshUtils.MakeFace(pool, e, _fHead);

            return e;
        }

        /// <summary>
        /// Splice is the basic operation for changing the
        /// mesh connectivity and topology.  It changes the mesh so that
        ///     eOrg->Onext = OLD( eDst->Onext )
        ///     eDst->Onext = OLD( eOrg->Onext )
        /// where OLD(...) means the value before the meshSplice operation.
        /// 
        /// This can have two effects on the vertex structure:
        ///  - if eOrg->Org != eDst->Org, the two vertices are merged together
        ///  - if eOrg->Org == eDst->Org, the origin is split into two vertices
        /// In both cases, eDst->Org is changed and eOrg->Org is untouched.
        /// 
        /// Similarly (and independently) for the face structure,
        ///  - if eOrg->Lface == eDst->Lface, one loop is split into two
        ///  - if eOrg->Lface != eDst->Lface, two distinct loops are joined into one
        /// In both cases, eDst->Lface is changed and eOrg->Lface is unaffected.
        /// 
        /// Some special cases:
        /// If eDst == eOrg, the operation has no effect.
        /// If eDst == eOrg->Lnext, the new face will have a single edge.
        /// If eDst == eOrg->Lprev, the old face will have a single edge.
        /// If eDst == eOrg->Onext, the new vertex will have a single edge.
        /// If eDst == eOrg->Oprev, the old vertex will have a single edge.
        /// </summary>
        public void Splice(IPool pool, MeshUtils.Edge eOrg, MeshUtils.Edge eDst)
        {
            if (eOrg == eDst)
            {
                return;
            }

            bool joiningVertices = false;
            if (eDst._Org != eOrg._Org)
            {
                // We are merging two disjoint vertices -- destroy eDst->Org
                joiningVertices = true;
                MeshUtils.KillVertex(pool, eDst._Org, eOrg._Org);
            }
            bool joiningLoops = false;
            if (eDst._Lface != eOrg._Lface)
            {
                // We are connecting two disjoint loops -- destroy eDst->Lface
                joiningLoops = true;
                MeshUtils.KillFace(pool, eDst._Lface, eOrg._Lface);
            }

            // Change the edge structure
            MeshUtils.Splice(eDst, eOrg);

            if (!joiningVertices)
            {
                // We split one vertex into two -- the new vertex is eDst->Org.
                // Make sure the old vertex points to a valid half-edge.
                MeshUtils.MakeVertex(pool, eDst, eOrg._Org);
                eOrg._Org._anEdge = eOrg;
            }
            if (!joiningLoops)
            {
                // We split one loop into two -- the new loop is eDst->Lface.
                // Make sure the old face points to a valid half-edge.
                MeshUtils.MakeFace(pool, eDst, eOrg._Lface);
                eOrg._Lface._anEdge = eOrg;
            }
        }

        /// <summary>
        /// Removes the edge eDel. There are several cases:
        /// if (eDel->Lface != eDel->Rface), we join two loops into one; the loop
        /// eDel->Lface is deleted. Otherwise, we are splitting one loop into two;
        /// the newly created loop will contain eDel->Dst. If the deletion of eDel
        /// would create isolated vertices, those are deleted as well.
        /// </summary>
        public void Delete(IPool pool, MeshUtils.Edge eDel)
        {
            var eDelSym = eDel._Sym;

            // First step: disconnect the origin vertex eDel->Org.  We make all
            // changes to get a consistent mesh in this "intermediate" state.

            bool joiningLoops = false;
            if (eDel._Lface != eDel.Rface)
            {
                // We are joining two loops into one -- remove the left face
                joiningLoops = true;
                MeshUtils.KillFace(pool, eDel._Lface, eDel.Rface);
            }

            if (eDel._Onext == eDel)
            {
                MeshUtils.KillVertex(pool, eDel._Org, null);
            }
            else
            {
                // Make sure that eDel->Org and eDel->Rface point to valid half-edges
                eDel.Rface._anEdge = eDel.Oprev;
                eDel._Org._anEdge = eDel._Onext;

                MeshUtils.Splice(eDel, eDel.Oprev);

                if (!joiningLoops)
                {
                    // We are splitting one loop into two -- create a new loop for eDel.
                    MeshUtils.MakeFace(pool, eDel, eDel._Lface);
                }
            }

            // Claim: the mesh is now in a consistent state, except that eDel->Org
            // may have been deleted.  Now we disconnect eDel->Dst.

            if (eDelSym._Onext == eDelSym)
            {
                MeshUtils.KillVertex(pool, eDelSym._Org, null);
                MeshUtils.KillFace(pool, eDelSym._Lface, null);
            }
            else
            {
                // Make sure that eDel->Dst and eDel->Lface point to valid half-edges
                eDel._Lface._anEdge = eDelSym.Oprev;
                eDelSym._Org._anEdge = eDelSym._Onext;
                MeshUtils.Splice(eDelSym, eDelSym.Oprev);
            }

            // Any isolated vertices or faces have already been freed.
            MeshUtils.KillEdge(pool, eDel);
        }

        /// <summary>
        /// Creates a new edge such that eNew == eOrg.Lnext and eNew.Dst is a newly created vertex.
        /// eOrg and eNew will have the same left face.
        /// </summary>
        public MeshUtils.Edge AddEdgeVertex(IPool pool, MeshUtils.Edge eOrg)
        {
            var eNew = MeshUtils.MakeEdge(pool, eOrg);
            var eNewSym = eNew._Sym;

            // Connect the new edge appropriately
            MeshUtils.Splice(eNew, eOrg._Lnext);

            // Set vertex and face information
            eNew._Org = eOrg.Dst;
            MeshUtils.MakeVertex(pool, eNewSym, eNew._Org);
            eNew._Lface = eNewSym._Lface = eOrg._Lface;

            return eNew;
        }

        /// <summary>
        /// Splits eOrg into two edges eOrg and eNew such that eNew == eOrg.Lnext.
        /// The new vertex is eOrg.Dst == eNew.Org.
        /// eOrg and eNew will have the same left face.
        /// </summary>
        public MeshUtils.Edge SplitEdge(IPool pool, MeshUtils.Edge eOrg)
        {
            var eTmp = AddEdgeVertex(pool, eOrg);
            var eNew = eTmp._Sym;

            // Disconnect eOrg from eOrg->Dst and connect it to eNew->Org
            MeshUtils.Splice(eOrg._Sym, eOrg._Sym.Oprev);
            MeshUtils.Splice(eOrg._Sym, eNew);

            // Set the vertex and face information
            eOrg.Dst = eNew._Org;
            eNew.Dst._anEdge = eNew._Sym; // may have pointed to eOrg->Sym
            eNew.Rface = eOrg.Rface;
            eNew._winding = eOrg._winding; // copy old winding information
            eNew._Sym._winding = eOrg._Sym._winding;

            return eNew;
        }

        /// <summary>
        /// Creates a new edge from eOrg->Dst to eDst->Org, and returns the corresponding half-edge eNew.
        /// If eOrg->Lface == eDst->Lface, this splits one loop into two,
        /// and the newly created loop is eNew->Lface.  Otherwise, two disjoint
        /// loops are merged into one, and the loop eDst->Lface is destroyed.
        /// 
        /// If (eOrg == eDst), the new face will have only two edges.
        /// If (eOrg->Lnext == eDst), the old face is reduced to a single edge.
        /// If (eOrg->Lnext->Lnext == eDst), the old face is reduced to two edges.
        /// </summary>
        public MeshUtils.Edge Connect(IPool pool, MeshUtils.Edge eOrg, MeshUtils.Edge eDst)
        {
            var eNew = MeshUtils.MakeEdge(pool, eOrg);
            var eNewSym = eNew._Sym;

            bool joiningLoops = false;
            if (eDst._Lface != eOrg._Lface)
            {
                // We are connecting two disjoint loops -- destroy eDst->Lface
                joiningLoops = true;
                MeshUtils.KillFace(pool, eDst._Lface, eOrg._Lface);
            }

            // Connect the new edge appropriately
            MeshUtils.Splice(eNew, eOrg._Lnext);
            MeshUtils.Splice(eNewSym, eDst);

            // Set the vertex and face information
            eNew._Org = eOrg.Dst;
            eNewSym._Org = eDst._Org;
            eNew._Lface = eNewSym._Lface = eOrg._Lface;

            // Make sure the old face points to a valid half-edge
            eOrg._Lface._anEdge = eNewSym;

            if (!joiningLoops)
            {
                MeshUtils.MakeFace(pool, eNew, eOrg._Lface);
            }

            return eNew;
        }

    }

    public interface ITypePool
    {
        object Get();
        void Return(object obj);
    }

    public class DefaultTypePool<T> : ITypePool where T : class, IPooled<T>, new()
    {
        private readonly Queue<T> _pool = new Queue<T>();

        public object Get()
        {
            lock (_pool)
            {
                if (_pool.Count > 0)
                {
                    return _pool.Dequeue();
                }
            }
            return new T();
        }

        public void Return(object obj)
        {
            lock (_pool)
            {
                _pool.Enqueue(obj as T);
            }
        }
    }

    public abstract class IPool
    {
        public IPool()
        {
            Register<MeshUtils.Vertex>(new DefaultTypePool<MeshUtils.Vertex>());
            Register<MeshUtils.Face>(new DefaultTypePool<MeshUtils.Face>());
            Register<MeshUtils.Edge>(new DefaultTypePool<MeshUtils.Edge>());
            Register<Tess.ActiveRegion>(new DefaultTypePool<Tess.ActiveRegion>());
        }
        public abstract void Register<T>(ITypePool typePool) where T : class, IPooled<T>, new();
        public abstract T Get<T>() where T : class, IPooled<T>, new();
        public abstract void Return<T>(T obj) where T : class, IPooled<T>, new();
    }

    public class NullPool : IPool
    {
        public override T Get<T>()
        {
            var obj = new T();
            obj.Init(this);
            return obj;
        }

        public override void Register<T>(ITypePool typePool)
        {
        }

        public override void Return<T>(T obj)
        {
        }
    }

    public class DefaultPool : IPool
    {
        private IDictionary<Type, ITypePool> _register;

        public override void Register<T>(ITypePool typePool)
        {
            if (_register == null)
            {
                // can support multiple readers as long as it's not modified
                _register = new Dictionary<Type, ITypePool>();
            }
            _register[typeof(T)] = typePool;
        }

        public override T Get<T>()
        {
            T obj = null;
            if (_register.TryGetValue(typeof(T), out ITypePool typePool))
            {
                obj = typePool.Get() as T;
            }
            if (obj == null)
            {
                obj = new T();
            }
            obj.Init(this);
            return obj;
        }

        public override void Return<T>(T obj)
        {
            if (obj == null)
            {
                return;
            }
            obj.Reset(this);
            if (_register.TryGetValue(typeof(T), out ITypePool typePool))
            {
                typePool.Return(obj);
            }
        }
    }

    public interface IPooled<T> where T : class, IPooled<T>, new()
    {
        void Init(IPool pool);
        void Reset(IPool pool);
    }

    internal static class MeshUtils
    {
        internal class Vertex : IPooled<Vertex>
        {
            internal Vertex _prev, _next;
            internal Edge _anEdge;

            internal Vec3 _coords;
            internal double _s, _t;
            internal PQHandle _pqHandle;
            internal int _n;
            internal object _data;

            public void Init(IPool pool)
            {
            }

            public void Reset(IPool pool)
            {
                _prev = _next = null;
                _anEdge = null;
                _coords = Vec3.Zero;
                _s = 0;
                _t = 0;
                _pqHandle = new PQHandle();
                _n = 0;
                _data = null;
            }
        }

        internal class Face : IPooled<Face>
        {
            internal Face _prev, _next;
            internal Edge _anEdge;

            internal Face _trail;
            internal int _n;
            internal bool _marked, _inside;

            internal int VertsCount
            {
                get
                {
                    int n = 0;
                    var eCur = _anEdge;
                    do
                    {
                        n++;
                        eCur = eCur._Lnext;
                    } while (eCur != _anEdge);
                    return n;
                }
            }

            public void Init(IPool pool)
            {
            }

            public void Reset(IPool pool)
            {
                _prev = _next = null;
                _anEdge = null;
                _trail = null;
                _n = 0;
                _marked = false;
                _inside = false;
            }
        }

        internal struct EdgePair
        {
            internal Edge _e, _eSym;

            public static EdgePair Create(IPool pool)
            {
                var e = pool.Get<MeshUtils.Edge>();
                var eSym = pool.Get<MeshUtils.Edge>();

                e._pair._e = e;
                e._pair._eSym = eSym;
                eSym._pair = e._pair;
                return e._pair;
            }

            public void Reset(IPool pool)
            {
                if (pool is null)
                {
                    _e = _eSym = null;
                }

                _e = _eSym = null;
            }
        }

        internal class Edge : IPooled<Edge>
        {
            internal EdgePair _pair;
            internal Edge _next, _Sym, _Onext, _Lnext;
            internal Vertex _Org;
            internal Face _Lface;
            internal Tess.ActiveRegion _activeRegion;
            internal int _winding;

            internal Face Rface { get { return _Sym._Lface; } set { _Sym._Lface = value; } }
            internal Vertex Dst { get { return _Sym._Org; } set { _Sym._Org = value; } }

            internal Edge Oprev { get { return _Sym._Lnext; } set { _Sym._Lnext = value; } }
            internal Edge Lprev { get { return _Onext._Sym; } set { _Onext._Sym = value; } }
            internal Edge Dprev { get { return _Lnext._Sym; } set { _Lnext._Sym = value; } }
            internal Edge Rprev { get { return _Sym._Onext; } set { _Sym._Onext = value; } }
            internal Edge Dnext { get { return Rprev._Sym; } set { Rprev._Sym = value; } }
            internal Edge Rnext { get { return Oprev._Sym; } set { Oprev._Sym = value; } }

            internal static void EnsureFirst(ref Edge e)
            {
                if (e == e._pair._eSym)
                {
                    e = e._Sym;
                }
            }

            public void Init(IPool pool)
            {
            }

            public void Reset(IPool pool)
            {
                _pair.Reset(pool);
                _next = _Sym = _Onext = _Lnext = null;
                _Org = null;
                _Lface = null;
                _activeRegion = null;
                _winding = 0;
            }
        }

        /// <summary>
        /// Splice( a, b ) is best described by the Guibas/Stolfi paper or the
        /// CS348a notes (see Mesh.cs). Basically it modifies the mesh so that
        /// a->Onext and b->Onext are exchanged. This can have various effects
        /// depending on whether a and b belong to different face or vertex rings.
        /// For more explanation see Mesh.Splice().
        /// </summary>
        public static void Splice(Edge a, Edge b)
        {
            var aOnext = a._Onext;
            var bOnext = b._Onext;

            aOnext._Sym._Lnext = b;
            bOnext._Sym._Lnext = a;
            a._Onext = bOnext;
            b._Onext = aOnext;
        }

        /// <summary>
        /// MakeVertex( eOrig, vNext ) attaches a new vertex and makes it the
        /// origin of all edges in the vertex loop to which eOrig belongs. "vNext" gives
        /// a place to insert the new vertex in the global vertex list. We insert
        /// the new vertex *before* vNext so that algorithms which walk the vertex
        /// list will not see the newly created vertices.
        /// </summary>
        public static void MakeVertex(IPool pool, Edge eOrig, Vertex vNext)
        {
            var vNew = pool.Get<MeshUtils.Vertex>();

            // insert in circular doubly-linked list before vNext
            var vPrev = vNext._prev;
            vNew._prev = vPrev;
            vPrev._next = vNew;
            vNew._next = vNext;
            vNext._prev = vNew;

            vNew._anEdge = eOrig;
            // leave coords, s, t undefined

            // fix other edges on this vertex loop
            var e = eOrig;
            do
            {
                e._Org = vNew;
                e = e._Onext;
            } while (e != eOrig);
        }

        /// <summary>
        /// MakeFace( eOrig, fNext ) attaches a new face and makes it the left
        /// face of all edges in the face loop to which eOrig belongs. "fNext" gives
        /// a place to insert the new face in the global face list. We insert
        /// the new face *before* fNext so that algorithms which walk the face
        /// list will not see the newly created faces.
        /// </summary>
        public static void MakeFace(IPool pool, Edge eOrig, Face fNext)
        {
            var fNew = pool.Get<MeshUtils.Face>();

            // insert in circular doubly-linked list before fNext
            var fPrev = fNext._prev;
            fNew._prev = fPrev;
            fPrev._next = fNew;
            fNew._next = fNext;
            fNext._prev = fNew;

            fNew._anEdge = eOrig;
            fNew._trail = null;
            fNew._marked = false;

            // The new face is marked "inside" if the old one was. This is a
            // convenience for the common case where a face has been split in two.
            fNew._inside = fNext._inside;

            // fix other edges on this face loop
            var e = eOrig;
            do
            {
                e._Lface = fNew;
                e = e._Lnext;
            } while (e != eOrig);
        }

        /// <summary>
        /// MakeEdge creates a new pair of half-edges which form their own loop.
        /// No vertex or face structures are allocated, but these must be assigned
        /// before the current edge operation is completed.
        /// </summary>
        public static MeshUtils.Edge MakeEdge(IPool pool, MeshUtils.Edge eNext)
        {
            Debug.Assert(eNext != null);

            var pair = MeshUtils.EdgePair.Create(pool);
            var e = pair._e;
            var eSym = pair._eSym;

            // Make sure eNext points to the first edge of the edge pair
            MeshUtils.Edge.EnsureFirst(ref eNext);

            // Insert in circular doubly-linked list before eNext.
            // Note that the prev pointer is stored in Sym->next.
            var ePrev = eNext._Sym._next;
            eSym._next = ePrev;
            ePrev._Sym._next = e;
            e._next = eNext;
            eNext._Sym._next = eSym;

            e._Sym = eSym;
            e._Onext = e;
            e._Lnext = eSym;
            e._Org = null;
            e._Lface = null;
            e._winding = 0;
            e._activeRegion = null;

            eSym._Sym = e;
            eSym._Onext = eSym;
            eSym._Lnext = e;
            eSym._Org = null;
            eSym._Lface = null;
            eSym._winding = 0;
            eSym._activeRegion = null;

            return e;
        }

        /// <summary>
        /// KillEdge( eDel ) destroys an edge (the half-edges eDel and eDel->Sym),
        /// and removes from the global edge list.
        /// </summary>
        public static void KillEdge(IPool pool, Edge eDel)
        {
            // Half-edges are allocated in pairs, see EdgePair above
            Edge.EnsureFirst(ref eDel);

            // delete from circular doubly-linked list
            var eNext = eDel._next;
            var ePrev = eDel._Sym._next;
            eNext._Sym._next = ePrev;
            ePrev._Sym._next = eNext;

            pool.Return(eDel._Sym);
            pool.Return(eDel);
        }

        /// <summary>
        /// KillVertex( vDel ) destroys a vertex and removes it from the global
        /// vertex list. It updates the vertex loop to point to a given new vertex.
        /// </summary>
        public static void KillVertex(IPool pool, Vertex vDel, Vertex newOrg)
        {
            var eStart = vDel._anEdge;

            // change the origin of all affected edges
            var e = eStart;
            do
            {
                e._Org = newOrg;
                e = e._Onext;
            } while (e != eStart);

            // delete from circular doubly-linked list
            var vPrev = vDel._prev;
            var vNext = vDel._next;
            vNext._prev = vPrev;
            vPrev._next = vNext;

            pool.Return(vDel);
        }

        /// <summary>
        /// KillFace( fDel ) destroys a face and removes it from the global face
        /// list. It updates the face loop to point to a given new face.
        /// </summary>
        public static void KillFace(IPool pool, Face fDel, Face newLFace)
        {
            var eStart = fDel._anEdge;

            // change the left face of all affected edges
            var e = eStart;
            do
            {
                e._Lface = newLFace;
                e = e._Lnext;
            } while (e != eStart);

            // delete from circular doubly-linked list
            var fPrev = fDel._prev;
            var fNext = fDel._next;
            fNext._prev = fPrev;
            fPrev._next = fNext;

            pool.Return(fDel);
        }

        /// <summary>
        /// Return signed area of face.
        /// </summary>
        public static double FaceArea(Face f)
        {
            double area = 0;
            var e = f._anEdge;
            do
            {
                area += (e._Org._s - e.Dst._s) * (e._Org._t + e.Dst._t);
                e = e._Lnext;
            } while (e != f._anEdge);
            return area;
        }
    }

    internal struct PQHandle
    {
        public static readonly int Invalid = 0x0fffffff;
        internal int _handle;
    }

    internal class PriorityHeap<TValue> where TValue : class
    {
        public delegate bool LessOrEqual(TValue lhs, TValue rhs);

        protected class HandleElem
        {
            internal TValue _key;
            internal int _node;
        }

        private readonly LessOrEqual _leq;
        private int[] _nodes;
        private HandleElem[] _handles;
        private int _size, _max;
        private int _freeList;
        private bool _initialized;

        public bool Empty { get { return _size == 0; } }

        public PriorityHeap(int initialSize, LessOrEqual leq)
        {
            _leq = leq;

            _nodes = new int[initialSize + 1];
            _handles = new HandleElem[initialSize + 1];

            _size = 0;
            _max = initialSize;
            _freeList = 0;
            _initialized = false;

            _nodes[1] = 1;
            _handles[1] = new HandleElem { _key = null };
        }

        private void FloatDown(int curr)
        {
            int child;
            int hCurr, hChild;

            hCurr = _nodes[curr];
            while (true)
            {
                child = curr << 1;
                if (child < _size && _leq(_handles[_nodes[child + 1]]._key, _handles[_nodes[child]]._key))
                {
                    ++child;
                }

                Debug.Assert(child <= _max);

                hChild = _nodes[child];
                if (child > _size || _leq(_handles[hCurr]._key, _handles[hChild]._key))
                {
                    _nodes[curr] = hCurr;
                    _handles[hCurr]._node = curr;
                    break;
                }

                _nodes[curr] = hChild;
                _handles[hChild]._node = curr;
                curr = child;
            }
        }

        private void FloatUp(int curr)
        {
            int parent;
            int hCurr, hParent;

            hCurr = _nodes[curr];
            while (true)
            {
                parent = curr >> 1;
                hParent = _nodes[parent];
                if (parent == 0 || _leq(_handles[hParent]._key, _handles[hCurr]._key))
                {
                    _nodes[curr] = hCurr;
                    _handles[hCurr]._node = curr;
                    break;
                }
                _nodes[curr] = hParent;
                _handles[hParent]._node = curr;
                curr = parent;
            }
        }


        public void Init()
        {
            for (int i = _size; i >= 1; --i)
            {
                FloatDown(i);
            }
            _initialized = true;
        }

        public PQHandle Insert(TValue value)
        {
            int curr = ++_size;
            if ((curr * 2) > _max)
            {
                _max <<= 1;
                Array.Resize(ref _nodes, _max + 1);
                Array.Resize(ref _handles, _max + 1);
            }

            int free;
            if (_freeList == 0)
            {
                free = curr;
            }
            else
            {
                free = _freeList;
                _freeList = _handles[free]._node;
            }

            _nodes[curr] = free;
            if (_handles[free] == null)
            {
                _handles[free] = new HandleElem { _key = value, _node = curr };
            }
            else
            {
                _handles[free]._node = curr;
                _handles[free]._key = value;
            }

            if (_initialized)
            {
                FloatUp(curr);
            }

            Debug.Assert(free != PQHandle.Invalid);
            return new PQHandle { _handle = free };
        }

        public TValue ExtractMin()
        {
            Debug.Assert(_initialized);

            int hMin = _nodes[1];
            TValue min = _handles[hMin]._key;

            if (_size > 0)
            {
                _nodes[1] = _nodes[_size];
                _handles[_nodes[1]]._node = 1;

                _handles[hMin]._key = null;
                _handles[hMin]._node = _freeList;
                _freeList = hMin;

                if (--_size > 0)
                {
                    FloatDown(1);
                }
            }

            return min;
        }

        public TValue Minimum()
        {
            Debug.Assert(_initialized);
            return _handles[_nodes[1]]._key;
        }

        public void Remove(PQHandle handle)
        {
            Debug.Assert(_initialized);

            int hCurr = handle._handle;
            Debug.Assert(hCurr >= 1 && hCurr <= _max && _handles[hCurr]._key != null);

            int curr = _handles[hCurr]._node;
            _nodes[curr] = _nodes[_size];
            _handles[_nodes[curr]]._node = curr;

            if (curr <= --_size)
            {
                if (curr <= 1 || _leq(_handles[_nodes[curr >> 1]]._key, _handles[_nodes[curr]]._key))
                {
                    FloatDown(curr);
                }
                else
                {
                    FloatUp(curr);
                }
            }

            _handles[hCurr]._key = null;
            _handles[hCurr]._node = _freeList;
            _freeList = hCurr;
        }
    }

    internal class PriorityQueue<TValue> where TValue : class
    {
        private readonly PriorityHeap<TValue>.LessOrEqual _leq;
        private readonly PriorityHeap<TValue> _heap;
        private TValue[] _keys;
        private int[] _order;

        private int _size, _max;
        private bool _initialized;

        public PriorityQueue(int initialSize, PriorityHeap<TValue>.LessOrEqual leq)
        {
            _leq = leq;
            _heap = new PriorityHeap<TValue>(initialSize, leq);

            _keys = new TValue[initialSize];

            _size = 0;
            _max = initialSize;
            _initialized = false;
        }

        class StackItem
        {
            internal int p, r;
        };

        static void Swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }

        public void Init()
        {
            var stack = new Stack<StackItem>();
            int p, r, i, j, piv;
            uint seed = 2016473283;

            p = 0;
            r = _size - 1;
            _order = new int[_size + 1];
            for (piv = 0, i = p; i <= r; ++piv, ++i)
            {
                _order[i] = piv;
            }

            stack.Push(new StackItem { p = p, r = r });
            while (stack.Count > 0)
            {
                var top = stack.Pop();
                p = top.p;
                r = top.r;

                while (r > p + 10)
                {
                    seed = seed * 1539415821 + 1;
                    i = p + (int)(seed % (r - p + 1));
                    piv = _order[i];
                    _order[i] = _order[p];
                    _order[p] = piv;
                    i = p - 1;
                    j = r + 1;
                    do
                    {
                        do { ++i; } while (!_leq(_keys[_order[i]], _keys[piv]));
                        do { --j; } while (!_leq(_keys[piv], _keys[_order[j]]));
                        Swap(ref _order[i], ref _order[j]);
                    } while (i < j);
                    Swap(ref _order[i], ref _order[j]);
                    if (i - p < r - j)
                    {
                        stack.Push(new StackItem { p = j + 1, r = r });
                        r = i - 1;
                    }
                    else
                    {
                        stack.Push(new StackItem { p = p, r = i - 1 });
                        p = j + 1;
                    }
                }
                for (i = p + 1; i <= r; ++i)
                {
                    piv = _order[i];
                    for (j = i; j > p && !_leq(_keys[piv], _keys[_order[j - 1]]); --j)
                    {
                        _order[j] = _order[j - 1];
                    }
                    _order[j] = piv;
                }
            }

            _max = _size;
            _initialized = true;
            _heap.Init();
        }

        public PQHandle Insert(TValue value)
        {
            if (_initialized)
            {
                return _heap.Insert(value);
            }

            int curr = _size;
            if (++_size >= _max)
            {
                _max <<= 1;
                Array.Resize(ref _keys, _max);
            }

            _keys[curr] = value;
            return new PQHandle { _handle = -(curr + 1) };
        }

        public TValue ExtractMin()
        {
            Debug.Assert(_initialized);

            if (_size == 0)
            {
                return _heap.ExtractMin();
            }
            TValue sortMin = _keys[_order[_size - 1]];
            if (!_heap.Empty)
            {
                TValue heapMin = _heap.Minimum();
                if (_leq(heapMin, sortMin))
                    return _heap.ExtractMin();
            }
            do
            {
                --_size;
            } while (_size > 0 && _keys[_order[_size - 1]] == null);

            return sortMin;
        }

        public TValue Minimum()
        {
            Debug.Assert(_initialized);

            if (_size == 0)
            {
                return _heap.Minimum();
            }
            TValue sortMin = _keys[_order[_size - 1]];
            if (!_heap.Empty)
            {
                TValue heapMin = _heap.Minimum();
                if (_leq(heapMin, sortMin))
                    return heapMin;
            }
            return sortMin;
        }

        public void Remove(PQHandle handle)
        {
            Debug.Assert(_initialized);

            int curr = handle._handle;
            if (curr >= 0)
            {
                _heap.Remove(handle);
                return;
            }
            curr = -(curr + 1);
            Debug.Assert(curr < _max && _keys[curr] != null);

            _keys[curr] = null;
            while (_size > 0 && _keys[_order[_size - 1]] == null)
            {
                --_size;
            }
        }
    }
}