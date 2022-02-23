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
                ErrorExit("!Missing input file", 1);
            NmmFileName nmmFileName = new NmmFileName(fileNames[0]);
            string outPutFilename = nmmFileName.BaseFileName + ".csv";

            // requested output style
            OutputStyle outputStyle = OutputStyle.Pretty;
            if (options.Plain)
                outputStyle = OutputStyle.Plain;

            Csv csv = new Csv(outputStyle);

            NmmDescriptionFileParser nmmDsc = new NmmDescriptionFileParser(nmmFileName);
            int numberOfScans = nmmDsc.NumberOfScans;
            NmmEnvironmentData nmmPos;
            if (numberOfScans==1)
            {
                nmmFileName.SetScanIndex(0);
                nmmPos = new NmmEnvironmentData(nmmFileName);
                csv.Add(nmmPos, 0);
            }
            else
            {
                for (int scanIndex = 1; scanIndex <= numberOfScans; scanIndex++)
                {
                    nmmFileName.SetScanIndex(scanIndex);
                    nmmPos = new NmmEnvironmentData(nmmFileName);
                    csv.Add(nmmPos, scanIndex);
                }
            }



            File.WriteAllText(outPutFilename, csv.GetCsvString());

        }

        /**********************************************************************/

        private static void ErrorExit(string message, int code)
        {
            Console.WriteLine($"{message} (error code {code})");
            Environment.Exit(code);
        }
    }
}
