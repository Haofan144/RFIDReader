
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
using System.Net;
using System.Net.Sockets;


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

        public int threshold = 10;
       // public AddTag tag30 = new AddTag(0, new List<int>(),10);
        //static List<int> list = new List<int>();
        static int IncomingTagNumber30 = 0;
        static List<int> TagList30 = new List<int>();
        //static ArrayList TagList93 = new ArrayList();
        static int IncomingTagNumber37 = 0;
        static List<int> TagList37 = new List<int>();
        static int IncomingTagNumber59 = 0;
        static List<int> TagList59 = new List<int>();
        static int IncomingTagNumber25 = 0;
        static List<int> TagList25 = new List<int>();
        static int IncomingTagNumber44 = 0;
        static List<int> TagList44 = new List<int>();
        static int IncomingTagNumber51 = 0;
        static List<int> TagList51 = new List<int>();
        static int IncomingTagNumber16 = 0;
        static List<int> TagList16 = new List<int>();
        static int IncomingTagNumber60 = 0;
        static List<int> TagList60 = new List<int>();
        static int IncomingTagNumber41 = 0;
        static List<int> TagList41 = new List<int>();
        static int IncomingTagNumber47 = 0;
        static List<int> TagList47 = new List<int>();
        static int IncomingTagNumber46 = 0;
        static List<int> TagList46 = new List<int>();
        static int IncomingTagNumber68 = 0;
        static List<int> TagList68 = new List<int>();
        static int IncomingTagNumber35 = 0;
        static List<int> TagList35 = new List<int>();
        static int IncomingTagNumber01 = 0;
        static List<int> TagList01 = new List<int>();
        static int IncomingTagNumber17 = 0;
        static List<int> TagList17 = new List<int>();
        static int IncomingTagNumber40 = 0;
        static List<int> TagList40 = new List<int>();
        static int IncomingTagNumber45 = 0;
        static List<int> TagList45 = new List<int>();
        static int IncomingTagNumber48 = 0;
        static List<int> TagList48 = new List<int>();
        static int IncomingTagNumber54 = 0;
        static List<int> TagList54 = new List<int>();
        static int IncomingTagNumber53 = 0;
        static List<int> TagList53 = new List<int>();
        static int IncomingTagNumber52 = 0;
        static List<int> TagList52 = new List<int>();
        static int IncomingTagNumber58 = 0;
        static List<int> TagList58 = new List<int>();

        static void InitTagsEPC()
        {
            TagsEPC.Add("0000 0000 0000 0000 0000 0030", 1);
            TagsEPC.Add("0000 0000 0000 0000 0000 0037", 2);
            TagsEPC.Add("0000 0000 0000 0000 0000 0059", 3);
            TagsEPC.Add("0000 0000 0000 0000 0000 0025", 4);
            TagsEPC.Add("0000 0000 0000 0000 0000 0044", 5);
            TagsEPC.Add("0000 0000 0000 0000 0000 0051", 6);
            TagsEPC.Add("0000 0000 0000 0000 0000 0016", 7);
            TagsEPC.Add("0000 0000 0000 0000 0000 0041", 8);
            TagsEPC.Add("0000 0000 0000 0000 0000 0060", 9);
            TagsEPC.Add("0000 0000 0000 0000 0000 0046", 10);
            TagsEPC.Add("0000 0000 0000 0000 0000 0047", 11);
            TagsEPC.Add("0000 0000 0000 0000 0000 0068", 12);
            TagsEPC.Add("0000 0000 0000 0000 0000 0035", 13);
            TagsEPC.Add("0000 0000 0000 0000 0000 0001", 14);
            TagsEPC.Add("0000 0000 0000 0000 0000 0017", 15);
            TagsEPC.Add("0000 0000 0000 0000 0000 0040", 16);
            TagsEPC.Add("0000 0000 0000 0000 0000 0045", 17);
            TagsEPC.Add("0000 0000 0000 0000 0000 0048", 18);
            TagsEPC.Add("0000 0000 0000 0000 0000 0054", 19);
            TagsEPC.Add("0000 0000 0000 0000 0000 0053", 20);
            TagsEPC.Add("0000 0000 0000 0000 0000 0052", 21);
            TagsEPC.Add("0000 0000 0000 0000 0000 0058", 22);


            TagNames.Add("0000 0000 0000 0000 0000 0030");
            TagNames.Add("0000 0000 0000 0000 0000 0037");
            TagNames.Add("0000 0000 0000 0000 0000 0059");
            TagNames.Add("0000 0000 0000 0000 0000 0025");
            TagNames.Add("0000 0000 0000 0000 0000 0044");
            TagNames.Add("0000 0000 0000 0000 0000 0051");
            TagNames.Add("0000 0000 0000 0000 0000 0016");
            TagNames.Add("0000 0000 0000 0000 0000 0041");
            TagNames.Add("0000 0000 0000 0000 0000 0060");
            TagNames.Add("0000 0000 0000 0000 0000 0046");
            TagNames.Add("0000 0000 0000 0000 0000 0047");
            TagNames.Add("0000 0000 0000 0000 0000 0068");
            TagNames.Add("0000 0000 0000 0000 0000 0035");
            TagNames.Add("0000 0000 0000 0000 0000 0001");
            TagNames.Add("0000 0000 0000 0000 0000 0017");
            TagNames.Add("0000 0000 0000 0000 0000 0040");
            TagNames.Add("0000 0000 0000 0000 0000 0045");
            TagNames.Add("0000 0000 0000 0000 0000 0048");
            TagNames.Add("0000 0000 0000 0000 0000 0053");
            TagNames.Add("0000 0000 0000 0000 0000 0054");
            TagNames.Add("0000 0000 0000 0000 0000 0052");
            TagNames.Add("0000 0000 0000 0000 0000 0058");




        }


        static void Main(string[] args)
        {
            //Determine the MRT at the first time
            //Client client = new Client();
            //client.Start();
            // AddTag tag30 = new AddTag(0, new List<int>(), 10);
             DetermineMRT();
            //DetermineMRT(client);
            //Check the Status Change 
            
            //InspectStatusChange(TxRange);
        }



        static void DetermineMRT()
 //static void DetermineMRT(Client client)
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
                AddTag tag30 = new AddTag(0);
                //tag30.Test();
                while (true)
                {
                    //for (double i = 32; i > 28; i = i - 0.25)
                    //{
                        int i = 32;
                        //double i = 29.75;
                        PowerRange.Add(i);
                   
                        IncomingTagNumber30 = 0;
                        IncomingTagNumber37 = 0;
                    IncomingTagNumber59 = 0;
                    IncomingTagNumber25 = 0;
                    IncomingTagNumber44 = 0;
                    IncomingTagNumber51 = 0;
                    IncomingTagNumber16 = 0;
                    IncomingTagNumber41 = 0;
                    IncomingTagNumber60 = 0;
                    IncomingTagNumber46 = 0;
                    IncomingTagNumber47 = 0;
                    IncomingTagNumber68 = 0;
                    IncomingTagNumber35 = 0;
                    IncomingTagNumber01 = 0;
                    IncomingTagNumber17 = 0;
                    IncomingTagNumber40 = 0;
                    IncomingTagNumber45 = 0;
                    IncomingTagNumber48 = 0;
                    IncomingTagNumber54 = 0;
                    IncomingTagNumber53 = 0;
                    IncomingTagNumber52 = 0;
                    IncomingTagNumber58 = 0;
                    SpecifiedTx = i;

                        settings.Antennas.GetAntenna(1).TxPowerInDbm = SpecifiedTx;
                        reader.ApplySettings(settings);
                        reader.Start();
                        Thread.Sleep(1000);//收集5s数据？
                        reader.Stop();
                        System.Console.WriteLine("Current Tx power is:" + settings.Antennas.GetAntenna(1).TxPowerInDbm);
                        System.Console.WriteLine("IncomingTagNumber30 is:" + IncomingTagNumber30);
                        TagList30.Add(IncomingTagNumber30);
                    tag30.IncomingTagNumber = IncomingTagNumber30;
                 

                        System.Console.WriteLine("IncomingTagNumber37 is:" + IncomingTagNumber37);
                        TagList37.Add(IncomingTagNumber37);
                        System.Console.WriteLine("IncomingTagNumber59 is:" + IncomingTagNumber59);
                        TagList59.Add(IncomingTagNumber59);
                        System.Console.WriteLine("IncomingTagNumber25 is:" + IncomingTagNumber25);
                        TagList25.Add(IncomingTagNumber25);
                        System.Console.WriteLine("IncomingTagNumber44 is:" + IncomingTagNumber44);
                        TagList44.Add(IncomingTagNumber44);
                        System.Console.WriteLine("IncomingTagNumber51 is:" + IncomingTagNumber51);
                        TagList51.Add(IncomingTagNumber51);
                    System.Console.WriteLine("IncomingTagNumber16 is:" + IncomingTagNumber16);
                    TagList16.Add(IncomingTagNumber16);
                    System.Console.WriteLine("IncomingTagNumber41 is:" + IncomingTagNumber41);
                    TagList41.Add(IncomingTagNumber41);
                    System.Console.WriteLine("IncomingTagNumber60 is:" + IncomingTagNumber60);
                    TagList60.Add(IncomingTagNumber60);
                    System.Console.WriteLine("IncomingTagNumber46 is:" + IncomingTagNumber46);
                    TagList46.Add(IncomingTagNumber46);
                    System.Console.WriteLine("IncomingTagNumber47 is:" + IncomingTagNumber47);
                    TagList47.Add(IncomingTagNumber47);
                    System.Console.WriteLine("IncomingTagNumber68 is:" + IncomingTagNumber68);
                    TagList68.Add(IncomingTagNumber68);
                    System.Console.WriteLine("IncomingTagNumber35 is:" + IncomingTagNumber35);
                    TagList35.Add(IncomingTagNumber35);
                    System.Console.WriteLine("IncomingTagNumber01 is:" + IncomingTagNumber01);
                    TagList01.Add(IncomingTagNumber01);
                    System.Console.WriteLine("IncomingTagNumber17 is:" + IncomingTagNumber17);
                    TagList17.Add(IncomingTagNumber17);
                    System.Console.WriteLine("IncomingTagNumber40 is:" + IncomingTagNumber40);
                    TagList40.Add(IncomingTagNumber40);
                    System.Console.WriteLine("IncomingTagNumber45 is:" + IncomingTagNumber45);
                    TagList45.Add(IncomingTagNumber45);
                    System.Console.WriteLine("IncomingTagNumber48 is:" + IncomingTagNumber48);
                    TagList48.Add(IncomingTagNumber48);
                    System.Console.WriteLine("IncomingTagNumber54 is:" + IncomingTagNumber54);
                    TagList54.Add(IncomingTagNumber54);
                    System.Console.WriteLine("IncomingTagNumber53 is:" + IncomingTagNumber53);
                    TagList53.Add(IncomingTagNumber53);
                    System.Console.WriteLine("IncomingTagNumber52 is:" + IncomingTagNumber52);
                    TagList52.Add(IncomingTagNumber52);
                    System.Console.WriteLine("IncomingTagNumber58 is:" + IncomingTagNumber58);
                    TagList58.Add(IncomingTagNumber58);
                    //client.ReadInput(IncomingTagNumber70);
                    //    client.Send();



                    //}

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
                if (EPCStr.Contains("0030"))
                {
                    IncomingTagNumber30 = IncomingTagNumber30 + 1;
                }
                else if (EPCStr.Contains("0037"))
                {
                    IncomingTagNumber37 = IncomingTagNumber37 + 1;
                }
                else if (EPCStr.Contains("0059"))
                {
                    IncomingTagNumber59 = IncomingTagNumber59 + 1;
                }
                else if (EPCStr.Contains("0025"))
                {
                    IncomingTagNumber25 = IncomingTagNumber25 + 1;
                }
                else if (EPCStr.Contains("0044"))
                {
                    IncomingTagNumber44 = IncomingTagNumber44 + 1;
                }
                else if (EPCStr.Contains("0051"))
                {
                    IncomingTagNumber51 = IncomingTagNumber51 + 1;
                }
                else if (EPCStr.Contains("0016"))
                {
                    IncomingTagNumber16 = IncomingTagNumber16 + 1;
                }
                else if (EPCStr.Contains("0041"))
                {
                    IncomingTagNumber41 = IncomingTagNumber41 + 1;
                }
                else if (EPCStr.Contains("0060"))
                {
                    IncomingTagNumber60 = IncomingTagNumber60 + 1;
                }
                else if (EPCStr.Contains("0046"))
                {
                    IncomingTagNumber46 = IncomingTagNumber46 + 1;
                }
                else if (EPCStr.Contains("0047"))
                {
                    IncomingTagNumber47 = IncomingTagNumber47 + 1;
                }
                else if (EPCStr.Contains("0068"))
                {
                    IncomingTagNumber68 = IncomingTagNumber68 + 1;
                }
                else if (EPCStr.Contains("0035"))
                {
                    IncomingTagNumber35 = IncomingTagNumber35 + 1;
                }
                else if (EPCStr.Contains("0001"))
                {
                    IncomingTagNumber01 = IncomingTagNumber01+ 1;
                }
                else if (EPCStr.Contains("0017"))
                {
                    IncomingTagNumber17 = IncomingTagNumber17 + 1;
                }
                else if (EPCStr.Contains("0040"))
                {
                    IncomingTagNumber40 = IncomingTagNumber40 + 1;
                }
                else if (EPCStr.Contains("0045"))
                {
                    IncomingTagNumber45 = IncomingTagNumber45 + 1;
                }
                else if (EPCStr.Contains("0048"))
                {
                    IncomingTagNumber48 = IncomingTagNumber48 + 1;
                }

                else if (EPCStr.Contains("0054"))
                {
                    IncomingTagNumber54 = IncomingTagNumber54 + 1;
                }
                else if (EPCStr.Contains("0053"))
                {
                    IncomingTagNumber53 = IncomingTagNumber53 + 1;
                }
                else if (EPCStr.Contains("0052"))
                {
                    IncomingTagNumber52 = IncomingTagNumber52 + 1;
                }
                else if (EPCStr.Contains("0058"))
                {
                    IncomingTagNumber58 = IncomingTagNumber58 + 1;
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





