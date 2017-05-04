using System;
using System.Collections.Generic;
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
                var inputFilenames = new List<string>();
                string outputFilename = null;

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
                            Logger.Info($"Input found: {args[i]}");
                            inputFilenames.Add(args[i]);
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
                            if (outputFilename != null)
                            {
                                Logger.Error(
                                    $"Output filename already set to: {outputFilename}, cannot set to {args[i]}");
                                throw new ArgumentException("Only one item output filename may be specified.");
                            }
                            outputFilename = args[i];
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
                            break;
                        // If this argument is not provided, the application will infer the grade based on the items
                        case "-grade":
                            if (i >= args.Length)
                            {
                                Logger.Error("-id option must be followed by a valid string value");
                                throw new ArgumentException(
                                    "Invalid command line. '-id' option not followed by value.");
                            }
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
                else if (inputFilenames.Count == 0 || outputFilename == null)
                {
                    Logger.Error(
                        "Invalid command line. One output filename and at least one input filename must be specified.");
                }
                else
                {
                    var result = inputFilenames.ToList().SelectMany(CsvExtractor.ExtractTestItemInput).ToList();
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