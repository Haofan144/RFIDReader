
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
            "" +
            "" +
            "" +
            "" +
            "" +
            "" +
            "" +
            "" +
            "" + ".csv";

        static CsvStreamWriter CsvWriter = new CsvStreamWriter(filePath + fileName);
        static double txPowerValue = 0;
        static ushort antennaPort = 1;
        static FormRealTime DSForm;
        static Hashtable TagsEPC = new Hashtable();
        static List<String> TagNames = new List<String>();

        static ArrayList TagList = new ArrayList();

        static double SpecifiedTx = 30;


        //static int[] TagCounter = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        static int PreviousCounter;

        static int intIndex;

        //three MRT for tag 93,87,96
        static double mrt93, mrt87, mrt96;

        static int IncomingTagNumber93 =0;
        static int IncomingTagNumber87 = 0;
        static int IncomingTagNumber96 = 0;


        static void InitTagsEPC()
        {
            TagsEPC.Add("0000 0000 0000 0000 0000 0093", 1);
            TagsEPC.Add("0000 0000 0000 0000 0000 0096", 2);
            TagsEPC.Add("0000 0000 0000 0000 0000 0087", 3);
            //TagNames.Add("0001");
            //TagNames.Add("0002");
            //TagNames.Add("0003");
            //TagNames.Add("0004");
            //TagNames.Add("0005");
           
            TagNames.Add("0000 0000 0000 0000 0000 0093");
            TagNames.Add("0000 0000 0000 0000 0000 0096");
            TagNames.Add("0000 0000 0000 0000 0000 0087");

    

        }


        static void Main(string[] args)
        {
            try
            {
                System.Console.WriteLine("Before------------!!!");
                initDataRow();
                InitTagsEPC();

                // Connect to the reader.
                // Change the ReaderHostname constant in SolutionConstants.cs
                // to the IP address or hostname of your reader.
                System.Console.WriteLine("Before Connect!!!");
                reader.Connect(SolutionConstants.ReaderHostname);
                System.Console.WriteLine("Connect!!!");
                // 获取当前默认设置
                // Get the reader features to determine if the
                // reader supports a fixed-frequency table.
                FeatureSet features = reader.QueryFeatureSet();

                // Get the default settings
                // We'll use these as a starting point
                // and then modify the settings we're
                // interested in.
                Settings settings = reader.QueryDefaultSettings();
                reportSetting(settings);

                // Use antenna #2

                settings.Antennas.DisableAll();
                settings.Antennas.GetAntenna(1).IsEnabled = true;
                settings.Antennas.GetAntenna(2).IsEnabled = true;
                //settings.Antennas.EnableAll();
                //for (double i = 10; i < 32; i = i + 0.25)
                //{
                SpecifiedTx = 30;
                settings.Antennas.GetAntenna(1).TxPowerInDbm = SpecifiedTx;
                

                //settings.Antennas.GetAntenna(1).RxSensitivityInDbm = -55;
                //settings.Antennas.GetAntenna(2).TxPowerInDbm = 32;
                // settings.Antennas.GetAntenna(2).RxSensitivityInDbm = -55;


                //settings.ReaderMode = ReaderMode.AutoSetDenseReader;
                settings.SearchMode = SearchMode.DualTarget;
                settings.Session = 2;
                // ReaderMode must be set to DenseReaderM4 or DenseReaderM8.
                settings.ReaderMode = ReaderMode.MaxThroughput;//编码方式
                System.Console.WriteLine("1!");
                // 每读取一个tag就report
                // Send a tag report for every tag read.
                settings.Report.Mode = ReportMode.Individual;

                filterTags(settings);
                // System.Console.WriteLine("4!");
                if (0 != fixFrequency(features, settings))
                {
                    applicactionClose();
                }

                // Apply the newly modified settings.
                System.Console.WriteLine("3!");
                reader.ApplySettings(settings);
                System.Console.WriteLine("5!");
                // Assign the TagsReported event handler.
                // This specifies which method to call
                // when tags reports are available.
                reader.TagsReported += OnTagsReported;
                //reader.TagsReported += OnTagsReported2;//Original EventHandler
                // Read with fix tx power
                singleRead();
                //singleRead2();//Original singleReader 

                // multiple tx power read
                // txPowerRead(features, settings);

            }
            catch (OctaneSdkException e)
            {
                // Handle Octane SDK errors.
                Console.WriteLine("Octane SDK exception: {0}", e.Message);
            }
            catch (Exception e)
            {
                // Handle other .NET errors.
                Console.WriteLine("Exception : {0}", e.Message);
            }
        }

        static void singleRead()
        {
            Settings settings = reader.QueryDefaultSettings();
            reader.Start();
            //DSForm = new FormRealTime(TagNames);
            //Application.Run(DSForm);

            Thread.Sleep(5000);
            reader.Stop();
            System.Console.WriteLine("IncomingTagNumber93 is:" + IncomingTagNumber93);
            System.Console.WriteLine("IncomingTagNumber87 is:" + IncomingTagNumber87);
            System.Console.WriteLine("IncomingTagNumber96 is:" + IncomingTagNumber96);

            while (true)
            {
                //for (double i = 30; i > 29.75; i = i - 0.25)
                //{
                    double i = 29.75;
                    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    //sw.Start();
                    IncomingTagNumber93 = 0;
                    IncomingTagNumber87 = 0;
                    IncomingTagNumber96 = 0;
                    SpecifiedTx = i;
                    settings.Antennas.GetAntenna(1).TxPowerInDbm = SpecifiedTx;
                    reportSetting(settings);
                    settings.Antennas.DisableAll();
                    settings.Antennas.GetAntenna(1).IsEnabled = true;
                    settings.SearchMode = SearchMode.DualTarget;
                    settings.Session = 2;
                    settings.ReaderMode = ReaderMode.MaxThroughput;
                    settings.Report.Mode = ReportMode.Individual;
                    filterTags(settings);

                    //
                    reader.ApplySettings(settings);
                    reader.Start();
                    //AddRedundancy();//indicate the start




                    //
                    //reader.ApplySettings(settings);
                    //reader.Start();
                    Thread.Sleep(3000);//收集5s数据？
                    System.Console.WriteLine("Current Tx power is:" + settings.Antennas.GetAntenna(1).TxPowerInDbm);
                    reader.Stop();
                    System.Console.WriteLine("IncomingTagNumber93 is:" + IncomingTagNumber93);
                    System.Console.WriteLine("IncomingTagNumber87 is:" + IncomingTagNumber87);
                    System.Console.WriteLine("IncomingTagNumber96 is:" + IncomingTagNumber96);
                //}
            }


            applicactionClose();
        }

        static void singleRead2()
        {

            reader.Start();
            DSForm = new FormRealTime(TagNames);
            Application.Run(DSForm);
            // Start reading.

            //Thread.Sleep(31000); //收集31s数据
            // Wait for the user to press enter.
            //Console.WriteLine("Press enter to exit.");
            //Console.ReadLine();

            // Stop reading.
            reader.Stop();

            applicactionClose();
        }

        static void AddRedundancy()
        {
            DataRow row = DataDS.NewRow();
            DateTime dt = DateTime.Now;
            row["EPC"] = "0000";
            row["Doppler Shift"] = "0.00";
            row["Time"] = "123";
            row["Antenna"] = 1;
            row["Tx Power"] = 22;
            row["Current Frequency"] = "123";
            row["PeakRSSI"] = "123";
            row["Phase Angle"] = 0;//tag.PhaseAngleInRadians.ToString();
            row["Phase"] = ((2) / Math.PI) * 180;
            DataDS.Rows.Add(row);
        }

        // The following specifies which methods to call when tags are reported or operations are complete.
        // The TagsReported handler method will handle all new incoming tags
        static void OnTagsReported(ImpinjReader sender, TagReport report)
        {
            // This event handler is called asynchronously
            // when tag reports are available.
            // Loop through each tag in the report
            // and print the data.
            //System.Console.WriteLine("TagReport is:" + report);
            foreach (Tag tag in report)
            {
                //Console.WriteLine("EPC : {0} Doppler Frequency (Hz) : {1} Current Frequecy: {2}  PeakRSSI ：{3}  PhaseAngle : {4} PhaseDegree :{5}",
                //                    tag.Epc, tag.RfDopplerFrequency.ToString("0.00"), tag.ChannelInMhz, tag.PeakRssiInDbm, tag.PhaseAngleInRadians, ((tag.PhaseAngleInRadians) / Math.PI) * 180);

                //if (stock.Contains(tag.Epc.ToString()))
                //{
                //System.Console.WriteLine("TagReport is:" + tag);
                double phaseAngle;
                phaseAngle = tag.PhaseAngleInRadians;
                //System.Console.WriteLine("current phase is: " +tag.PhaseAngleInRadians);
                //System.Console.WriteLine("epc : {0}  phase ：{1} ",
                //                        tag.Epc, tag.PhaseAngleInRadians);


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
                //row["Counter"]=1;

                //intIndex = TagList.IndexOf(tag.Epc.ToString());
                //PreviousCounter = TagCounter[intIndex];
                //PreviousCounter = PreviousCounter + 1;
                //TagCounter[intIndex] = PreviousCounter;

                string EPCStr = tag.Epc.ToString();
                //把有值的列添加到表
                if (EPCStr.Contains("0093"))
                {
                    IncomingTagNumber93 = IncomingTagNumber93 + 1;
                }
                else if (EPCStr.Contains("0087"))
                {
                    IncomingTagNumber87 = IncomingTagNumber87 + 1;
                }
                else if (EPCStr.Contains("0096"))
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



        static void OnTagsReported2(ImpinjReader sender, TagReport report)
        {
            // This event handler is called asynchronously
            // when tag reports are available.
            // Loop through each tag in the report
            // and print the data.
            //System.Console.WriteLine("TagReport is:" + report);
            foreach (Tag tag in report)
            {
                //Console.WriteLine("EPC : {0} Doppler Frequency (Hz) : {1} Current Frequecy: {2}  PeakRSSI ：{3}  PhaseAngle : {4} PhaseDegree :{5}",
                //                    tag.Epc, tag.RfDopplerFrequency.ToString("0.00"), tag.ChannelInMhz, tag.PeakRssiInDbm, tag.PhaseAngleInRadians, ((tag.PhaseAngleInRadians) / Math.PI) * 180);

                //if (stock.Contains(tag.Epc.ToString()))
                //{
                //System.Console.WriteLine("TagReport is:" + tag);
                double phaseAngle;
                phaseAngle = tag.PhaseAngleInRadians;
                //System.Console.WriteLine("current phase is: " +tag.PhaseAngleInRadians);
                System.Console.WriteLine("epc : {0}  phase ：{1} ",
                                        tag.Epc, tag.PhaseAngleInRadians);


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
                //row["Counter"]=1;

                //intIndex = TagList.IndexOf(tag.Epc.ToString());
                //PreviousCounter = TagCounter[intIndex];
                //PreviousCounter = PreviousCounter + 1;
                //TagCounter[intIndex] = PreviousCounter;

                string EPCStr = tag.Epc.ToString();
                //把有值的列添加到表
                if (EPCStr.Contains("0093"))
                {
                    IncomingTagNumber93 = IncomingTagNumber93 + 1;
                }
                else if (EPCStr.Contains("0087"))
                {
                    IncomingTagNumber87 = IncomingTagNumber87 + 1;
                }
                else if (EPCStr.Contains("0096"))
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
            // 创建表中的列
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
            // 初始化列名

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
            //foreach (int element in TagCounter)
            //{
            //    Console.WriteLine(element);
            //    Console.WriteLine("hello");
            //}
            Console.WriteLine("Press enter to exit.");


            //Console.WriteLine(TagCounter);
            Console.ReadLine();
            // 写CSV文件
            CsvWriter.AddData(DataDS, 1);
            CsvWriter.Save();


            // Disconnect from the reader.
            reader.Disconnect();
            Console.WriteLine(DataDS.Rows.Count);
            Console.ReadLine();


        }

        static public void filterTags(Settings settings)
        {
            // Setup a tag filter.
            // Only the tags that match this filter will respond.
            // First, setup tag filter #1.
            // We want to apply the filter to the EPC memory bank.
            settings.Filters.TagFilter1.MemoryBank = MemoryBank.Epc;
            // Start matching at the third word (bit 32), since the
            // first two words of the EPC memory bank are the
            // CRC and control bits. BitPointers.Epc is a helper
            // enumeration you can use, so you don't have to remember this.
            settings.Filters.TagFilter1.BitPointer = BitPointers.Epc;
            // Only match tags with EPCs that start with "3008"
            settings.Filters.TagFilter1.TagMask = "0000 0000 0000 0000 0000";
            // This filter is 16 bits long (one word).
            settings.Filters.TagFilter1.BitCount = 8;

          

        }

        static public int fixFrequency(FeatureSet features, Settings settings)
        {
            // Specify the transmit frequencies to use.
            // Make sure your reader supports this and
            // that the frequencies are valid for your region.
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

        //static void txPowerRead(FeatureSet features, Settings settings)
        //{
        //    foreach (TxPowerTableEntry tx in features.TxPowers)
        //    {
        //        // Set the transmit power (in dBm).
        //        Console.WriteLine("Setting Tx Power to {0} dBm", tx.Dbm);
        //        settings.Antennas.GetAntenna(antennaPort).TxPowerInDbm = tx.Dbm;
        //        txPowerValue = tx.Dbm;

        //        // Apply the new transmit power settings.
        //        reader.ApplySettings(settings);

        //        // Start the reader.
        //        reader.Start();

        //        // Wait
        //        Thread.Sleep(1000);

        //        // Stop the reader.
        //        reader.Stop();
        //    }
        //    applicactionClose();
        //}



    }
}


