﻿
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
        // Create an instance of the ImpinjReader class.
        static ImpinjReader reader = new ImpinjReader();
        //新建一个dataTable
        static DataTable DataDS = new DataTable();

        //static string filePath = "C:\\Users\\qla\b\\Desktop\\";
        static string filePath = "C:\\Users\\haofa\\OneDrive\\Documents\\Taggesture\\testdata\\";

        // C:\Users\qlab\Desktop\close\5tags\walkb
        //static string fileName = "id1086_1.csv"4
        static string fileName = "test_new_loop_93_21" +
          
            "" + ".csv";

        static CsvStreamWriter CsvWriter = new CsvStreamWriter(filePath + fileName);
        static double txPowerValue = 0;
        static ushort antennaPort = 1;
        static FormRealTime DSForm;
        static Hashtable TagsEPC = new Hashtable();
        static List<String> TagNames = new List<String>();

        static double SpecifiedTx = 30;

        //static List<int> list = new List<int>();
        static int IncomingTagNumber93 = 0;
        static List<int> TagList93 = new List<int>();
        //static ArrayList TagList93 = new ArrayList();
        static int IncomingTagNumber70 = 0;
        static List<int> TagList70 = new List<int>();
        static int IncomingTagNumber96 = 0;
        static List<int> TagList96 = new List<int>();
        static int IncomingTagNumber73 = 0;
        static List<int> TagList73 = new List<int>();

        static int IncomingPieceNumber74 = 0;
        static List<int> PieceList74 = new List<int>();
        static int IncomingPieceNumber75 = 0;
        static List<int> PieceList75 = new List<int>();


        static void InitTagsEPC()
        {
            TagsEPC.Add("0000 0000 0000 0000 0000 0093", 1);
            TagsEPC.Add("0000 0000 0000 0000 0000 0096", 2);
            TagsEPC.Add("0000 0000 0000 0000 0000 0070", 3);
            TagsEPC.Add("0000 0000 0000 0000 0000 0073", 4);
            TagsEPC.Add("0000 0000 0000 0000 0000 0074", 5);
            TagsEPC.Add("0000 0000 0000 0000 0000 0075", 6);
            //TagNames.Add("0001");


            TagNames.Add("0000 0000 0000 0000 0000 0093");
            TagNames.Add("0000 0000 0000 0000 0000 0096");
            TagNames.Add("0000 0000 0000 0000 0000 0070");
            TagNames.Add("0000 0000 0000 0000 0000 0073");
            TagNames.Add("0000 0000 0000 0000 0000 0074");
            TagNames.Add("0000 0000 0000 0000 0000 0075");

        }


        static void Main(string[] args)
        {
            //Determine the MRT at the first time
            DetermineMRT();
            //Check the Status Change 
            
            //InspectStatusChange(TxRange);
        }



        static void DetermineMRT()
        {

            try
            {
                System.Console.WriteLine("Before------------!!!");
                initDataRow();
                InitTagsEPC();
                System.Console.WriteLine("Before Connect!!!");
                reader.Connect(SolutionConstants.ReaderHostname);
                System.Console.WriteLine("Connect!!!");
                FeatureSet features = reader.QueryFeatureSet();
                Settings settings = reader.QueryDefaultSettings();
                reportSetting(settings);
                settings.Antennas.DisableAll();
                settings.Antennas.GetAntenna(1).IsEnabled = true;
                settings.Antennas.GetAntenna(2).IsEnabled = true;

                SpecifiedTx = 30;
                settings.Antennas.GetAntenna(1).TxPowerInDbm = SpecifiedTx;
                settings.SearchMode = SearchMode.DualTarget;
                settings.Session = 2;
                settings.ReaderMode = ReaderMode.MaxThroughput;//编码方式
                System.Console.WriteLine("1!");
                settings.Report.Mode = ReportMode.Individual;
                filterTags(settings);
                if (0 != fixFrequency(features, settings))
                {
                    applicactionClose();
                }

                System.Console.WriteLine("3!");
                reader.ApplySettings(settings);
                reader.TagsReported += OnTagsReported;
                List<double> PowerRange = new List<double>();
                while (true)
                {
                    for (double i = 32; i > 28; i = i - 0.25)
                    {
                        //double i = 29.75;
                        PowerRange.Add(i);
                        IncomingTagNumber93 = 0;
                        IncomingTagNumber70 = 0;
                        IncomingTagNumber96 = 0;
                        IncomingTagNumber73 = 0;
                        IncomingPieceNumber74 = 0;
                        IncomingPieceNumber75 = 0;

                        SpecifiedTx = i;

                        settings.Antennas.GetAntenna(1).TxPowerInDbm = SpecifiedTx;
                        reader.ApplySettings(settings);
                        reader.Start();
                        Thread.Sleep(900);//收集5s数据？
                        reader.Stop();
                        System.Console.WriteLine("Current Tx power is:" + settings.Antennas.GetAntenna(1).TxPowerInDbm);
                        System.Console.WriteLine("IncomingTagNumber93 is:" + IncomingTagNumber93);
                        TagList93.Add(IncomingTagNumber93);
                        System.Console.WriteLine("IncomingTagNumber70 is:" + IncomingTagNumber70);
                        TagList70.Add(IncomingTagNumber70);
                        System.Console.WriteLine("IncomingTagNumber96 is:" + IncomingTagNumber96);
                        TagList96.Add(IncomingTagNumber96);
                        //System.Console.WriteLine("IncomingTagNumber73 is:" + IncomingTagNumber73);
                        //TagList73.Add(IncomingTagNumber73);
                        //System.Console.WriteLine("IncomingPieceNumber74 is:" + IncomingPieceNumber74);
                        //PieceList74.Add(IncomingPieceNumber74);
                        //System.Console.WriteLine("IncomingPieceNumber75 is:" + IncomingPieceNumber75);
                        //PieceList75.Add(IncomingPieceNumber75);

                    }

                }
              
                


            }
            catch (OctaneSdkException e)
            {
                Console.WriteLine("Octane SDK exception: {0}", e.Message);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : {0}", e.Message);
                //return null;
            }
           
        }




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
                }
                else if (EPCStr.Contains("0012"))
                {
                    IncomingTagNumber70 = IncomingTagNumber70 + 1;
                }
                else if (EPCStr.Contains("0078"))
                {
                    IncomingTagNumber96 = IncomingTagNumber96 + 1;
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




        static public void initDataRow()
        {
            // Columns initializations
            DataDS.Columns.Add("EPC");
            DataDS.Columns.Add("Doppler Shift");
            DataDS.Columns.Add("Time");
            DataDS.Columns.Add("Antenna");
            DataDS.Columns.Add("Tx Power");
            DataDS.Columns.Add("Current Frequency");
            DataDS.Columns.Add("PeakRSSI");
            DataDS.Columns.Add("Phase Angle");
            DataDS.Columns.Add("Phase");
            DataDS.Columns.Add("Counter");
            // Rows initializations

            DataRow row = DataDS.NewRow();
            row["EPC"] = "EPC";
            row["Doppler Shift"] = "DopplerShift(Hz)";
            row["Time"] = "Time";
            row["Antenna"] = "Antenna";
            row["Tx Power"] = "TxPower";
            row["Current Frequency"] = "Frequency(MHz)";
            row["PeakRSSI"] = "RSS(dbm)";
            row["Phase Angle"] = "PhaseAngle(Radian)";
            row["Phase"] = "PhaseAngle(Degree)";
            //row["Counter"]="Counter";
            DataDS.Rows.Add(row);

        }

        static public void reportSetting(Settings settings)
        {
            // Tell the reader to include the
            // RF doppler frequency in all tag reports.
            settings.Report.IncludeDopplerFrequency = true;

            // 允许输出
            settings.Report.IncludeChannel = true;
            settings.Report.IncludePeakRssi = true;
            settings.Report.IncludePhaseAngle = true;
            settings.Report.IncludeFirstSeenTime = true;
            settings.Report.IncludeAntennaPortNumber = true;
        }

        static public void applicactionClose()
        {
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
            // 写CSV文件
            CsvWriter.AddData(DataDS, 1);
            CsvWriter.Save();
            reader.Disconnect();
            Console.WriteLine(DataDS.Rows.Count);
            Console.ReadLine();

        }

        static public void filterTags(Settings settings)
        {
            settings.Filters.TagFilter1.MemoryBank = MemoryBank.Epc;
            settings.Filters.TagFilter1.BitPointer = BitPointers.Epc;
            settings.Filters.TagFilter1.TagMask = "0000 0000 0000 0000 0000";
            settings.Filters.TagFilter1.BitCount = 8;
        }

        static public int fixFrequency(FeatureSet features, Settings settings)
        {
            List<double> freqList = new List<double>();

            if (!features.IsHoppingRegion)
            {
                //freqList.Add(921.625);
                //freqList.Add(921.875);
                //freqList.Add(922.125);
                //freqList.Add(921.625 );
                freqList.Add(924.375);
                // 其他符合标准的频率值
                // 921.625;921.875;922.125;922.375;922.625;922.875;923.125;923.375;923.625;923.875;924.125;924.375;
                settings.TxFrequenciesInMhz = freqList;
                return 0;
            }
            else
            {
                Console.WriteLine("This reader does not allow the transmit frequencies to be specified.");
                return -1;
            }
        }





    }
}




