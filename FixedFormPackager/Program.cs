using System;
using System.Linq;
using CommandLine;
using FixedFormPackager.Common.Models;
using FixedFormPackager.Common.Utilities;
using ItemRetriever.GitLab;
using NLog;

namespace FixedFormPackager
{
    internal class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            Logger.Debug(string.Concat(Enumerable.Repeat("-", 60)));
            Logger.Debug("Fixed Form Packager Initialized");
            try
            {
                var options = new Options();
                if (Parser.Default.ParseArgumentsStrict(args, options,
                    () =>
                    {
                        Logger.Fatal(
                            $"Incorrect parameters provided at the command line [{args.Aggregate((x, y) => $"{x},{y}")}]. Terminating.");
                    }))
                {
                    ExtractionSettings.UniqueId = options.UniqueId;
                    ExtractionSettings.Input = options.Input;
                    ExtractionSettings.GitLabInfo = new GitLabInfo
                    {
                        BaseUrl = options.GitLabBaseUrl,
                        Group = options.GitLabGroup,
                        Password = options.GitLabPassword,
                        Username = options.GitLabUsername
                    };
                    var result = CsvExtractor.ExtractItemInput(ExtractionSettings.Input);
                    result.ToList().ForEach(x =>
                    {
                        ResourceGenerator.Retrieve(ExtractionSettings.GitLabInfo, $"Item-{x.ItemId}");
                        if (!string.IsNullOrEmpty(x.AssociatedStimuliId))
                        {
                            ResourceGenerator.Retrieve(ExtractionSettings.GitLabInfo, $"stim-{x.AssociatedStimuliId}");
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            finally
            {
                Console.Write("Press any key to exit.");
                Console.ReadKey(true);
            }
        }
    }
}