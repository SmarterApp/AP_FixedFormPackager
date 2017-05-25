using System;
using System.Linq;
using AssessmentPackageBuilder.Common;
using CommandLine;
using FixedFormPackager.Common.Models;
using FixedFormPackager.Common.Models.Csv;
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
                    ExtractionSettings.GitLabInfo = new GitLabInfo
                    {
                        BaseUrl = options.GitLabBaseUrl,
                        Group = options.GitLabGroup,
                        Password = options.GitLabPassword,
                        Username = options.GitLabUsername
                    };
                    ExtractionSettings.AssessmentScoring =
                        CsvExtractor.Extract<AssessmentScoringComputationRule>(options.AssessmentScoringInput).ToList();
                    ExtractionSettings.ItemInput = CsvExtractor.Extract<Item>(options.ItemInput).ToList();
                    ExtractionSettings.AssessmentInfo =
                        CsvExtractor.Extract<Assessment>(options.AssessmentInput).First();
                    ExtractionSettings.ItemInput.ForEach(x =>
                    {
                        ResourceGenerator.Retrieve(ExtractionSettings.GitLabInfo, $"Item-{x.ItemId}");
                        if (!string.IsNullOrEmpty(x.AssociatedStimuliId))
                        {
                            ResourceGenerator.Retrieve(ExtractionSettings.GitLabInfo, $"stim-{x.AssociatedStimuliId}");
                        }
                    });
                    var itemPool = ItemPool.Construct(ExtractionSettings.ItemInput);
                    var testBlueprint = TestBlueprint.Construct(ExtractionSettings.ItemInput, itemPool,
                        ExtractionSettings.AssessmentInfo.UniqueId);
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