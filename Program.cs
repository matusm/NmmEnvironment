﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Bev.IO.NmmReader;
using Bev.IO.NmmReader.scan_mode;

namespace NmmEnvironment
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Options options = new Options();

            if (!CommandLine.Parser.Default.ParseArgumentsStrict(args, options))
                Console.WriteLine("*** ParseArgumentsStrict returned false");

            // get the filename(s)
            string[] fileNames = options.ListOfFileNames.ToArray();
            if (fileNames.Length == 0)
                ErrorExit("!input file not specified", 1);
            NmmFileName nmmFileName = new NmmFileName(fileNames[0]);
            string outPutFilename = nmmFileName.BaseFileName + "_THP.csv";

            Csv csv = new Csv(options.CsvStyle);
            Statistics stat = new Statistics();

            NmmDescriptionFileParser nmmDsc = new NmmDescriptionFileParser(nmmFileName);
            Console.WriteLine($"{nmmFileName.BaseFileName} [{nmmDsc.Procedure}]");
            bool hasDuration = nmmDsc.Procedure == MeasurementProcedure.Scan;
            if (nmmDsc.Procedure == MeasurementProcedure.NoFile)
                ErrorExit("!file not found(?)", 2);
            int numberOfScans = nmmDsc.NumberOfScans;
            NmmEnvironmentData nmmPos;
            NmmIndFileParser nmmInd;
            double scanDuration = 0.0;
            if (numberOfScans == 1)
            {
                nmmFileName.SetScanIndex(0);
                nmmPos = new NmmEnvironmentData(nmmFileName);
                Console.WriteLine($"Single scan : {nmmPos.AirTemperatureSeries.Length} samples");
                csv.Add(nmmPos, 0);
                stat.Update(nmmPos);
                if (hasDuration)
                {
                    nmmInd = new NmmIndFileParser(nmmFileName);
                    scanDuration = nmmInd.ScanDuration.TotalSeconds;
                }
            }
            else
            {
                for (int scanIndex = 1; scanIndex <= numberOfScans; scanIndex++)
                {
                    nmmFileName.SetScanIndex(scanIndex);
                    nmmPos = new NmmEnvironmentData(nmmFileName);
                    Console.WriteLine($"Scan #{scanIndex} : {nmmPos.AirTemperatureSeries.Length} samples");
                    csv.Add(nmmPos, scanIndex);
                    stat.Update(nmmPos);
                    if (hasDuration)
                    {
                        nmmInd = new NmmIndFileParser(nmmFileName);
                        scanDuration += nmmInd.ScanDuration.TotalSeconds;
                    }
                }
            }

            File.WriteAllText(outPutFilename, csv.GetCsvString());
            Console.WriteLine($"{csv.RunningIndex} samples in file {outPutFilename}");
            if (hasDuration && scanDuration > 0)
            {
                Console.WriteLine($"Total duration: {scanDuration:F0} s");
            }
            Console.WriteLine();
            Console.WriteLine($"Table: {stat.SampleTemperature:F2} °C ± {stat.SampleTemperatureRange / 2:F2} °C");
            Console.WriteLine($"Air:   {stat.AirTemperature:F2} °C ± {stat.AirTemperatureRange / 2:F2} °C");

        }

        /**********************************************************************/

        private static void ErrorExit(string message, int code)
        {
            Console.WriteLine($"{message} (error code {code})");
            Environment.Exit(code);
        }
    }
}
