using CommandLine;

namespace FixedFormPackager
{
    public class Options
    {
        [Option('i', "input", Required = true,
            HelpText = "Path to a valid .csv file containing the items to be packaged")]
        public string Input { get; set; }

        [Option('u', "unique identifier", Required = true,
            HelpText = "The unique identifier of the assessment to be constructed")]
        public string UniqueId { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return "Help string\n";
        }
    }
}