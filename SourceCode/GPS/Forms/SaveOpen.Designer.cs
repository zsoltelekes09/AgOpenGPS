using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        //list of the list of patch data individual triangles for field sections
        public List<List<Vec3>> PatchSaveList = new List<List<Vec3>>();
        public List<List<Vec3>> PatchDrawList = new List<List<Vec3>>();
        public List<List<Vec3>> ContourSaveList = new List<List<Vec3>>();

        public List<CAutoLoadField> Fields = new List<CAutoLoadField>();

        public void FileSaveCurveLines()
        {
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\CurveLines.txt";

            int cnt = CurveLines.Lines.Count;

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                try
                {
                    writer.WriteLine("$CurveLines");
                    if (cnt > 0)
                    {
                        for (int i = 0; i < cnt; i++)
                        {
                            //write out the Name
                            writer.WriteLine(CurveLines.Lines[i].Name);

                            //write out the aveheading
                            writer.WriteLine(CurveLines.Lines[i].SpiralMode.ToString(CultureInfo.InvariantCulture));
                            writer.WriteLine(CurveLines.Lines[i].CircleMode.ToString(CultureInfo.InvariantCulture));
                            writer.WriteLine(CurveLines.Lines[i].BoundaryMode.ToString(CultureInfo.InvariantCulture));
                            writer.WriteLine(CurveLines.Lines[i].Heading.ToString(CultureInfo.InvariantCulture));

                            //write out the points of ref line
                            int cnt2 = CurveLines.Lines[i].curvePts.Count;

                            writer.WriteLine(cnt2.ToString(CultureInfo.InvariantCulture));
                            if (CurveLines.Lines[i].curvePts.Count > 0)
                            {
                                for (int j = 0; j < cnt2; j++)
                                    writer.WriteLine(Math.Round(CurveLines.Lines[i].curvePts[j].Easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                        Math.Round(CurveLines.Lines[i].curvePts[j].Northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                            Math.Round(CurveLines.Lines[i].curvePts[j].Heading, 5).ToString(CultureInfo.InvariantCulture));
                            }
                        }
                    }
                }
                catch (Exception er)
                {
                    WriteErrorLog("Saving Curve Line" + er.ToString());

                    return;
                }
            }
        }

        public void FileLoadCurveLines()
        {
            CurveLines.Lines.Clear();

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\CurveLines.txt";

            if (!File.Exists(filename))
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine("$CurveLines");
                }
            }

            //get the file of previous AB Lines
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            if (!File.Exists(filename))
            {
                TimedMessageBox(2000, String.Get("gsFileError"), String.Get("gsMissingABCurveFile"));
            }
            else
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    try
                    {
                        string line;

                        //read header $CurveLine
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            CurveLines.Lines.Add(new CCurveLines());
                            int idx = CurveLines.Lines.Count - 1;

                            //read header $CurveLine
                            CurveLines.Lines[idx].Name = reader.ReadLine();
                            // get the average heading
                            line = reader.ReadLine();

                            if (line == "True" || line == "False")
                            {
                                CurveLines.Lines[idx].SpiralMode = bool.Parse(line);
                                line = reader.ReadLine();
                            }
                            else
                            {
                                CurveLines.Lines[idx].SpiralMode = false;
                            }
                            if (line == "True" || line == "False")
                            {
                                CurveLines.Lines[idx].CircleMode = bool.Parse(line);
                                line = reader.ReadLine();
                            }
                            else
                            {
                                CurveLines.Lines[idx].CircleMode = false;
                            }
                            if (line == "True" || line == "False")
                            {
                                CurveLines.Lines[idx].BoundaryMode = bool.Parse(line);
                                line = reader.ReadLine();
                            }
                            else
                            {
                                CurveLines.Lines[idx].BoundaryMode = false;
                            }
                            CurveLines.Lines[idx].Heading = double.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            int numPoints = int.Parse(line);

                            if (numPoints > 0)
                            {
                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    Vec3 vecPt = new Vec3(double.Parse(words[1], CultureInfo.InvariantCulture),
                                        double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[2], CultureInfo.InvariantCulture));
                                    CurveLines.Lines[idx].curvePts.Add(vecPt);
                                }
                                CurveLines.Lines[idx].curvePts.CalculateRoundedCorner(0.5, CurveLines.Lines[idx].BoundaryMode, 0.0436332, CancellationToken.None);
                            }
                            else
                            {
                                if (CurveLines.Lines.Count > 0)
                                {
                                    CurveLines.Lines.RemoveAt(idx);
                                    idx--;
                                }
                            }
                        }
                    }
                    catch (Exception er)
                    {
                        TimedMessageBox(2000, String.Get("gsCurveLineFileIsCorrupt"), String.Get("gsButFieldIsLoaded"));
                        WriteErrorLog("Load Curve Line" + er.ToString());
                    }
                }
            }
        }

        public void FileSaveABLines()
        {
            //make sure at least a global blank AB Line file exists
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

            //get the file of previous AB Lines
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\ABLines.txt";
            int cnt = ABLines.ABLines.Count;

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                if (cnt > 0)
                {
                    foreach (var item in ABLines.ABLines)
                    {
                        //make it culture invariant
                        string line = item.Name
                            + ',' + (Math.Round(Glm.ToDegrees(item.Heading), 8)).ToString(CultureInfo.InvariantCulture)
                            + ',' + (Math.Round(item.ref1.Easting, 3)).ToString(CultureInfo.InvariantCulture)
                            + ',' + (Math.Round(item.ref1.Northing, 3)).ToString(CultureInfo.InvariantCulture)
                            + ',' + (item.UsePoint).ToString(CultureInfo.InvariantCulture)
                            + ',' + (Math.Round(item.ref2.Easting, 3)).ToString(CultureInfo.InvariantCulture)
                            + ',' + (Math.Round(item.ref2.Northing, 3)).ToString(CultureInfo.InvariantCulture);

                        //write out to file
                        writer.WriteLine(line);
                    }
                }
            }
        }

        public void FileLoadABLines()
        {
            //make sure at least a global blank AB Line file exists
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\ABLines.txt";

            if (!File.Exists(filename))
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                }
            }

            if (!File.Exists(filename))
            {
                TimedMessageBox(2000, String.Get("gsFileError"), String.Get("gsMissingABLinesFile"));
            }
            else
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    try
                    {
                        string line;
                        ABLines.ABLines.Clear();

                        //read all the lines
                        for (int i = 0; !reader.EndOfStream; i++)
                        {

                            line = reader.ReadLine();
                            string[] words = line.Split(',');

                            if (words.Length != 7) break;

                            ABLines.ABLines.Add(new CABLines());

                            ABLines.ABLines[i].Name = words[0];
                            ABLines.ABLines[i].Heading = Glm.ToRadians(double.Parse(words[1], CultureInfo.InvariantCulture));
                            ABLines.ABLines[i].ref1.Easting = double.Parse(words[2], CultureInfo.InvariantCulture);
                            ABLines.ABLines[i].ref1.Northing = double.Parse(words[3], CultureInfo.InvariantCulture);
                            ABLines.ABLines[i].UsePoint = bool.Parse(words[4]);
                            ABLines.ABLines[i].ref2.Easting = double.Parse(words[5], CultureInfo.InvariantCulture);
                            ABLines.ABLines[i].ref2.Northing = double.Parse(words[6], CultureInfo.InvariantCulture);
                        }
                    }
                    catch (Exception er)
                    {
                        TimedMessageBox(2000, String.Get("gsABLineFileIsCorrupt"), "Please delete it!!!");
                        WriteErrorLog("FieldOpen, Loading ABLine, Corrupt ABLine File" + er);
                    }
                }
            }
        }

        //function that save vehicle and section settings
        public void FileSaveVehicle(string FileName)
        {
            string dir = Path.GetDirectoryName(vehiclesDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            vehicleFileName = Path.GetFileNameWithoutExtension(FileName) + " - ";
            Properties.Vehicle.Default.setVehicle_vehicleName = vehicleFileName;
            Properties.Vehicle.Default.Save();

            using (StreamWriter writer = new StreamWriter(FileName))
            {
                writer.WriteLine("Version," + Application.ProductVersion.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("AntennaHeight," + Properties.Vehicle.Default.setVehicle_antennaHeight.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("AntennaPivot," + Properties.Vehicle.Default.setVehicle_antennaPivot.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("AntennaOffset," + Properties.Vehicle.Default.setVehicle_antennaOffset.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsPivotBehindAntenna," + Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsSteerAxleAhead," + Properties.Vehicle.Default.setVehicle_isSteerAxleAhead.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("Wheelbase," + Properties.Vehicle.Default.setVehicle_wheelbase.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("MinTurningRadius," + Properties.Vehicle.Default.setVehicle_minTurningRadius.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("MinFixStep," + Properties.Settings.Default.setF_minFixStep.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("VehicleType," + Properties.Vehicle.Default.setVehicle_vehicleType.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("HeadingCorrection," + Properties.Settings.Default.HeadingCorrection.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("DualAntennaDistance," + Properties.Settings.Default.DualAntennaDistance.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");

                writer.WriteLine("GeoFenceDistance," + Properties.Vehicle.Default.set_geoFenceDistance.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("UTurnSkipWidth," + Properties.Vehicle.Default.set_youSkipWidth.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("YouTurnDistance," + Properties.Vehicle.Default.set_youTurnDistance.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("youTriggerDistance," + Properties.Vehicle.Default.set_youTriggerDistance.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("YouTurnUseDubins," + Properties.Vehicle.Default.Youturn_Type.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsMachineControlToAS," + Properties.Vehicle.Default.setVehicle_isMachineControlToAutoSteer.ToString(CultureInfo.InvariantCulture));

                //AutoSteer
                writer.WriteLine("pidP," + Properties.Vehicle.Default.setAS_Kp.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("LowSteerPWM," + Properties.Vehicle.Default.setAS_lowSteerPWM.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("pidD," + Properties.Vehicle.Default.setAS_Kd.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("pidO," + Properties.Vehicle.Default.setAS_Ko.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("SteerAngleOffset," + Properties.Vehicle.Default.setAS_steerAngleOffset.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("minPWM," + Properties.Vehicle.Default.setAS_minSteerPWM.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("HighSteerPWM," + Properties.Vehicle.Default.setAS_highSteerPWM.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("CountsPerDegree," + Properties.Vehicle.Default.setAS_countsPerDegree.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("MaxSteerAngle," + Properties.Vehicle.Default.setVehicle_maxSteerAngle.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("MaxAngularVelocity," + Properties.Vehicle.Default.setVehicle_maxAngularVelocity.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("SnapDistance," + Properties.Settings.Default.setAS_snapDistance.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("isStanleyUsed," + Properties.Vehicle.Default.setVehicle_isStanleyUsed.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("StanleyGain," + Properties.Vehicle.Default.setVehicle_stanleyGain.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("StanleyHeadingError," + Properties.Vehicle.Default.setVehicle_stanleyHeadingErrorGain.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("GoalPointLookAhead," +
                    Properties.Vehicle.Default.setVehicle_goalPointLookAhead.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("GoalPointLookAheadUTurnMult," +
                    Properties.Vehicle.Default.setVehicle_goalPointLookAheadUturnMult.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("GoalPointLookAheadMinumum," +
                    Properties.Vehicle.Default.setVehicle_lookAheadMinimum.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("GoalPointLookAheadDistanceFromLine," +
                    Properties.Vehicle.Default.setVehicle_lookAheadDistanceFromLine.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("HydLiftLookAhead," + Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");

                //IMU
                writer.WriteLine("HeadingFromSource," + Properties.Settings.Default.setGPS_headingFromWhichSource);
                writer.WriteLine("GPSWhichSentence," + Properties.Settings.Default.setGPS_fixFromWhichSentence.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("HeadingFromBrick," + Properties.Settings.Default.setIMU_isHeadingCorrectionFromBrick);
                writer.WriteLine("RollFromAutoSteer," + Properties.Settings.Default.setIMU_isRollFromAutoSteer);
                writer.WriteLine("HeadingFromAutoSteer," + Properties.Settings.Default.setIMU_isHeadingCorrectionFromAutoSteer);
                writer.WriteLine("IMUPitchZero," + Properties.Settings.Default.setIMU_pitchZeroX16.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IMURollZero," + Properties.Settings.Default.setIMU_rollZeroX16.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("IMUFusion," + Properties.Settings.Default.setIMU_fusionWeight.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");

                //Arduino steer Config
                writer.WriteLine("ArduinoInclinometer," + Properties.Vehicle.Default.setArdSteer_inclinometer.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("ArduinoMaxPulseCounts," + Properties.Vehicle.Default.setArdSteer_maxPulseCounts.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("ArduinoMaxSpeed," + Properties.Vehicle.Default.setArdSteer_maxSpeed.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("ArduinoMinSpeed," + Properties.Vehicle.Default.setArdSteer_minSpeed.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("ArduinoSetting0," + Properties.Vehicle.Default.setArdSteer_setting0.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("ArduinoSetting1," + Properties.Vehicle.Default.setArdSteer_setting1.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("ArduinoAckermanFix," + Properties.Vehicle.Default.setArdSteer_ackermanFix.ToString(CultureInfo.InvariantCulture));

                //Arduino Machine Config
                writer.WriteLine("ArdMachineRaiseTime," + Properties.Vehicle.Default.setArdMac_hydRaiseTime.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("ArdMachineLowerTime," + Properties.Vehicle.Default.setArdMac_hydLowerTime.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("ArdMachineEnableHydraulics," + Properties.Vehicle.Default.setArdMac_isHydEnabled.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("ArdSteerSetting2," + Properties.Vehicle.Default.setArdSteer_setting2);
                writer.WriteLine("ArdMacSetting0," + Properties.Vehicle.Default.setArdMac_setting0);

                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");

                //uturn sequences
                writer.WriteLine("SequenceFunctionEnter;" + Properties.Vehicle.Default.seq_FunctionEnter);
                writer.WriteLine("SequenceFunctionExit;" + Properties.Vehicle.Default.seq_FunctionExit);
                writer.WriteLine("SequenceActionEnter;" + Properties.Vehicle.Default.seq_ActionEnter);
                writer.WriteLine("SequenceActionExit;" + Properties.Vehicle.Default.seq_ActionExit);
                writer.WriteLine("SequenceDistanceEnter;" + Properties.Vehicle.Default.seq_DistanceEnter);
                writer.WriteLine("SequenceDistanceExit;" + Properties.Vehicle.Default.seq_DistanceExit);

                writer.WriteLine("FunctionList;" + Properties.Vehicle.Default.seq_FunctionList);
                writer.WriteLine("ActionList;" + Properties.Vehicle.Default.seq_ActionList);

                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
            }

            //little show to say saved and where
            TimedMessageBox(3000, String.Get("gsSavedInFolder"), vehiclesDirectory);
        }

        //function to open a previously saved field
        public bool FileOpenVehicle(string filename)
        {
            //OpenFileDialog ofd = new OpenFileDialog();

            ////get the directory where the fields are stored
            //string directoryName = vehiclesDirectory;

            ////make sure the directory exists, if not, create it
            //if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            //{ Directory.CreateDirectory(directoryName); }

            ////the initial directory, fields, for the open dialog
            //ofd.InitialDirectory = directoryName;

            ////When leaving dialog put windows back where it was
            //ofd.RestoreDirectory = true;

            ////set the filter to text files only
            //ofd.Filter = "txt files (*.txt)|*.txt";

            ////was a file selected
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    //if job started close it
            //    if (isJobStarted) JobClose();

            //make sure the file if fully valid and vehicle matches sections
            using (StreamReader reader = new StreamReader(filename))
            {
                try
                {
                    string line;
                    Properties.Vehicle.Default.setVehicle_vehicleName = filename;
                    string[] words;
                    line = reader.ReadLine(); words = line.Split(',');

                    //file version
                    string[] fullVers = words[1].Split('.');
                    string vers = fullVers[0] + fullVers[1];
                    int fileVersion = int.Parse(vers, CultureInfo.InvariantCulture);

                    //assembly version
                    string assemblyVersion = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);
                    fullVers = assemblyVersion.Split('.');
                    int appVersion = int.Parse(fullVers[0] + fullVers[1], CultureInfo.InvariantCulture);

                    if (fileVersion < appVersion)
                    {
                        TimedMessageBox(5000, String.Get("gsVehicleFileIsWrongVersion"), String.Get("gsMustBeVersion") + Application.ProductVersion.ToString(CultureInfo.InvariantCulture) + " or higher");
                        return false;
                    }
                    else
                    {
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_antennaHeight = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_antennaPivot = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_antennaOffset = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');

                        Properties.Vehicle.Default.setVehicle_isSteerAxleAhead = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_wheelbase = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_minTurningRadius = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setF_minFixStep = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_vehicleType = int.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.HeadingCorrection = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.DualAntennaDistance = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.set_geoFenceDistance = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.set_youSkipWidth = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.set_youTurnDistance = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.set_youTriggerDistance = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.Youturn_Type = byte.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_isMachineControlToAutoSteer = bool.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setAS_Kp = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setAS_lowSteerPWM = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setAS_Kd = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setAS_Ko = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setAS_steerAngleOffset = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setAS_minSteerPWM = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setAS_highSteerPWM = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setAS_countsPerDegree = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_maxSteerAngle = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_maxAngularVelocity = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setAS_snapDistance = int.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_isStanleyUsed = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_stanleyGain = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_stanleyHeadingErrorGain = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');

                        Properties.Vehicle.Default.setVehicle_goalPointLookAhead = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_goalPointLookAheadUturnMult = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_lookAheadMinimum = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_lookAheadDistanceFromLine = double.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead = double.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setGPS_headingFromWhichSource = (words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setGPS_fixFromWhichSentence = (words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIMU_isHeadingCorrectionFromBrick = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIMU_isRollFromAutoSteer = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIMU_isHeadingCorrectionFromAutoSteer = bool.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIMU_pitchZeroX16 = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIMU_rollZeroX16 = int.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIMU_fusionWeight = double.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setArdSteer_inclinometer = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setArdSteer_maxPulseCounts = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setArdSteer_maxSpeed = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setArdSteer_minSpeed = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setArdSteer_setting0 = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setArdSteer_setting1 = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setArdSteer_ackermanFix = byte.Parse(words[1], CultureInfo.InvariantCulture);

                        //Arduino Machine Config
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setArdMac_hydRaiseTime = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setArdMac_hydLowerTime = byte.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.setArdMac_isHydEnabled = byte.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine(); words = line.Split(',');
                        //if (words[0] == "Empty") Properties.Vehicle.Default.setVehicle_lookAheadDistanceFromLine = 0;
                        Properties.Vehicle.Default.setArdSteer_setting2 = byte.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine(); words = line.Split(',');
                        //if (words[0] == "Empty") Properties.Vehicle.Default.setVehicle_lookAheadDistanceFromLine = 0;
                        Properties.Vehicle.Default.setArdMac_setting0 = byte.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        line = reader.ReadLine(); words = line.Split(';');
                        Properties.Vehicle.Default.seq_FunctionEnter = words[1];
                        line = reader.ReadLine(); words = line.Split(';');
                        Properties.Vehicle.Default.seq_FunctionExit = words[1];
                        line = reader.ReadLine(); words = line.Split(';');
                        Properties.Vehicle.Default.seq_ActionEnter = words[1];
                        line = reader.ReadLine(); words = line.Split(';');
                        Properties.Vehicle.Default.seq_ActionExit = words[1];
                        line = reader.ReadLine(); words = line.Split(';');
                        Properties.Vehicle.Default.seq_DistanceEnter = words[1];
                        line = reader.ReadLine(); words = line.Split(';');
                        Properties.Vehicle.Default.seq_DistanceExit = words[1];

                        line = reader.ReadLine(); words = line.Split(';');
                        Properties.Vehicle.Default.seq_FunctionList = words[1];
                        line = reader.ReadLine(); words = line.Split(';');
                        Properties.Vehicle.Default.seq_ActionList = words[1];

                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        //fill in the current variables with restored data
                        vehicleFileName = Path.GetFileNameWithoutExtension(filename) + " - ";
                        Properties.Vehicle.Default.setVehicle_vehicleName = vehicleFileName;

                        Properties.Settings.Default.Save();
                        Properties.Vehicle.Default.Save();

                        //from settings grab the vehicle specifics
                        vehicle.antennaHeight = Properties.Vehicle.Default.setVehicle_antennaHeight;
                        vehicle.antennaPivot = Properties.Vehicle.Default.setVehicle_antennaPivot;
                        vehicle.antennaOffset = Properties.Vehicle.Default.setVehicle_antennaOffset;
                        vehicle.isPivotBehindAntenna = Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna;
                        vehicle.isSteerAxleAhead = Properties.Vehicle.Default.setVehicle_isSteerAxleAhead;

                        vehicle.wheelbase = Properties.Vehicle.Default.setVehicle_wheelbase;
                        vehicle.minTurningRadius = Properties.Vehicle.Default.setVehicle_minTurningRadius;
                        minFixStepDist = Properties.Settings.Default.setF_minFixStep;
                        HeadingCorrection = Properties.Settings.Default.HeadingCorrection;
                        DualAntennaDistance = Properties.Settings.Default.DualAntennaDistance;

                        vehicle.vehicleType = Properties.Vehicle.Default.setVehicle_vehicleType;

                        yt.geoFenceDistance = Properties.Vehicle.Default.set_geoFenceDistance;
                        yt.rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;
                        yt.youTurnStartOffset = Properties.Vehicle.Default.set_youTurnDistance;
                        yt.triggerDistanceOffset = Properties.Vehicle.Default.set_youTriggerDistance;
                        yt.YouTurnType = Properties.Vehicle.Default.Youturn_Type;
                        mc.isMachineDataSentToAutoSteer = Properties.Vehicle.Default.setVehicle_isMachineControlToAutoSteer;

                        mc.Config_AutoSteer[mc.ssKp] = Properties.Vehicle.Default.setAS_Kp;
                        mc.Config_AutoSteer[mc.ssLowPWM] = Properties.Vehicle.Default.setAS_lowSteerPWM;
                        mc.Config_AutoSteer[mc.ssKd] = Properties.Vehicle.Default.setAS_Kd;
                        mc.Config_AutoSteer[mc.ssKo] = Properties.Vehicle.Default.setAS_Ko;
                        mc.Config_AutoSteer[mc.ssSteerOffset] = Properties.Vehicle.Default.setAS_steerAngleOffset;
                        mc.Config_AutoSteer[mc.ssMinPWM] = Properties.Vehicle.Default.setAS_minSteerPWM;
                        mc.Config_AutoSteer[mc.ssHighPWM] = Properties.Vehicle.Default.setAS_highSteerPWM;
                        mc.Config_AutoSteer[mc.ssCountsPerDegree] = Properties.Vehicle.Default.setAS_countsPerDegree;

                        vehicle.maxSteerAngle = Properties.Vehicle.Default.setVehicle_maxSteerAngle;
                        vehicle.maxAngularVelocity = Properties.Vehicle.Default.setVehicle_maxAngularVelocity;

                        isStanleyUsed = Properties.Vehicle.Default.setVehicle_isStanleyUsed;
                        vehicle.stanleyGain = Properties.Vehicle.Default.setVehicle_stanleyGain;
                        vehicle.stanleyHeadingErrorGain = Properties.Vehicle.Default.setVehicle_stanleyHeadingErrorGain;

                        vehicle.goalPointLookAheadSeconds = Properties.Vehicle.Default.setVehicle_goalPointLookAhead;
                        vehicle.goalPointLookAheadMinimumDistance = Properties.Vehicle.Default.setVehicle_lookAheadMinimum;
                        vehicle.goalPointDistanceMultiplier = Properties.Vehicle.Default.setVehicle_lookAheadDistanceFromLine;
                        vehicle.goalPointLookAheadUturnMult = Properties.Vehicle.Default.setVehicle_goalPointLookAheadUturnMult;

                        vehicle.hydLiftLookAheadTime = Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead;

                        headingFromSource = Properties.Settings.Default.setGPS_headingFromWhichSource;
                        pn.fixFrom = Properties.Settings.Default.setGPS_fixFromWhichSentence;

                        ahrs.isHeadingCorrectionFromBrick = Properties.Settings.Default.setIMU_isHeadingCorrectionFromBrick;
                        ahrs.isRollFromAutoSteer = Properties.Settings.Default.setIMU_isRollFromAutoSteer;
                        ahrs.isHeadingCorrectionFromAutoSteer = Properties.Settings.Default.setIMU_isHeadingCorrectionFromAutoSteer;

                        ahrs.pitchZeroX16 = Properties.Settings.Default.setIMU_pitchZeroX16;
                        ahrs.rollZeroX16 = Properties.Settings.Default.setIMU_rollZeroX16;

                        ahrs.fusionWeight = Properties.Settings.Default.setIMU_fusionWeight;

                        mc.Config_ardSteer[0] = 127; //PGN - 32750
                        mc.Config_ardSteer[1] = 238;
                        mc.Config_ardSteer[mc.arSet0] = Properties.Vehicle.Default.setArdSteer_setting0;
                        mc.Config_ardSteer[mc.arSet1] = Properties.Vehicle.Default.setArdSteer_setting1;
                        mc.Config_ardSteer[mc.arMaxSpd] = Properties.Vehicle.Default.setArdSteer_maxSpeed;
                        mc.Config_ardSteer[mc.arMinSpd] = Properties.Vehicle.Default.setArdSteer_minSpeed;
                        mc.Config_ardSteer[mc.arAckermanFix] = Properties.Vehicle.Default.setArdSteer_ackermanFix;

                        byte inc = (byte)(Properties.Vehicle.Default.setArdSteer_inclinometer << 6);
                        mc.Config_ardSteer[mc.arIncMaxPulse] = (byte)(inc + (byte)Properties.Vehicle.Default.setArdSteer_maxPulseCounts);

                        mc.Config_ardSteer[mc.arSet2] = Properties.Vehicle.Default.setArdSteer_setting2;

                        mc.Config_ardMachine[mc.amRaiseTime] = Properties.Vehicle.Default.setArdMac_hydRaiseTime;
                        mc.Config_ardMachine[mc.amLowerTime] = Properties.Vehicle.Default.setArdMac_hydLowerTime;
                        mc.Config_ardMachine[mc.amEnableHyd] = Properties.Vehicle.Default.setArdMac_isHydEnabled;
                        mc.Config_ardMachine[mc.amSet0] = Properties.Vehicle.Default.setArdMac_setting0;

                        string sentence = Properties.Vehicle.Default.seq_FunctionEnter;
                        words = sentence.Split(',');
                        for (int i = 0; i < FormGPS.MAXFUNCTIONS; i++) int.TryParse(words[i], out seq.seqEnter[i].function);

                        sentence = Properties.Vehicle.Default.seq_ActionEnter;
                        words = sentence.Split(',');
                        for (int i = 0; i < FormGPS.MAXFUNCTIONS; i++) int.TryParse(words[i], out seq.seqEnter[i].action);

                        sentence = Properties.Vehicle.Default.seq_DistanceEnter;
                        words = sentence.Split(',');
                        for (int i = 0; i < FormGPS.MAXFUNCTIONS; i++)
                            double.TryParse(words[i], NumberStyles.Float, CultureInfo.InvariantCulture, out seq.seqEnter[i].distance);

                        sentence = Properties.Vehicle.Default.seq_FunctionExit;
                        words = sentence.Split(',');
                        for (int i = 0; i < FormGPS.MAXFUNCTIONS; i++) int.TryParse(words[i], out seq.seqExit[i].function);

                        sentence = Properties.Vehicle.Default.seq_ActionExit;
                        words = sentence.Split(',');
                        for (int i = 0; i < FormGPS.MAXFUNCTIONS; i++) int.TryParse(words[i], out seq.seqExit[i].action);

                        sentence = Properties.Vehicle.Default.seq_DistanceExit;
                        words = sentence.Split(',');
                        for (int i = 0; i < FormGPS.MAXFUNCTIONS; i++)
                            double.TryParse(words[i], NumberStyles.Float, CultureInfo.InvariantCulture, out seq.seqExit[i].distance);

                    }
                    return true;
                }
                catch (Exception e) //FormatException e || IndexOutOfRangeException e2)
                {
                    WriteErrorLog("Open Vehicle" + e.ToString());

                    //vehicle is corrupt, reload with all default information
                    Properties.Vehicle.Default.Reset();
                    Properties.Vehicle.Default.Save();
                    Properties.Settings.Default.Reset();
                    Properties.Settings.Default.Save();
                    MessageBox.Show(String.Get("gsProgramWillResetToRecoverPleaseRestart"), String.Get("gsVehicleFileIsCorrupt"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Application.Restart();
                    Environment.Exit(0);
                    return false;
                }
            }
        }//end of open file

        //function that save vehicle and section settings
        public void FileSaveTool(string FileName)
        {
            CheckToolSettings();

            string dir = Path.GetDirectoryName(toolsDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            toolFileName = Path.GetFileNameWithoutExtension(FileName) + " - ";
            Properties.Vehicle.Default.setVehicle_toolName = toolFileName;
            Properties.Vehicle.Default.Save();

            using (StreamWriter writer = new StreamWriter(FileName))
            {
                writer.WriteLine("Version," + Application.ProductVersion.ToString(CultureInfo.InvariantCulture));


                writer.WriteLine("ToolCount," + Properties.Vehicle.Default.ToolSettings.Count.ToString());
                for (int i = 0; i < Properties.Vehicle.Default.ToolSettings.Count; i++)
                {
                    writer.WriteLine("LookAheadOn," + Properties.Vehicle.Default.ToolSettings[i].LookAheadOn.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("LookAheadOff," + Properties.Vehicle.Default.ToolSettings[i].LookAheadOff.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("TurnOffDelay," + Properties.Vehicle.Default.ToolSettings[i].TurnOffDelay.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("MappingOnDelay," + Properties.Vehicle.Default.ToolSettings[i].MappingOnDelay.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("MappingOffDelay," + Properties.Vehicle.Default.ToolSettings[i].MappingOffDelay.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("HitchLength," + Properties.Vehicle.Default.ToolSettings[i].HitchLength.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("TankWheelLength," + Properties.Vehicle.Default.ToolSettings[i].TankWheelLength.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("TankHitchLength," + Properties.Vehicle.Default.ToolSettings[i].TankHitchLength.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("ToolWheelLength," + Properties.Vehicle.Default.ToolSettings[i].ToolWheelLength.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("ToolHitchLength," + Properties.Vehicle.Default.ToolSettings[i].ToolHitchLength.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("IsToolBehindPivot," + Properties.Vehicle.Default.ToolSettings[i].BehindPivot.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IsToolTrailing," + Properties.Vehicle.Default.ToolSettings[i].Trailing.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IsToolTBT," + Properties.Vehicle.Default.ToolSettings[i].TBT.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("ToolMinUnappliedPixels," + Properties.Vehicle.Default.ToolSettings[i].MinApplied.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("Sections," + Properties.Vehicle.Default.ToolSettings[i].Sections.Count.ToString());
                    for (int j = 0; j < Properties.Vehicle.Default.ToolSettings[i].Sections.Count; j++)
                    {
                        writer.WriteLine("Section" + (j + 1).ToString() + "," + Properties.Vehicle.Default.ToolSettings[i].Sections[j][0].ToString() + "," + Properties.Vehicle.Default.ToolSettings[i].Sections[j][1].ToString() + "," + Properties.Vehicle.Default.ToolSettings[i].Sections[j][2].ToString());
                    }
                }

                writer.WriteLine("GuidanceWidth," + Properties.Vehicle.Default.GuidanceWidth.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("GuidanceOverlap," + Properties.Vehicle.Default.GuidanceOverlap.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("GuidanceOffset," + Properties.Vehicle.Default.GuidanceOffset.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");

                writer.WriteLine("WorkSwitch," + Properties.Vehicle.Default.setF_IsWorkSwitchEnabled.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("ActiveLow," + Properties.Vehicle.Default.setF_IsWorkSwitchActiveLow.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("SwitchManual," + Properties.Vehicle.Default.setF_IsWorkSwitchManual.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("AutoSteerRemote," + Properties.Vehicle.Default.setAS_isAutoSteerAutoOn.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("Empty," + "10");

                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");

                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");

                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
            }

            //little show to say saved and where
            TimedMessageBox(3000, String.Get("gsSavedInFolder"), toolsDirectory);
        }

        //function to open a previously saved field
        public bool FileOpenTool(string fileName)
        {
            //make sure the file if fully valid and vehicle matches sections
            using (StreamReader reader = new StreamReader(fileName))
            {
                try
                {
                    string line;
                    Properties.Vehicle.Default.setVehicle_toolName = fileName;
                    string[] words;
                    line = reader.ReadLine(); words = line.Split(',');

                    string vers = words[1].Replace('.', '0');
                    int fileVersion = int.Parse(vers, CultureInfo.InvariantCulture);

                    string assemblyVersion = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);
                    assemblyVersion = assemblyVersion.Replace('.', '0');
                    int appVersion = int.Parse(assemblyVersion, CultureInfo.InvariantCulture);

                    appVersion /= 100;
                    fileVersion /= 100;

                    if (fileVersion < appVersion)
                    {
                        TimedMessageBox(5000, String.Get("gsFileError"), String.Get("gsMustBeVersion") + Application.ProductVersion.ToString(CultureInfo.InvariantCulture) + " or higher");
                        return false;
                    }

                    else
                    {
                        List<ToolSettings> test = new List<ToolSettings>();

                        line = reader.ReadLine(); words = line.Split(',');
                        int count = int.Parse(words[1], CultureInfo.InvariantCulture);
                        for (int i = 0; i < count; i++)
                        {
                            test.Add(new ToolSettings());

                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].LookAheadOn = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].LookAheadOff = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].TurnOffDelay = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].MappingOnDelay = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].MappingOffDelay = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].HitchLength = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].TankWheelLength = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].TankHitchLength = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].ToolWheelLength = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].ToolHitchLength = double.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].BehindPivot = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].Trailing = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].TBT = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            test[i].MinApplied = int.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine(); words = line.Split(',');
                            int count2 = int.Parse(words[1], CultureInfo.InvariantCulture);
                            for (int j = 0; j < count2; j++)
                            {
                                line = reader.ReadLine(); words = line.Split(',');
                                test[i].Sections.Add(new double[] { double.Parse(words[1], CultureInfo.InvariantCulture), double.Parse(words[2], CultureInfo.InvariantCulture), double.Parse(words[3], CultureInfo.InvariantCulture) });
                            }
                        }
                        Properties.Vehicle.Default.ToolSettings = test;

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.GuidanceWidth = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.GuidanceOverlap = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Vehicle.Default.GuidanceOffset = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        line = reader.ReadLine(); words = line.Split(',');
                        mc.isWorkSwitchEnabled = Properties.Vehicle.Default.setF_IsWorkSwitchEnabled = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        mc.isWorkSwitchActiveLow = Properties.Vehicle.Default.setF_IsWorkSwitchActiveLow = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        mc.isWorkSwitchManual = Properties.Vehicle.Default.setF_IsWorkSwitchManual = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        mc.RemoteAutoSteer = Properties.Vehicle.Default.setAS_isAutoSteerAutoOn = bool.Parse(words[1]);
                        line = reader.ReadLine();



                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        //fill in the current variables with restored data
                        toolFileName = Path.GetFileNameWithoutExtension(fileName) + " - ";
                        Properties.Vehicle.Default.setVehicle_toolName = toolFileName;

                        Properties.Vehicle.Default.Save();

                        LoadTools();

                        return true;
                    }
                }
                catch (Exception e)
                {
                    WriteErrorLog("pen Tool" + e.ToString());

                    TimedMessageBox(3000, String.Get("gsFileError"), String.Get("gsVehicleFileIsCorrupt"));

                    return false;
                }
            }    //cancelled out of open file
        }//end of open file

        //function that save vehicle and section settings
        public void FileSaveEnvironment(string FileName)
        {
            string dir = Path.GetDirectoryName(envDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            envFileName = Path.GetFileNameWithoutExtension(FileName);
            Properties.Vehicle.Default.setVehicle_envName = envFileName;
            Properties.Vehicle.Default.Save();

            using (StreamWriter writer = new StreamWriter(FileName))
            {
                writer.WriteLine("Version," + Application.ProductVersion.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("Culture," + Properties.Settings.Default.setF_culture.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("CamPitch," + Properties.Settings.Default.setDisplay_camPitch.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("Empty,10");
                writer.WriteLine("LightBarCMPerPixel," + Properties.Settings.Default.setDisplay_lightbarCmPerPixel.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("LineWidth," + Properties.Settings.Default.setDisplay_lineWidth.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("IsCompassOn," + Properties.Settings.Default.setMenu_isCompassOn.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsGridOn," + Properties.Settings.Default.setMenu_isGridOn.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsLightBarOn," + Properties.Settings.Default.setMenu_isLightbarOn.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsLogNMEA," + Properties.Settings.Default.setMenu_isLogNMEA.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsMetric," + Properties.Settings.Default.setMenu_isMetric.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsOGLZoom," + Properties.Settings.Default.setMenu_isOGLZoomOn.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("IsPurePursuitLineOn," + Properties.Settings.Default.setMenu_isPureOn.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsGuideLinesOn," + Properties.Settings.Default.setMenu_isSideGuideLines.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsSimulatorOn," + Properties.Settings.Default.setMenu_isSimulatorOn.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsSkyOn," + Properties.Settings.Default.setMenu_isSkyOn.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsSpeedoOn," + Properties.Settings.Default.setMenu_isSpeedoOn.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsUTurnAlwaysOn," + Properties.Settings.Default.setMenu_isUTurnAlwaysOn.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsAutoDayNightModeOn," + Properties.Settings.Default.setDisplay_isAutoDayNight.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("StartFullScreen," + Properties.Settings.Default.setDisplay_isStartFullScreen.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsRTKOn," + Properties.Settings.Default.setGPS_isRTK.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("NMEA_Hz," + Properties.Settings.Default.setPort_NMEAHz.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");

                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");

                writer.WriteLine("IsNtripCasterIP," + Properties.Settings.Default.setNTRIP_casterIP.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripCasterPort," + Properties.Settings.Default.setNTRIP_casterPort.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripCasterURL," + Properties.Settings.Default.setNTRIP_casterURL.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripGGAManual," + Properties.Settings.Default.setNTRIP_isGGAManual.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripOn," + Properties.Settings.Default.setNTRIP_isOn.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripTCP," + Properties.Settings.Default.setNTRIP_isTCP.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripManualLat," + Properties.Settings.Default.setNTRIP_manualLat.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripManualLon," + Properties.Settings.Default.setNTRIP_manualLon.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripMount," + Properties.Settings.Default.setNTRIP_mount.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripGGAInterval," + Properties.Settings.Default.setNTRIP_sendGGAInterval.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripSendToUDPPort," + Properties.Settings.Default.setNTRIP_sendToUDPPort.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripUserName," + Properties.Settings.Default.setNTRIP_userName.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripUserPassword," + Properties.Settings.Default.setNTRIP_userPassword.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("IsUDPOn," + Properties.Settings.Default.setUDP_isOn.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("GPSSimLatitude," + Properties.Settings.Default.setGPS_SimLatitude.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("GPSSimLongitude" + "," + Properties.Settings.Default.setGPS_SimLongitude.ToString(CultureInfo.InvariantCulture));


                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");


                writer.WriteLine("FieldColorDay," + Properties.Settings.Default.setDisplay_colorFieldDay.ToArgb().ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("SectionColorDay," + Properties.Settings.Default.setDisplay_colorSectionsDay.ToArgb().ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("FieldColorNight," + Properties.Settings.Default.setDisplay_colorFieldNight.ToArgb().ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("SectionColorNight," + Properties.Settings.Default.setDisplay_colorSectionsNight.ToArgb().ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("DayColor," + Properties.Settings.Default.setDisplay_colorDayMode.ToArgb().ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("NightColor," + Properties.Settings.Default.setDisplay_colorNightMode.ToArgb().ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsSimple," + Properties.Settings.Default.setDisplay_isSimple.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsDayMode," + Properties.Settings.Default.setDisplay_isDayMode.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("CustomColors," + Properties.Settings.Default.setDisplay_customColors.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
            }

            //little show to say saved and where
            TimedMessageBox(3000, String.Get("gsSavedInFolder"), envDirectory);
        }

        //function to open a previously saved field
        public DialogResult FileOpenEnvironment(string fileName)
        {
            //make sure the file if fully valid and vehicle matches sections
            using (StreamReader reader = new StreamReader(fileName))
            {
                try
                {
                    string line;
                    Properties.Vehicle.Default.setVehicle_envName = fileName;
                    string[] words;
                    line = reader.ReadLine(); words = line.Split(',');


                    string vers = words[1].Replace('.', '0');
                    int fileVersion = int.Parse(vers, CultureInfo.InvariantCulture);

                    string assemblyVersion = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);
                    assemblyVersion = assemblyVersion.Replace('.', '0');
                    int appVersion = int.Parse(assemblyVersion, CultureInfo.InvariantCulture);

                    appVersion /= 100;
                    fileVersion /= 100;

                    if (fileVersion < appVersion)
                    {
                        TimedMessageBox(5000, String.Get("gsFileError"), String.Get("gsMustBeVersion") + Application.ProductVersion.ToString(CultureInfo.InvariantCulture) + " or higher");
                        return DialogResult.Abort;
                    }

                    else
                    {
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setF_culture = (words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_camPitch = double.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine(); words = line.Split(',');
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_lightbarCmPerPixel = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_lineWidth = int.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setMenu_isCompassOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setMenu_isGridOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setMenu_isLightbarOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setMenu_isLogNMEA = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setMenu_isMetric = bool.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setMenu_isOGLZoomOn = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setMenu_isPureOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setMenu_isSideGuideLines = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setMenu_isSimulatorOn = bool.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setMenu_isSkyOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setMenu_isSpeedoOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setMenu_isUTurnAlwaysOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_isAutoDayNight = bool.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_isStartFullScreen = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setGPS_isRTK = bool.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        //if (words[0] == "Empty") Properties.Settings.Default.setPort_NMEAHz = 5;
                        Properties.Settings.Default.setPort_NMEAHz = int.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_casterIP = words[1];
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_casterPort = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_casterURL = words[1];

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_isGGAManual = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_isOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_isTCP = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_manualLat = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_manualLon = double.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_mount = (words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_sendGGAInterval = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_sendToUDPPort = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_userName = (words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_userPassword = (words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setUDP_isOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setGPS_SimLatitude = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setGPS_SimLongitude = double.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_colorFieldDay = Color.FromArgb(int.Parse(words[1], CultureInfo.InvariantCulture));
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_colorSectionsDay = Color.FromArgb(int.Parse(words[1], CultureInfo.InvariantCulture));
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_colorFieldNight = Color.FromArgb(int.Parse(words[1], CultureInfo.InvariantCulture));
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_colorSectionsNight = Color.FromArgb(int.Parse(words[1], CultureInfo.InvariantCulture));
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_colorDayMode = Color.FromArgb(int.Parse(words[1], CultureInfo.InvariantCulture));
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_colorNightMode = Color.FromArgb(int.Parse(words[1], CultureInfo.InvariantCulture));

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_isSimple = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_isDayMode = bool.Parse(words[1]);

                        line = reader.ReadLine();
                        Properties.Settings.Default.setDisplay_customColors = line.Substring(13);

                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        //fill in the current variables with restored data
                        envFileName = Path.GetFileNameWithoutExtension(fileName);
                        Properties.Vehicle.Default.setVehicle_envName = envFileName;

                        Properties.Settings.Default.Save();
                        Properties.Vehicle.Default.Save();
                    }

                    return DialogResult.OK;
                }
                catch (Exception e) //FormatException e || IndexOutOfRangeException e2)
                {
                    WriteErrorLog("Open Vehicle" + e.ToString());

                    //vehicle is corrupt, reload with all default information
                    Properties.Settings.Default.Reset();
                    Properties.Settings.Default.Save();

                    TimedMessageBox(3000, String.Get("gsFileError"), String.Get("gsVehicleFileIsCorrupt"));
                    return DialogResult.Cancel;
                }
            }
        }//end of open file

        //function to open a previously saved field, resume, open exisiting, open named field
        public void FileOpenField(string _openType)
        {
            string fileAndDirectory = "";
            if (_openType.Contains("Field.txt"))
            {
                fileAndDirectory = _openType;
                _openType = "Load";
            }
            else fileAndDirectory = "Cancel";

            //get the directory where the fields are stored
            //string directoryName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+ "\\fields\\";
            switch (_openType)
            {
                case "Resume":
                    {
                        //Either exit or update running save
                        fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Field.txt";
                        if (!File.Exists(fileAndDirectory)) fileAndDirectory = "Cancel";
                        break;
                    }

                case "Open":
                    {
                        //create the dialog instance
                        OpenFileDialog ofd = new OpenFileDialog();

                        //the initial directory, fields, for the open dialog
                        ofd.InitialDirectory = fieldsDirectory;

                        //When leaving dialog put windows back where it was
                        ofd.RestoreDirectory = true;

                        //set the filter to text files only
                        ofd.Filter = "Field files (Field.txt)|Field.txt";

                        //was a file selected
                        if (ofd.ShowDialog(this) == DialogResult.Cancel) fileAndDirectory = "Cancel";
                        else fileAndDirectory = ofd.FileName;
                        break;
                    }
            }

            if (fileAndDirectory == "Cancel") return;

            isJobStarted = true;

            List<Task> tasks = new List<Task>();
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
                else
                {
                    TaskList[j].Token.Cancel();
                }
            }

            Task NewTask = Task_JobNew(fileAndDirectory, tasks);
            TaskList.Add(new TaskClass(NewTask, null, TaskName.OpenJob, new CancellationTokenSource()));
            tasks.Add(NewTask);
        }
        
        public void FileOpenField2(string fileAndDirectory)
        {
            try
            {
                //start to read the file
                string line;
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //Date time line
                        line = reader.ReadLine();

                        //dir header $FieldDir
                        line = reader.ReadLine();

                        //read field directory
                        line = reader.ReadLine();

                        currentFieldDirectory = line.Trim();

                        //Offset header
                        line = reader.ReadLine();

                        //read the Offsets 
                        line = reader.ReadLine();
                        string[] offs = line.Split(',');

                        pn.utmEast = int.Parse(offs[0]);
                        pn.utmNorth = int.Parse(offs[1]);
                        pn.zone = int.Parse(offs[2]);

                        //create a new grid
                        worldGrid.CheckWorldGrid(pn.actualNorthing - pn.utmNorth, pn.actualEasting - pn.utmEast);

                        //convergence angle update
                        if (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            line = reader.ReadLine();
                            pn.convergenceAngle = double.Parse(line, CultureInfo.InvariantCulture);
                        }

                        //start positions
                        if (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            line = reader.ReadLine();
                            offs = line.Split(',');

                            pn.latStart = double.Parse(offs[0], CultureInfo.InvariantCulture);
                            pn.lonStart = double.Parse(offs[1], CultureInfo.InvariantCulture);
                        }
                    }

                    catch (Exception e)
                    {
                        WriteErrorLog("While Opening Field" + e.ToString());
                        TimedMessageBox(2000, String.Get("gsFieldFileIsCorrupt"), String.Get("gsChooseADifferentField"));

                        StartTasks(null, 0, TaskName.CloseJob);
                        return;
                    }
                }

                // ABLine -------------------------------------------------------------------------------------------------
                FileLoadABLines();

                //CurveLines
                FileLoadCurveLines();

                //section patches
                fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Sections.txt";
                if (!File.Exists(fileAndDirectory))
                {
                    TimedMessageBox(2000, String.Get("gsMissingSectionFile"), String.Get("gsButFieldIsLoaded"));
                    //return;
                }
                else
                {
                    bool isv3 = false;
                    using (StreamReader reader = new StreamReader(fileAndDirectory))
                    {
                        try
                        {
                            fd.workedAreaTotal = 0;
                            fd.distanceUser = 0;
                            Vec3 vecFix = new Vec3();

                            //read header
                            while (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();
                                if (line.Contains("ect"))
                                {
                                    isv3 = true;
                                    break;
                                }
                                int verts = int.Parse(line);


                                PatchDrawList.Add(new List<Vec3>());
                                int idx = PatchDrawList.Count - 1;
                                for (int v = 0; v < verts; v++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vecFix.Easting = double.Parse(words[0], CultureInfo.InvariantCulture);
                                    vecFix.Northing = double.Parse(words[1], CultureInfo.InvariantCulture);
                                    vecFix.Heading = double.Parse(words[2], CultureInfo.InvariantCulture);
                                    PatchDrawList[idx].Add(vecFix);
                                }
                                //calculate area of this patch - AbsoluteValue of (Ax(By-Cy) + Bx(Cy-Ay) + Cx(Ay-By)/2)
                                verts -= 2;
                                if (verts >= 2)
                                {
                                    for (int j = 1; j < verts; j++)
                                    {
                                        double temp = 0;
                                        temp = PatchDrawList[idx][j].Easting * (PatchDrawList[idx][j + 1].Northing - PatchDrawList[idx][j + 2].Northing) +
                                               PatchDrawList[idx][j + 1].Easting * (PatchDrawList[idx][j + 2].Northing - PatchDrawList[idx][j].Northing) +
                                               PatchDrawList[idx][j + 2].Easting * (PatchDrawList[idx][j].Northing - PatchDrawList[idx][j + 1].Northing);

                                        fd.workedAreaTotal += Math.Abs((temp * 0.5));
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            WriteErrorLog("Section file" + e.ToString());

                            TimedMessageBox(2000, String.Get("gsSectionFileIsCorrupt"), String.Get("gsButFieldIsLoaded"));
                        }

                    }

                    //was old version prior to v4
                    if (isv3)
                    {
                        //Append the current list to the field file
                        using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Sections.txt"), false))
                        {
                        }
                    }
                }

                // Contour points ----------------------------------------------------------------------------

                fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Contour.txt";
                if (!File.Exists(fileAndDirectory))
                {
                    TimedMessageBox(2000, String.Get("gsMissingContourFile"), String.Get("gsButFieldIsLoaded"));
                    //return;
                }

                //Points in Patch followed by easting, heading, northing, altitude
                else
                {
                    using (StreamReader reader = new StreamReader(fileAndDirectory))
                    {
                        try
                        {
                            //read header
                            line = reader.ReadLine();

                            while (!reader.EndOfStream)
                            {
                                //read how many vertices in the following patch
                                line = reader.ReadLine();
                                int verts = int.Parse(line);

                                Vec3 vecFix = new Vec3(0, 0, 0);

                                ct.stripList.Add(new List<Vec3>());

                                for (int v = 0; v < verts; v++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vecFix.Easting = double.Parse(words[0], CultureInfo.InvariantCulture);
                                    vecFix.Northing = double.Parse(words[1], CultureInfo.InvariantCulture);
                                    vecFix.Heading = double.Parse(words[2], CultureInfo.InvariantCulture);
                                    ct.stripList[ct.stripList.Count - 1].Add(vecFix);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            WriteErrorLog("Loading Contour file" + e.ToString());

                            TimedMessageBox(2000, String.Get("gsContourFileIsCorrupt"), String.Get("gsButFieldIsLoaded"));
                        }
                    }
                }


                // Flags -------------------------------------------------------------------------------------------------

                //Either exit or update running save
                fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Flags.txt";
                if (File.Exists(fileAndDirectory))
                {
                    flagPts.Clear();
                    using (StreamReader reader = new StreamReader(fileAndDirectory))
                    {
                        try
                        {
                            //read header
                            line = reader.ReadLine();

                            //number of flags
                            line = reader.ReadLine();
                            int points = int.Parse(line);

                            if (points > 0)
                            {
                                double lat;
                                double longi;
                                double east;
                                double nort;
                                double head;
                                int color, ID;
                                string notes;

                                for (int v = 0; v < points; v++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');

                                    if (words.Length == 8)
                                    {
                                        lat = double.Parse(words[0], CultureInfo.InvariantCulture);
                                        longi = double.Parse(words[1], CultureInfo.InvariantCulture);
                                        east = double.Parse(words[2], CultureInfo.InvariantCulture);
                                        nort = double.Parse(words[3], CultureInfo.InvariantCulture);
                                        head = double.Parse(words[4], CultureInfo.InvariantCulture);
                                        color = int.Parse(words[5]);
                                        ID = int.Parse(words[6]);
                                        notes = words[7].Trim();
                                    }
                                    else
                                    {
                                        lat = double.Parse(words[0], CultureInfo.InvariantCulture);
                                        longi = double.Parse(words[1], CultureInfo.InvariantCulture);
                                        east = double.Parse(words[2], CultureInfo.InvariantCulture);
                                        nort = double.Parse(words[3], CultureInfo.InvariantCulture);
                                        head = 0;
                                        color = int.Parse(words[4]);
                                        ID = int.Parse(words[5]);
                                        notes = "";
                                    }

                                    CFlag flagPt = new CFlag(lat, longi, east, nort, head, color, ID, notes);
                                    flagPts.Add(flagPt);
                                }
                            }
                        }

                        catch (Exception e)
                        {
                            TimedMessageBox(2000, String.Get("gsFlagFileIsCorrupt"), String.Get("gsButFieldIsLoaded"));
                            WriteErrorLog("FieldOpen, Loading Flags, Corrupt Flag File" + e.ToString());
                        }
                    }
                }

                //Boundaries
                //Either exit or update running save
                fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Boundary.txt";
                if (!File.Exists(fileAndDirectory))
                    fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Boundary.Tmp";

                if (!File.Exists(fileAndDirectory))
                {
                    TimedMessageBox(2000, String.Get("gsMissingBoundaryFile"), String.Get("gsButFieldIsLoaded"));
                }
                else
                {
                    using (StreamReader reader = new StreamReader(fileAndDirectory))
                    {
                        try
                        {
                            //read header
                            line = reader.ReadLine();//Boundary

                            while (true)
                            {
                                if (reader.EndOfStream) break;
                                CBoundaryLines newbnd = new CBoundaryLines();

                                //True or False OR points from older boundary files
                                line = reader.ReadLine();

                                //Check for older boundary files, then above line string is num of points
                                if (line == "True" || line == "False")
                                {
                                    newbnd.isDriveThru = bool.Parse(line);
                                    line = reader.ReadLine(); //number of points

                                    newbnd.isDriveAround = bool.Parse(line);
                                    line = reader.ReadLine(); //number of points
                                }

                                int numPoints = int.Parse(line);

                                if (numPoints > 0)
                                {
                                    //load the line
                                    for (int i = 0; i < numPoints; i++)
                                    {
                                        line = reader.ReadLine();
                                        if (line == null) break;
                                        string[] words = line.Split(',');
                                        Vec3 vecPt = new Vec3(
                                        double.Parse(words[1], CultureInfo.InvariantCulture),
                                            double.Parse(words[0], CultureInfo.InvariantCulture),
                                            double.Parse(words[2], CultureInfo.InvariantCulture));
                                        newbnd.bndLine.Add(vecPt);
                                    }

                                    bnd.bndArr.Add(newbnd);

                                    StartTasks(newbnd, bnd.bndArr.Count - 1, TaskName.Boundary);
                                }
                                if (reader.EndOfStream) break;
                            }
                        }
                        catch (Exception e)
                        {
                            TimedMessageBox(2000, String.Get("gsBoundaryLineFilesAreCorrupt"), String.Get("gsButFieldIsLoaded"));
                            WriteErrorLog("Load Boundary Line" + e.ToString());
                        }
                    }
                }

                // Headland  -------------------------------------------------------------------------------------------------
                fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Headland.txt";

                if (!File.Exists(fileAndDirectory)) fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Headland.Tmp";
                if (File.Exists(fileAndDirectory))
                {
                    using (StreamReader reader = new StreamReader(fileAndDirectory))
                    {
                        try
                        {
                            //read header
                            line = reader.ReadLine();//writer.WriteLine("$Headland");
                            line = reader.ReadLine();
                            int headArr = int.Parse(line);

                            for (int i = 0; i < headArr && i < bnd.bndArr.Count; i++)//hd.headArr.Count
                            {
                                line = reader.ReadLine();
                                int numHeadLine = int.Parse(line);
                                for (int j = 0; j < numHeadLine; j++)//hd.headArr[i].HeadLine.Count
                                {
                                    bnd.bndArr[i].HeadLine.Add(new List<Vec3>());
                                    line = reader.ReadLine();
                                    int numHeadLinecount = int.Parse(line);//hd.headArr[i].HeadLine[j].Count

                                    for (int k = 0; k < numHeadLinecount; k++)
                                    {
                                        line = reader.ReadLine();
                                        string[] words = line.Split(',');
                                        Vec3 vecPt = new Vec3(
                                            double.Parse(words[1], CultureInfo.InvariantCulture),
                                            double.Parse(words[0], CultureInfo.InvariantCulture),
                                            double.Parse(words[2], CultureInfo.InvariantCulture));
                                        bnd.bndArr[i].HeadLine[j].Add(vecPt);
                                    }
                                }

                                StartTasks(bnd.bndArr[i], i, TaskName.HeadLand);
                            }
                        }
                        catch (Exception e)
                        {
                            TimedMessageBox(2000, "Headland File is Corrupt", "But Field is Loaded");
                            WriteErrorLog("Load Headland Loop" + e.ToString());
                        }
                    }
                }

                //Recorded Path
                fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\RecPath.txt";
                if (File.Exists(fileAndDirectory))
                {
                    using (StreamReader reader = new StreamReader(fileAndDirectory))
                    {
                        try
                        {
                            //read header
                            line = reader.ReadLine();
                            line = reader.ReadLine();
                            int numPoints = int.Parse(line);
                            recPath.recList.Clear();

                            while (!reader.EndOfStream)
                            {
                                for (int v = 0; v < numPoints; v++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    CRecPathPt point = new CRecPathPt(
                                        double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture),
                                        double.Parse(words[2], CultureInfo.InvariantCulture),
                                        double.Parse(words[3], CultureInfo.InvariantCulture),
                                        bool.Parse(words[4]));

                                    //add the point
                                    recPath.recList.Add(point);
                                }
                            }
                        }

                        catch (Exception e)
                        {
                            TimedMessageBox(2000, String.Get("gsRecordedPathFileIsCorrupt"), String.Get("gsButFieldIsLoaded"));
                            WriteErrorLog("Load Recorded Path" + e.ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
            
            
            }
        }//end of open file

        //creates the field file when starting new field
        public void FileCreateField()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            if (!isJobStarted)
            {
                TimedMessageBox(3000, String.Get("gsFieldNotOpen"), String.Get("gsCreateNewField"));
                return;
            }
            string myFileName, dirField;

            //get the directory and make sure it exists, create if not
            dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            myFileName = "Field.txt";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));

                writer.WriteLine("$FieldDir");
                writer.WriteLine(currentFieldDirectory.ToString(CultureInfo.InvariantCulture));

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine(pn.utmEast.ToString(CultureInfo.InvariantCulture) + "," +
                    pn.utmNorth.ToString(CultureInfo.InvariantCulture) + "," +
                    pn.zone.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("Convergence");
                writer.WriteLine(pn.convergenceAngle.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("StartFix");
                writer.WriteLine(pn.latitude.ToString(CultureInfo.InvariantCulture) + "," + pn.longitude.ToString(CultureInfo.InvariantCulture));
            }
        }

        public void FileCreateElevation()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //if (!isJobStarted)
            //{
            //    TimedMessageBox(3000, "Ooops, Job Not Started", "Start a Job First")
            //    return;
            //}

            string myFileName, dirField;

            //get the directory and make sure it exists, create if not
            dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            myFileName = "Elevation.txt";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));

                writer.WriteLine("$FieldDir");
                writer.WriteLine(currentFieldDirectory.ToString(CultureInfo.InvariantCulture));

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine(pn.utmEast.ToString(CultureInfo.InvariantCulture) + "," +
                    pn.utmNorth.ToString(CultureInfo.InvariantCulture) + "," +
                    pn.zone.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("Convergence");
                writer.WriteLine(pn.convergenceAngle.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("StartFix");
                writer.WriteLine(pn.latitude.ToString(CultureInfo.InvariantCulture) + "," + pn.longitude.ToString(CultureInfo.InvariantCulture));
            }
        }

        //save field Patches
        public void FileSaveSections()
        {
            //make sure there is something to save
            if (PatchSaveList.Count() > 0)
            {
                //Append the current list to the field file
                using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Sections.txt"), true))
                {
                    //for each patch, write out the list of triangles to the file
                    foreach (var triList in PatchSaveList)
                    {
                        int count2 = triList.Count();
                        writer.WriteLine(count2.ToString(CultureInfo.InvariantCulture));

                        for (int i = 0; i < count2; i++)
                            writer.WriteLine((Math.Round(triList[i].Easting, 3)).ToString(CultureInfo.InvariantCulture) +
                                "," + (Math.Round(triList[i].Northing, 3)).ToString(CultureInfo.InvariantCulture) +
                                 "," + (Math.Round(triList[i].Heading, 3)).ToString(CultureInfo.InvariantCulture));
                    }
                }

                //clear out that patchSaveList and begin adding new ones for next save
                PatchSaveList.Clear();
            }
        }

        //Create contour file
        public void FileCreateSections()
        {
            //$Sections
            //10 - points in this patch
            //10.1728031317344,0.723157039771303 -easting, northing

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Sections.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //write paths # of sections
                //writer.WriteLine("$Sectionsv4");
            }
        }

        //Create Flag file
        public void FileCreateFlags()
        {
            //$Sections
            //10 - points in this patch
            //10.1728031317344,0.723157039771303 -easting, northing

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Flags.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //write paths # of sections
                //writer.WriteLine("$Sectionsv4");
            }
        }

        //Create contour file
        public void FileCreateContour()
        {
            //12  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Contour.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                writer.WriteLine("$Contour");
            }
        }

        //save the contour points which include elevation values
        public void FileSaveContour()
        {
            //1  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude

            //make sure there is something to save
            if (ContourSaveList.Count() > 0)
            {
                //Append the current list to the field file
                using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Contour.txt"), true))
                {

                    //for every new chunk of patch in the whole section
                    foreach (var triList in ContourSaveList)
                    {
                        int count2 = triList.Count;

                        writer.WriteLine(count2.ToString(CultureInfo.InvariantCulture));

                        for (int i = 0; i < count2; i++)
                        {
                            writer.WriteLine(Math.Round((triList[i].Easting), 3).ToString(CultureInfo.InvariantCulture) + "," +
                                Math.Round(triList[i].Northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                Math.Round(triList[i].Heading, 3).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }

                ContourSaveList.Clear();

            }
        }

        //save the boundary
        public void FileSaveBoundary()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + "Boundary.Tmp"))
            {
                writer.WriteLine("$Boundary");
                for (int i = 0; i < bnd.bndArr.Count; i++)
                {
                    writer.WriteLine(bnd.bndArr[i].isDriveThru);
                    writer.WriteLine(bnd.bndArr[i].isDriveAround);

                    writer.WriteLine(bnd.bndArr[i].bndLine.Count.ToString(CultureInfo.InvariantCulture));
                    if (bnd.bndArr[i].bndLine.Count > 0)
                    {
                        for (int j = 0; j < bnd.bndArr[i].bndLine.Count; j++)
                        {
                            writer.WriteLine(Math.Round(bnd.bndArr[i].bndLine[j].Easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                Math.Round(bnd.bndArr[i].bndLine[j].Northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                    Math.Round(bnd.bndArr[i].bndLine[j].Heading, 5).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }
            }
            if (File.Exists(dirField + "Boundary.Txt"))
                File.Delete(dirField + "Boundary.Txt");
            File.Move(dirField + "Boundary.Tmp", dirField + "Boundary.Txt");
        }

        //save the headland
        public void FileSaveHeadland()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + "Headland.Tmp"))
            {
                writer.WriteLine("$Headland");

                writer.WriteLine(bnd.bndArr.Count.ToString(CultureInfo.InvariantCulture));
                for (int i = 0; i < bnd.bndArr.Count; i++)
                {
                    writer.WriteLine(bnd.bndArr[i].HeadLine.Count.ToString(CultureInfo.InvariantCulture));
                    for (int j = 0; j < bnd.bndArr[i].HeadLine.Count; j++)
                    {
                        writer.WriteLine(bnd.bndArr[i].HeadLine[j].Count.ToString(CultureInfo.InvariantCulture));
                        for (int k = 0; k < bnd.bndArr[i].HeadLine[j].Count; k++)
                        {
                            writer.WriteLine(Math.Round(bnd.bndArr[i].HeadLine[j][k].Easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                             Math.Round(bnd.bndArr[i].HeadLine[j][k].Northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                             Math.Round(bnd.bndArr[i].HeadLine[j][k].Heading, 3).ToString(CultureInfo.InvariantCulture));
                        }

                    }
                }
            }
            if (File.Exists(dirField + "Headland.Txt"))
                File.Delete(dirField + "Headland.Txt");
            File.Move(dirField + "Headland.Tmp", dirField + "Headland.Txt");
        }

        //Create contour file
        public void FileCreateRecPath()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "RecPath.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //write paths # of sections
                writer.WriteLine("$RecPath");
                writer.WriteLine("0");
            }
        }

        //save the recorded path
        public void FileSaveRecPath()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }



            if (!File.Exists(dirField + "RecPath.txt")) FileCreateRecPath();

            //write out the file
            using (StreamWriter writer = new StreamWriter((dirField + "RecPath.Tmp")))
            {
                writer.WriteLine("$RecPath");
                writer.WriteLine(recPath.recList.Count.ToString(CultureInfo.InvariantCulture));
                if (recPath.recList.Count > 0)
                {
                    for (int j = 0; j < recPath.recList.Count; j++)
                        writer.WriteLine(
                            Math.Round(recPath.recList[j].Easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(recPath.recList[j].Northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(recPath.recList[j].Heading, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(recPath.recList[j].Speed, 1).ToString(CultureInfo.InvariantCulture) + "," +
                            (recPath.recList[j].AutoBtnState).ToString());

                    //Clear list
                    //recPath.recList.Clear();
                }
            }
            if (File.Exists(dirField + "RecPath.Txt"))
                File.Delete(dirField + "RecPath.Txt");
            File.Move(dirField + "RecPath.Tmp", dirField + "RecPath.Txt");
        }

        //save all the flag markers
        public void FileSaveFlags()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FlagsDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //use Streamwriter to create and overwrite existing flag file
            using (StreamWriter writer = new StreamWriter(dirField + "Flags.Tmp"))
            {
                try
                {
                    writer.WriteLine("$Flags");

                    int count2 = flagPts.Count;
                    writer.WriteLine(count2);

                    for (int i = 0; i < count2; i++)
                    {
                        writer.WriteLine(
                            flagPts[i].Latitude.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].Longitude.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].Easting.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].Northing.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].Heading.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].color.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].ID.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].notes);
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\n Cannot write to file.");
                    WriteErrorLog("Saving Flags" + e.ToString());
                    return;
                }
            }
            if (File.Exists(dirField + "Flags.txt"))
                File.Delete(dirField + "Flags.txt");
            File.Move(dirField + "Flags.Tmp", dirField + "Flags.txt");
        }

        //save nmea sentences
        public void FileSaveNMEA()
        {
            using (StreamWriter writer = new StreamWriter((fieldsDirectory + "\\NMEA_log.txt"), true))
            {
                writer.Write(pn.logNMEASentence.ToString());
            }
            pn.logNMEASentence.Clear();
        }

        //save nmea sentences
        public void FileSaveElevation()
        {
            if (!File.Exists(fieldsDirectory + currentFieldDirectory + "\\Elevation.txt"))
                FileCreateElevation();

            using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Elevation.txt"), true))
            {
                writer.Write(sbFix.ToString());
            }
            sbFix.Clear();
        }

        //generate KML file from flag
        public void FileSaveSingleFlagKML2(int flagNumber)
        {
            double easting = flagPts[flagNumber - 1].Easting;
            double northing = flagPts[flagNumber - 1].Northing;

            double east = easting;
            double nort = northing;

            //fix the azimuth error
            easting = (Math.Cos(pn.convergenceAngle) * east) - (Math.Sin(pn.convergenceAngle) * nort);
            northing = (Math.Sin(pn.convergenceAngle) * east) + (Math.Cos(pn.convergenceAngle) * nort);

            easting += pn.utmEast;
            northing += pn.utmNorth;

            UTMToLatLon(easting, northing);

            double lat = utmLat;
            double lon = utmLon;


            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flag.kml";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //match new fix to current position


                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                if (flagPts[flagNumber - 1].color == 0)  //red - xbgr
                    writer.WriteLine(@"<color>ff4400ff</color>");
                if (flagPts[flagNumber - 1].color == 1)  //grn - xbgr
                    writer.WriteLine(@"<color>ff44ff00</color>");
                if (flagPts[flagNumber - 1].color == 2)  //yel - xbgr
                    writer.WriteLine(@"<color>ff44ffff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> " + flagNumber.ToString(CultureInfo.InvariantCulture) + @"</name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                lon.ToString(CultureInfo.InvariantCulture) + "," + lat.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }

        //generate KML file from flag
        public void FileSaveSingleFlagKML(int flagNumber)
        {

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flag.kml";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //match new fix to current position

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                if (flagPts[flagNumber - 1].color == 0)  //red - xbgr
                    writer.WriteLine(@"<color>ff4400ff</color>");
                if (flagPts[flagNumber - 1].color == 1)  //grn - xbgr
                    writer.WriteLine(@"<color>ff44ff00</color>");
                if (flagPts[flagNumber - 1].color == 2)  //yel - xbgr
                    writer.WriteLine(@"<color>ff44ffff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> " + flagNumber.ToString(CultureInfo.InvariantCulture) + @"</name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                flagPts[flagNumber - 1].Longitude.ToString(CultureInfo.InvariantCulture) + "," + flagPts[flagNumber - 1].Latitude.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }

        //generate KML file from flag
        public void FileMakeKMLFromCurrentPosition(double lat, double lon)
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            using (StreamWriter writer = new StreamWriter(dirField + "CurrentPosition.kml"))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                writer.WriteLine(@"<color>ff4400ff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> Your Current Position </name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                lon.ToString(CultureInfo.InvariantCulture) + "," + lat.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }

        //generate KML file from flags
        public void FileSaveFieldKML()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Field.Tmp";

            XmlTextWriter kml = new XmlTextWriter(dirField + myFileName, Encoding.UTF8);

            kml.Formatting = Formatting.Indented;
            kml.Indentation = 3;

            kml.WriteStartDocument();
            kml.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            kml.WriteStartElement("Document");

            //guidance lines AB
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "AB_Lines");
            kml.WriteElementString("visibility", "0");

            string linePts = "";

            for (int i = 0; i < ABLines.ABLines.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("visibility", "0");

                kml.WriteElementString("name", ABLines.ABLines[i].Name);
                kml.WriteStartElement("Style");

                kml.WriteStartElement("LineStyle");
                kml.WriteElementString("color", "ff0000ff");
                kml.WriteElementString("width", "2");
                kml.WriteEndElement(); // <LineStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("LineString");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("coordinates");

                linePts = GetUTMToLatLon(ABLines.ABLines[i].ref1.Easting, ABLines.ABLines[i].ref1.Northing);
                linePts += GetUTMToLatLon(ABLines.ABLines[i].ref2.Easting, ABLines.ABLines[i].ref2.Northing);
                kml.WriteRaw(linePts);

                kml.WriteEndElement(); // <coordinates>
                kml.WriteEndElement(); // <LineString>

                kml.WriteEndElement(); // <Placemark>

            }
            kml.WriteEndElement(); // <Folder>   

            //guidance lines Curve
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Curve_Lines");
            kml.WriteElementString("visibility", "0");

            for (int i = 0; i < CurveLines.Lines.Count; i++)
            {
                linePts = "";
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("visibility", "0");

                kml.WriteElementString("name", CurveLines.Lines[i].Name);
                kml.WriteStartElement("Style");

                kml.WriteStartElement("LineStyle");
                kml.WriteElementString("color", "ff6699ff");
                kml.WriteElementString("width", "2");
                kml.WriteEndElement(); // <LineStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("LineString");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("coordinates");

                for (int j = 0; j < CurveLines.Lines[i].curvePts.Count; j++)
                {
                    linePts += GetUTMToLatLon(CurveLines.Lines[i].curvePts[j].Easting, CurveLines.Lines[i].curvePts[j].Northing);
                }
                kml.WriteRaw(linePts);

                kml.WriteEndElement(); // <coordinates>
                kml.WriteEndElement(); // <LineString>

                kml.WriteEndElement(); // <Placemark>
            }
            kml.WriteEndElement(); // <Folder>   

            //flags  *************************************************************************
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Flags");

            for (int i = 0; i < flagPts.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("name", "Flag_" + i.ToString());

                kml.WriteStartElement("Style");
                kml.WriteStartElement("IconStyle");

                if (flagPts[i].color == 0)  //red - xbgr
                    kml.WriteElementString("color", "ff4400ff");
                if (flagPts[i].color == 1)  //grn - xbgr
                    kml.WriteElementString("color", "ff44ff00");
                if (flagPts[i].color == 2)  //yel - xbgr
                    kml.WriteElementString("color", "ff44ffff");

                kml.WriteEndElement(); //IconStyle
                kml.WriteEndElement(); //Style

                kml.WriteElementString("name", (i + 1).ToString());
                kml.WriteStartElement("Point");
                kml.WriteElementString("coordinates", flagPts[i].Longitude.ToString(CultureInfo.InvariantCulture) +
                    "," + flagPts[i].Latitude.ToString(CultureInfo.InvariantCulture) + ",0");
                kml.WriteEndElement(); //Point
                kml.WriteEndElement(); // <Placemark>
            }
            kml.WriteEndElement(); // <Folder>   
                                   //End of Flags

            //Boundary  ----------------------------------------------------------------------
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Boundaries");

            for (int i = 0; i < bnd.bndArr.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                if (i == 0) kml.WriteElementString("name", currentFieldDirectory);

                //lineStyle
                kml.WriteStartElement("Style");
                kml.WriteStartElement("LineStyle");
                if (i == 0) kml.WriteElementString("color", "ffdd00dd");
                else kml.WriteElementString("color", "ff4d3ffd");
                kml.WriteElementString("width", "4");
                kml.WriteEndElement(); // <LineStyle>

                kml.WriteStartElement("PolyStyle");
                if (i == 0) kml.WriteElementString("color", "407f3f55");
                else kml.WriteElementString("color", "703f38f1");
                kml.WriteEndElement(); // <PloyStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("Polygon");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("outerBoundaryIs");
                kml.WriteStartElement("LinearRing");

                //coords
                kml.WriteStartElement("coordinates");
                string bndPts = "";
                if (bnd.bndArr[i].bndLine.Count > 3)
                    bndPts = GetBoundaryPointsLatLon(i);
                kml.WriteRaw(bndPts);
                kml.WriteEndElement(); // <coordinates>

                kml.WriteEndElement(); // <Linear>
                kml.WriteEndElement(); // <OuterBoundary>
                kml.WriteEndElement(); // <Polygon>
                kml.WriteEndElement(); // <Placemark>
            }

            kml.WriteEndElement(); // <Folder>  
            //End of Boundary

            //Sections  ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Sections");

            string secPts = "";
            int cntr = 0;

            //for every new chunk of patch
            foreach (var triList in PatchDrawList)
            {
                if (triList.Count > 0)
                {
                    kml.WriteStartElement("Placemark");
                    kml.WriteElementString("name", "Sections_" + cntr.ToString());
                    cntr++;

                    string collor = "F0" + ((byte)(triList[0].Heading)).ToString("X2") +
                        ((byte)(triList[0].Northing)).ToString("X2") + ((byte)(triList[0].Easting)).ToString("X2");

                    //lineStyle
                    kml.WriteStartElement("Style");

                    kml.WriteStartElement("LineStyle");
                    kml.WriteElementString("color", collor);
                    //kml.WriteElementString("width", "6");
                    kml.WriteEndElement(); // <LineStyle>

                    kml.WriteStartElement("PolyStyle");
                    kml.WriteElementString("color", collor);
                    kml.WriteEndElement(); // <PloyStyle>
                    kml.WriteEndElement(); //Style

                    kml.WriteStartElement("Polygon");
                    kml.WriteElementString("tessellate", "1");
                    kml.WriteStartElement("outerBoundaryIs");
                    kml.WriteStartElement("LinearRing");

                    //coords
                    kml.WriteStartElement("coordinates");
                    secPts = "";
                    for (int k = 1; k < triList.Count; k += 2)
                    {
                        secPts += GetUTMToLatLon(triList[k].Easting, triList[k].Northing);
                    }
                    for (int k = triList.Count - 1; k > 1; k -= 2)
                    {
                        secPts += GetUTMToLatLon(triList[k].Easting, triList[k].Northing);
                    }
                    secPts += GetUTMToLatLon(triList[1].Easting, triList[1].Northing);
                    kml.WriteRaw(secPts);
                    kml.WriteEndElement(); // <coordinates>

                    kml.WriteEndElement(); // <LinearRing>
                    kml.WriteEndElement(); // <outerBoundaryIs>
                    kml.WriteEndElement(); // <Polygon>

                    kml.WriteEndElement(); // <Placemark>
                }
            }


            kml.WriteEndElement(); // <Folder>
            //End of sections

            //end of document
            kml.WriteEndElement(); // <Document>
            kml.WriteEndElement(); // <kml>

            //The end
            kml.WriteEndDocument();

            kml.Flush();

            //Write the XML to file and close the kml
            kml.Close();

            if (File.Exists(dirField + "Field.kml"))
                File.Delete(dirField + "Field.kml");
            File.Move(dirField + "Field.Tmp", dirField + "Field.kml");
        }

        public string GetUTMToLatLon(double easting, double northing)
        {
            double east = easting;
            double nort = northing;

            //fix the azimuth error
            easting = (Math.Cos(pn.convergenceAngle) * east) - (Math.Sin(pn.convergenceAngle) * nort);
            northing = (Math.Sin(pn.convergenceAngle) * east) + (Math.Cos(pn.convergenceAngle) * nort);

            easting += pn.utmEast;
            northing += pn.utmNorth;

            UTMToLatLon(easting, northing);

            return (utmLon.ToString("N7", CultureInfo.InvariantCulture) + ',' + utmLat.ToString("N7", CultureInfo.InvariantCulture) + ",0 ");
        }

        public string GetBoundaryPointsLatLon(int bndNum)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bnd.bndArr[bndNum].bndLine.Count; i++)
            {
                double easting = bnd.bndArr[bndNum].bndLine[i].Easting;
                double northing = bnd.bndArr[bndNum].bndLine[i].Northing;

                double east = easting;
                double nort = northing;

                //fix the azimuth error
                easting = (Math.Cos(pn.convergenceAngle) * east) - (Math.Sin(pn.convergenceAngle) * nort);
                northing = (Math.Sin(pn.convergenceAngle) * east) + (Math.Cos(pn.convergenceAngle) * nort);

                easting += pn.utmEast;
                northing += pn.utmNorth;

                UTMToLatLon(easting, northing);

                sb.Append(utmLon.ToString("N7", CultureInfo.InvariantCulture) + ',' + utmLat.ToString("N7", CultureInfo.InvariantCulture) + ",0 ");
            }
            return sb.ToString();
        }
    }
}