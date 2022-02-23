using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace NmmEnvironment
{
    class Options
    {
        [Option('p', "pretty", HelpText = "Pretty (sort of) CSV file.")]
        public bool Pretty { get; set; }

        [ValueList(typeof(List<string>), MaximumElements = 2)]
        public IList<string> ListOfFileNames { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string AppVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            HelpText help = new HelpText
            {
                Heading = new HeadingInfo(AppName, "version " + AppVer),
                Copyright = new CopyrightInfo("Michael Matus", 2022),
                AdditionalNewLineAfterOption = false,
                AddDashesToOption = true
            };
            string sPre = "Program to extract environmental data from the scanning files produced by SIOS NMM-1.";
            help.AddPreOptionsLine(sPre);
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("Usage: " + AppName + " filename [options]");
            help.AddPostOptionsLine("");

            help.AddOptions(this);

            return help;
        }


    }
}
