using CommandLine;

namespace FixedFormPackager
{
    public class Options
    {
        [Option('i', "itemInput", Required = true,
            HelpText = "Path to a valid .csv file containing the items to be packaged")]
        public string ItemInput { get; set; }

        [Option('a', "assessmentInput", Required = true,
            HelpText = "Path to a valid .csv file containing the metadata about the assessment to be packaged")]
        public string AssessmentInput { get; set; }

        [Option('n', "gitLabUsername", Required = true,
            HelpText = "Username for GitLab account where items and stimuli are located")]
        public string GitLabUsername { get; set; }

        [Option('p', "gitLabPassword", Required = true,
            HelpText = "Password for GitLab account where items and stimuli are located")]
        public string GitLabPassword { get; set; }

        [Option('l', "gitLabBaseUrl", Required = true,
            HelpText = "Base url for GitLab account where items are located")]
        public string GitLabBaseUrl { get; set; }

        [Option('g', "gitLabGroup", Required = true,
            HelpText = "The group where the items and stimuli are located in GitLab")]
        public string GitLabGroup { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return "Help string\n";
        }
    }
}