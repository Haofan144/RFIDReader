
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
            double dst=DistanceCalculate(phase1, phase2);
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


        static List<int> AppendTx(List<int> InspectTxPowerList, List<int> mrt)
        {
            foreach (int x in mrt)
            {
                InspectTxPowerList.Add(x);
            }
            return InspectTxPowerList;
        }


        



        //static void InspectStatusChange(List<double> TxRange,List<double> mrt93, List<double> mrt70, List<double> mrt96)
        


        

        // The following specifies which methods to call when tags are reported or operations are complete.
        // The TagsReported handler method will handle all new incoming tags
        static void OnTagsReported(ImpinjReader sender, TagReport report)
        {
            foreach (Tag tag in report)
            {
                double phaseAngle;
                phaseAngle = tag.PhaseAngleInRadians;

                DataRow row = DataDS.NewRow();
                //给列赋值
                DateTime dt = DateTime.Now;

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
                //把有值的列添加到表
                if (EPCStr.Contains("0093"))
                {
                    IncomingTagNumber93 = IncomingTagNumber93 + 1;
                    RSS93.Add(tag.PeakRssiInDbm.ToString());
                }
                else if (EPCStr.Contains("0012"))
                {
                    IncomingTagNumber70 = IncomingTagNumber70 + 1;
                    RSS70.Add(tag.PeakRssiInDbm.ToString());

                }
                else if (EPCStr.Contains("0078"))
                {
                    IncomingTagNumber96 = IncomingTagNumber96 + 1;
                    RSS96.Add(tag.PeakRssiInDbm.ToString());
                }
            
                else
                {

                }

                DataDS.Rows.Add(row);
                //DSForm.updateTaginfo(row);
                // DSForm.updateTaginfo((int)TagsEPC[tag.Epc.ToString()], (float)(tag.RfDopplerFrequency));
                DSForm.updateTaginfo((int)TagsEPC[tag.Epc.ToString()], (float)(tag.RfDopplerFrequency));//, tag.FirstSeenTime.LocalDateTime

                //DSForm.updateTaginfo((int)TagsEPC[tag.Epc.ToString()], (float)(tag.RfDopplerFrequency),(int)counter);
            }
        }




        





    }
}





