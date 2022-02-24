using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Bev.IO.NmmReader;

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
            string outPutFilename = nmmFileName.BaseFileName + ".csv";

            // requested output style
            OutputStyle outputStyle = OutputStyle.Plain;
            if (options.Pretty)
                outputStyle = OutputStyle.Pretty;
            Csv csv = new Csv(outputStyle);

            NmmDescriptionFileParser nmmDsc = new NmmDescriptionFileParser(nmmFileName);
            Console.WriteLine($"{nmmFileName.BaseFileName} [{nmmDsc.Procedure}]");
            if(nmmDsc.Procedure== MeasurementProcedure.Unknown)
                ErrorExit("!file not found(?)", 2);
            int numberOfScans = nmmDsc.NumberOfScans;
            NmmEnvironmentData nmmPos;
            if (numberOfScans==1)
            {
                nmmFileName.SetScanIndex(0);
                nmmPos = new NmmEnvironmentData(nmmFileName);
                Console.WriteLine($"Single scan : {nmmPos.AirTemperatureSeries.Length} samples");
                csv.Add(nmmPos, 0);
            }
            else
            {
                for (int scanIndex = 1; scanIndex <= numberOfScans; scanIndex++)
                {
                    nmmFileName.SetScanIndex(scanIndex);
                    nmmPos = new NmmEnvironmentData(nmmFileName);
                    Console.WriteLine($"Scan #{scanIndex} : {nmmPos.AirTemperatureSeries.Length} samples");
                    csv.Add(nmmPos, scanIndex);
                }
            }

            File.WriteAllText(outPutFilename, csv.GetCsvString());
            Console.WriteLine($"{csv.RunningIndex} samples in file {outPutFilename}");

        }

        /**********************************************************************/

        private static void ErrorExit(string message, int code)
        {
            Console.WriteLine($"{message} (error code {code})");
            Environment.Exit(code);
        }
    }
}
