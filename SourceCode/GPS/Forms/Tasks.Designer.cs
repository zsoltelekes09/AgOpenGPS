using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AgOpenGPS
{
    public enum TaskName { OpenJob, CloseJob, Save, Delete, FixBnd, FixHead, HeadLand, Boundary, TurnLine, GeoFence, Triangulate, MinMax };

    public enum HeadLandTaskName { Left, Right, Up, Down, Offset, FixHeading, Save };

    public class TaskClass
    {
        public Task Task;
        public CBoundaryLines Idx;
        public TaskName TaskName;
        public CancellationTokenSource Token;

        public TaskClass(Task task, CBoundaryLines idx, TaskName taskname, CancellationTokenSource token)
        {
            Task = task;
            Idx = idx;
            TaskName = taskname;
            Token = token;
        }
    }

    public partial class FormGPS
    {
        public List<TaskClass> TaskList = new List<TaskClass>();

        public void StartTasks(CBoundaryLines BoundaryLine, int k, TaskName TaskName)
        {
            List<Task> tasks = new List<Task>();
            CancellationTokenSource newtoken = new CancellationTokenSource();
            if (TaskName == TaskName.HeadLand || TaskName == TaskName.TurnLine || TaskName == TaskName.GeoFence)
            {
                for (int j = 0; j < TaskList.Count; j++)
                {
                    if (TaskList[j].Task.IsCompleted)
                    {
                        TaskList.RemoveAt(j);
                        j--;
                    }
                    else if (TaskList[j].TaskName == TaskName.OpenJob || TaskList[j].TaskName == TaskName.CloseJob || TaskList[j].TaskName == TaskName.Save)
                    {
                        tasks.Add(TaskList[j].Task);
                    }
                    else if (TaskList[j].Idx == BoundaryLine)
                    {
                        if (TaskList[j].TaskName == TaskName.Delete)
                        {
                            return;
                        }
                        else if (TaskName == TaskName.GeoFence || TaskName == TaskName.TurnLine)
                        {
                            if (TaskList[j].TaskName == TaskName.FixBnd)
                            {
                                tasks.Add(TaskList[j].Task);
                            }
                            else if (TaskList[j].TaskName == TaskName)
                            {
                                TaskList[j].Token.Cancel();
                                tasks.Add(TaskList[j].Task);
                            }
                        }
                        else
                        {
                            if (TaskList[j].TaskName == TaskName.FixHead)
                            {
                                tasks.Add(TaskList[j].Task);
                            }
                            else if (TaskList[j].TaskName == TaskName)
                            {
                                TaskList[j].Token.Cancel();
                                tasks.Add(TaskList[j].Task);
                            }
                        }
                    }
                }
                if (TaskName == TaskName.HeadLand)
                {
                    Task NewTask = Task_TriangulateHeadland(BoundaryLine, tasks, newtoken.Token);
                    TaskList.Add(new TaskClass(NewTask, BoundaryLine, TaskName.HeadLand, newtoken));
                }
                else if (TaskName == TaskName.TurnLine)
                {
                    Task NewTask = Task_BuildTurnLine(BoundaryLine, k == 0 ? Properties.Vehicle.Default.UturnTriggerDistance : -Properties.Vehicle.Default.UturnTriggerDistance, tasks, newtoken.Token);
                    TaskList.Add(new TaskClass(NewTask, BoundaryLine, TaskName.TurnLine, newtoken));
                }
                else if (TaskName == TaskName.GeoFence)
                {
                    Task NewTask = Task_BuildGeoFenceLine(BoundaryLine, k == 0 ? Properties.Vehicle.Default.GeoFenceOffset : -Properties.Vehicle.Default.GeoFenceOffset, tasks, newtoken.Token);
                    TaskList.Add(new TaskClass(NewTask, BoundaryLine, TaskName.GeoFence, newtoken));
                }
            }
            else if (TaskName == TaskName.Save || TaskName == TaskName.CloseJob || TaskName == TaskName.Delete)
            {

                for (int j = 0; j < TaskList.Count; j++)
                {
                    if (TaskList[j].Task.IsCompleted)
                    {
                        TaskList.RemoveAt(j);
                        j--;
                    }
                    else if (TaskList[j].TaskName == TaskName.OpenJob || TaskList[j].TaskName == TaskName.CloseJob || TaskList[j].TaskName == TaskName.Save || TaskList[j].TaskName == TaskName.FixBnd || TaskList[j].TaskName == TaskName.FixHead || TaskList[j].TaskName == TaskName.Delete)
                    {
                        tasks.Add(TaskList[j].Task);
                    }
                    else if (TaskName != TaskName.Save)
                    {
                        tasks.Add(TaskList[j].Task);
                        if (TaskName == TaskName.CloseJob || (TaskName == TaskName.Delete && TaskList[j].Idx == BoundaryLine))
                            TaskList[j].Token.Cancel();
                    }

                }

                if (TaskName == TaskName.Save)
                {
                    Task NewTask = Task_Save(k, tasks, newtoken.Token);

                    TaskList.Add(new TaskClass(NewTask, BoundaryLine, TaskName.Save, newtoken));
                }
                else if (TaskName == TaskName.CloseJob)
                {
                    Task NewTask = Task_JobClose(BoundaryLine, tasks, newtoken.Token);
                    TaskList.Add(new TaskClass(NewTask, BoundaryLine, TaskName.CloseJob, newtoken));
                }
                else if (TaskName == TaskName.Delete)
                {
                    Task NewTask = Task_Delete(BoundaryLine, tasks, newtoken.Token);
                    TaskList.Add(new TaskClass(NewTask, BoundaryLine, TaskName.Delete, newtoken));
                }
            }
            else
            {
                for (int j = 0; j < TaskList.Count; j++)
                {
                    if (TaskList[j].Task.IsCompleted)
                    {
                        TaskList.RemoveAt(j);
                        j--;
                    }
                    else
                    {

                        if (TaskList[j].TaskName == TaskName.OpenJob || TaskList[j].TaskName == TaskName.CloseJob || TaskList[j].TaskName == TaskName.Save)
                        {
                            tasks.Add(TaskList[j].Task);
                        }
                        else if (TaskList[j].Idx == BoundaryLine)
                        {
                            if (TaskList[j].TaskName == TaskName.Delete)
                            {
                                return;
                            }
                            if (TaskList[j].TaskName == TaskName.FixBnd)
                            {
                                tasks.Add(TaskList[j].Task);
                            }
                            else if (TaskList[j].TaskName != TaskName.FixHead && TaskList[j].TaskName != TaskName.HeadLand)
                            {
                                tasks.Add(TaskList[j].Task);
                                TaskList[j].Token.Cancel();
                            }
                        }
                    }
                }

                Task NewTask = Task_FixBoundaryLine(BoundaryLine, tasks, newtoken.Token);
                TaskList.Add(new TaskClass(NewTask, BoundaryLine, TaskName.FixBnd, newtoken));
                tasks.Add(NewTask);

                newtoken = new CancellationTokenSource();
                NewTask = Task_BoundaryArea(BoundaryLine, tasks, newtoken.Token);
                TaskList.Add(new TaskClass(NewTask, BoundaryLine, TaskName.MinMax, newtoken));

                newtoken = new CancellationTokenSource();
                NewTask = Task_TriangulateBoundary(BoundaryLine, tasks, newtoken.Token);
                TaskList.Add(new TaskClass(NewTask, BoundaryLine, TaskName.Triangulate, newtoken));

                newtoken = new CancellationTokenSource();
                NewTask = Task_BuildGeoFenceLine(BoundaryLine, k == 0 ? Properties.Vehicle.Default.GeoFenceOffset : -Properties.Vehicle.Default.GeoFenceOffset, tasks, newtoken.Token);
                TaskList.Add(new TaskClass(NewTask, BoundaryLine, TaskName.GeoFence, newtoken));

                newtoken = new CancellationTokenSource();
                NewTask = Task_BuildTurnLine(BoundaryLine, k == 0 ? Properties.Vehicle.Default.UturnTriggerDistance : -Properties.Vehicle.Default.UturnTriggerDistance, tasks, newtoken.Token);
                TaskList.Add(new TaskClass(NewTask, BoundaryLine, TaskName.TurnLine, newtoken));
            }
        }

        public async Task Task_FixBoundaryLine(CBoundaryLines BoundaryLine, List<Task> tasks, CancellationToken ct)
        {
            if (tasks.Count > 0) await Task.WhenAll(tasks);
            await Task.Run(() =>
            {
                BoundaryLine.FixBoundaryLine(ct);
                if (BoundaryLine.Area >= 100)
                {
                    BoundaryLine.bndLine.CalculateRoundedCorner(0.5, true, 0.0436332, ct);
                }
            });
        }

        public async Task Task_BoundaryArea(CBoundaryLines BoundaryLine, List<Task> tasks, CancellationToken ct)
        {
            if (tasks.Count > 0) await Task.WhenAll(tasks);
            await Task.Run(() =>
            {
                BoundaryLine.BoundaryMinMax(ct);

                fd.UpdateFieldBoundaryGUIAreas();
            });
        }

        public async Task Task_TriangulateBoundary(CBoundaryLines BoundaryLine, List<Task> tasks, CancellationToken ct)
        {
            if (tasks.Count > 0) await Task.WhenAll(tasks);

            BoundaryLine.Indexer = await Task.Run(() =>
            {
                if (BoundaryLine.bndLine.Count > 2 && BoundaryLine.Area > 99)
                {
                    return BoundaryLine.bndLine.TriangulatePolygon(ct);
                }
                else return new List<int>();
            });
        }

        public async Task Task_BuildGeoFenceLine(CBoundaryLines Boundary, double distance, List<Task> tasks, CancellationToken ct)
        {
            if (tasks.Count > 0) await Task.WhenAll(tasks);

            var tuple = await Task.Run(() =>
            {
                Boundary.BuildGeoFenceLine(Boundary.bndLine, distance, ct, out List<Vec3> geoFenceLine2, out List<Vec2> GeocalcList2);

                return new Tuple<List<Vec3>, List<Vec2>>(geoFenceLine2, GeocalcList2);
            });
            Boundary.geoFenceLine = tuple.Item1;
            Boundary.GeocalcList = tuple.Item2;
        }

        public async Task Task_BuildTurnLine(CBoundaryLines Boundary, double distance, List<Task> tasks, CancellationToken ct)
        {
            if (tasks.Count > 0) await Task.WhenAll(tasks);

            var tuple = await Task.Run(() =>
            {
                Boundary.BuildTurnLine(Boundary.bndLine, distance, ct, out List<Vec3> turnLine2, out List<Vec2> calcList2);
                
                return new Tuple<List<Vec3>, List<Vec2>>(turnLine2, calcList2);
            });
            Boundary.turnLine = tuple.Item1;
            Boundary.calcList = tuple.Item2;
        }

        public async Task Task_TriangulateHeadland(CBoundaryLines BoundaryLine, List<Task> tasks, CancellationToken ct)
        {
            if (tasks.Count > 0) await Task.WhenAll(tasks);

            BoundaryLine.HeadLineIndexer = await Task.Run(() =>
            {
                List<List<int>> Head = new List<List<int>>();

                for (int i = 0; i < BoundaryLine.HeadLine.Count; i++)
                    Head.Add(BoundaryLine.HeadLine[i].TriangulatePolygon(ct));

                return Head;
            });
        }

        public async Task Task_Delete(CBoundaryLines BoundaryLine, List<Task> tasks, CancellationToken ct)
        {
            if (tasks.Count > 0) await Task.WhenAll(tasks);

            bnd.bndArr.Remove(BoundaryLine);

            await Task.Run(() =>
            {
                fd.UpdateFieldBoundaryGUIAreas();
            });
        }

        public async Task Task_JobClose(CBoundaryLines BoundaryLine, List<Task> tasks, CancellationToken ct)
        {
            if (tasks.Count > 0) await Task.WhenAll(tasks);

            JobClose();

            fd.areaOuterBoundary = 0;
            fd.areaBoundaryOuterLessInner = 0;

            maxFieldX = 0; minFieldX = 0; maxFieldY = 0; minFieldY = 0; maxFieldDistance = 1500;
            maxCrossFieldLength = 1000;
            maxFieldDistance = 100;
        }

        public async Task Task_JobNew(string fileAndDirectory, List<Task> tasks)
        {
            if (tasks.Count > 0) await Task.WhenAll(tasks);

            JobNew();

            await Task.Run(() =>
            {
                FileOpenField2(fileAndDirectory);
            });
        }

        public async Task Task_Save(int t, List<Task> tasks, CancellationToken ct)
        {
            if (tasks.Count > 0) await Task.WhenAll(tasks);
            await Task.Run(() =>
            {
                if (t == 1 || t == 6) FileSaveBoundary();
                if (t == 2 || t == 6) FileSaveHeadland();
                if (t == 0 || t == 3 || t == 7) FileSaveSections();
                if (t == 0 || t == 5) FileSaveFieldKML();
            });
        }

        public async Task Task_FixHeadLand(CBoundaryLines BoundaryLine, HeadLandTaskName HeadLandAction, double Offset, int Idx, int Boundary, int start, int end, List<Task> tasks, CancellationToken ct)
        {
            if (tasks.Count > 0) await Task.WhenAll(tasks);

            await Task.Run(() =>
            {
                if (BoundaryLine.Template.Count > 0)
                {
                    if (HeadLandAction == HeadLandTaskName.Save)
                    {
                        BoundaryLine.HeadLine.Clear();
                        if (Idx == 0)
                            BoundaryLine.HeadLine.AddRange(BoundaryLine.Template);

                        StartTasks(BoundaryLine, 0, TaskName.HeadLand);
                        StartTasks(null, 2, TaskName.Save);
                    }
                    else if (HeadLandAction == HeadLandTaskName.FixHeading)
                    {
                        if (BoundaryLine.Template.Count > Idx)
                        {
                            BoundaryLine.Template[Idx].CalculateRoundedCorner(0.25, true, 0.04, ct);
                        }
                    }
                    else
                    {
                        double offset = Offset * (Boundary == 0 ? 1 : -1);
                        Vec3 Point;

                        int Start2 = start;
                        int End2 = end;

                        int Index = Idx;

                        if (start == -1 || end == -1)
                        {
                            Index = -1;
                        }
                        else
                        {
                            if (BoundaryLine.Template.Count > Idx)
                            {
                                if (End2 > BoundaryLine.Template[Idx].Count) End2 = 0;
                                if (((BoundaryLine.Template[Idx].Count - End2 + Start2) % BoundaryLine.Template[Idx].Count) < ((BoundaryLine.Template[Idx].Count - Start2 + End2) % BoundaryLine.Template[Idx].Count))
                                {
                                    int index = Start2; Start2 = End2; End2 = index;
                                }
                            }
                        }


                        int test = BoundaryLine.Template.Count();

                        for (int i = 0; i < test; i++)
                        {
                            bool Loop = Start2 > End2;

                            List<Vec3> Template2 = new List<Vec3>();

                            for (int j = 0; j < BoundaryLine.Template[i].Count; j++)
                            {
                                Point = BoundaryLine.Template[i][j];
                                if (Index == -1 || (Index == i && ((Loop && (j < End2 || j > Start2)) || (!Loop && j > Start2 && j < End2))))
                                {

                                    if (HeadLandAction == HeadLandTaskName.Offset)
                                    {
                                        double CosHeading = Math.Cos(BoundaryLine.Template[i][j].Heading);
                                        double SinHeading = Math.Sin(BoundaryLine.Template[i][j].Heading);
                                        Point.Northing += SinHeading * -offset;
                                        Point.Easting += CosHeading * offset;
                                    }
                                    else if (HeadLandAction == HeadLandTaskName.Up)
                                        Point.Northing++;
                                    else if (HeadLandAction == HeadLandTaskName.Down)
                                        Point.Northing--;
                                    else if (HeadLandAction == HeadLandTaskName.Right)
                                        Point.Easting++;
                                    else if (HeadLandAction == HeadLandTaskName.Left)
                                        Point.Easting--;
                                }
                                Template2.Add(Point);
                            }

                            if (HeadLandAction == HeadLandTaskName.Offset)
                            {
                                List<List<Vec3>> finalPoly = Template2.ClipPolyLine(null, true, offset, ct);

                                for (int j = 0; j < finalPoly.Count; j++)
                                {
                                    double Area = finalPoly[j].PolygonArea(ct);
                                    if (Area > -25)
                                    {
                                        finalPoly.RemoveAt(j);
                                        j--;
                                    }
                                    else
                                        finalPoly[j].CalculateHeading(true, ct);
                                    //finalPoly[j].CalculateRoundedCorner(0.25, true, 0.04, ct);
                                }
                                BoundaryLine.Template.AddRange(finalPoly);
                            }
                            else
                            {
                                BoundaryLine.Template.Add(Template2);
                            }
                        }
                        BoundaryLine.Template.RemoveRange(0, test);
                    }
                }
            });
        }
    }
}
