
////////////////////////////////////////////////////////////////////////////////
//
//    Tag Gesture
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.IO;
using System.Data;
using Impinj.OctaneSdk;
using System.Collections.Generic;
using System.Threading;
using DopplerVisualization;
using System.Windows.Forms;



namespace TagGesture
{
    class Program
    {
        static List<string> EPCList = new List<string>();
        static List<double> PhaseList = new List<double>();
        static List<string> TimeList = new List<string>();

        //Define Square Class
        public class Square
        {
            private string epc;
            private List<string> timeslot = new List<string>();
            private List<double> phase = new List<double>();
            public Sqaure(string epc, List<double> phase, List<string> timeslot)
            {
                this.epc = epc;
                this.phase = phase;
                this.timeslot = timeslot;
            }
        }
        //Define Piece Class
        public class Piece
        {
            private string epc;
            private List<string> timeslot = new List<string>();
            private List<double> phase = new List<double>();
            public Piece(string epc, List<double> phase, List<string> timeslot)
            {
                this.epc = epc;
                this.phase = phase;
                this.timeslot = timeslot;
            }
        }


        private static int Add(int x, int y)
        {
            return x + y;
        }
        public delegate int MethodDelegate(int x, int y);
        private static MethodDelegate method;
        method=new MethodDelegate(Add);

        //Calculate Euclidean Distance
        static double DistanceCalculate(double[] phase1, double[] phase2)
        {
            var distanceArray = new double[phase1.Length, phase2.Length];

            for (int i = 0; i < phase1.Length; i++)
                for (int j = 0; j < phase2.Length; j++)
                    distanceArray[i, j] = Distance(phase1[i, 0], phase1[i, 1], phase2[j, 0], phase2[j, 1]);

            double dst = distanceArray.Average();
            return dst;
        }


        //Euclidean Distance
        double Distance(double x1, double y1, double x2, double y2)
        => Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));

        //
        static double[] PhaseComparison(double[] phase1, double[] phase2)
        {
            if (phase1.Length != phase2.Length)
            {
                throw new Exception("Arrays must be equal length");
            }
            double dst = DistanceCalculate(phase1, phase2);
        }


        static double[] Deperiodicity(double[] phase)
        {
            for (int i = 0; i < phase.Length; i++)
            {
                if (phase[i] - phase[i - 1] > Math.PI)
                    phase[i] = phase[i] - Math.PI;
                else if (phase[i] - phase[i - 1] < -Math.PI)
                    phase[i] = phase[i - 1] + Math.PI;

            }
            return phase;
        }


        public class Observer
        {
            public void IsTagComing();
            System.Console.WriteLine("There is a tag coming");
        }

        // The following specifies which methods to call when tags are reported or operations are complete.
        // The TagsReported handler method will handle all new incoming tags
        static void OnTagsReported(ImpinjReader sender, TagReport report)
        {
            foreach (Tag tag in report)
            {
                double phaseAngle;
                phaseAngle = tag.PhaseAngleInRadians;

                //DataRow row = DataDS.NewRow();
                ////给列赋值
                //DateTime dt = DateTime.Now;

                row["EPC"] = tag.Epc.ToString();
                row["Doppler Shift"] = tag.RfDopplerFrequency.ToString("0.00");
                row["Time"] = tag.FirstSeenTime.ToString();
                row["Antenna"] = tag.AntennaPortNumber;
                row["Tx Power"] = txPowerValue;
                row["Current Frequency"] = tag.ChannelInMhz.ToString();
                row["PeakRSSI"] = tag.PeakRssiInDbm.ToString();
                row["Phase Angle"] = phaseAngle;//tag.PhaseAngleInRadians.ToString();
                row["Phase"] = ((tag.PhaseAngleInRadians) / Math.PI) * 180;


                string EPCStr = tag.Epc.ToString();
                EPCList.Add(tag.Epc.ToString());
                PhaseList.Add(phaseAngle);
                TimeList.Add(tag.FirstSeenTime.ToString());


                string EPCStr = tag.Epc.ToString();
                //把有值的列添加到表
                if (EPCStr.Contains("0093"))
                {
                    IncomingTagNumber93 = IncomingTagNumber93 + 1;
                }
                else if (EPCStr.Contains("0012"))
                {
                    IncomingTagNumber70 = IncomingTagNumber70 + 1;
                }
                else if (EPCStr.Contains("0078"))
                {
                    IncomingTagNumber96 = IncomingTagNumber96 + 1;
                }
                //把有值的列添加到表
                //if (EPCStr.Contains("0093"))
                //{
                //    IncomingTagNumber93 = IncomingTagNumber93 + 1;
                //    RSS93.Add(tag.PeakRssiInDbm.ToString());
                //}
                //else
                //{

                //}


                //DataDS.Rows.Add(row);
                //DSForm.updateTaginfo((int)TagsEPC[tag.Epc.ToString()], (float)(tag.RfDopplerFrequency));//, tag.FirstSeenTime.LocalDateTime

            }
        }










    }
}





