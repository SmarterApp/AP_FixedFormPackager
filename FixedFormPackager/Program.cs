using System;
using System.IO;
using System.Linq;
using FixedFormPackager.Common.Utilities;
using NLog;

namespace FixedFormPackager
{
    internal class Program
    {
        private const string HelpMessage =
            @"This is a description of the application";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            Logger.Debug(string.Concat(Enumerable.Repeat("-", 60)));
            Logger.Debug("Fixed Form Packager Initialized");
            try
            {
                var help = false;

                for (var i = 0; i < args.Length; ++i)
                {
                    switch (args[i])
                    {
                        case "-h":
                            help = true;
                            Logger.Debug("Application run in help mode");
                            break;
                        case "-i":
                        {
                            ++i;
                            if (i >= args.Length)
                            {
                                Logger.Error("-i option must be followed by a valid string filename");
                                throw new ArgumentException(
                                    "Invalid command line. '-i' option not followed by filename.");
                            }
                            if (!string.IsNullOrEmpty(ExtractionSettings.Input))
                            {
                                throw new ArgumentException("Only one input filename may be specified.");
                            }
                            Logger.Info($"Input found: {args[i]}");
                            ExtractionSettings.Input = args[i];
                        }
                            break;
                        case "-o":
                        {
                            ++i;
                            if (i >= args.Length)
                            {
                                Logger.Error("-o option must be followed by a valid string filename");
                                throw new ArgumentException(
                                    "Invalid command line. '-o' option not followed by filename.");
                            }
                            if (!string.IsNullOrEmpty(ExtractionSettings.Output))
                            {
                                throw new ArgumentException("Only one item output filename may be specified.");
                            }
                            ExtractionSettings.Output = args[i];
                            Logger.Info($"Output filename set to: {args[i]}");
                            Directory.CreateDirectory(args[i]);
                        }
                            break;
                        case "-id":
                            ++i;
                            if (i >= args.Length)
                            {
                                Logger.Error("-id option must be followed by a valid string value");
                                throw new ArgumentException(
                                    "Invalid command line. '-id' option not followed by value.");
                            }
                            ExtractionSettings.UniqueId = args[i];
                            break;
                        // If this argument is not provided, the application will infer the grade based on the items
                        case "-g":
                            if (i >= args.Length)
                            {
                                Logger.Error("-g option must be followed by a valid string value");
                                throw new ArgumentException(
                                    "Invalid command line. '-g' option not followed by value.");
                            }
                            ExtractionSettings.Grade = args[i];
                            break;
                        // If this argument is not provided, the application will attempt to infer the publisher based on the uniqueid.
                        // If unable to determine the publisher from either input or uniqueid, it will fall back to the config value
                        case "-p":
                            if (i >= args.Length)
                            {
                                Logger.Error("-p option must be followed by a valid string value");
                                throw new ArgumentException(
                                    "Invalid command line. '-p' option not followed by value.");
                            }
                            ExtractionSettings.Grade = args[i];
                            break;
                        default:
                            Logger.Error($"Unknown command line option '{args[i]}'. Use '-h' for syntax help.");
                            throw new ArgumentException(
                                $"Unknown command line option '{args[i]}'. Use '-h' for syntax help.");
                    }
                }

                if (help || args.Length == 0)
                {
                    Console.WriteLine(HelpMessage);
                }
                else if (string.IsNullOrEmpty(ExtractionSettings.Input) ||
                         string.IsNullOrEmpty(ExtractionSettings.Output) ||
                         string.IsNullOrEmpty(ExtractionSettings.UniqueId))
                {
                    Logger.Error(
                        "Invalid command line. Output filename (-o), input filename (-i), and unique identifier (-id) are required inputs.");
                }
                else
                {
                    var result = CsvExtractor.ExtractItemInput(ExtractionSettings.Input);
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