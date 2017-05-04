using System;
using System.Diagnostics;
using System.IO;
using FixedFormPackager.Common.Extensions;
using FixedFormPackager.Common.Models;
using NLog;

namespace FixedFormPackager.Test.Utilities
{
    public static class ProcessUtility
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void LaunchExecutable(string options)
        {
            // The path to the tabulator executable
            var executablePath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName,
                "Resources",
                "TabulateSmarterTestPackage.exe");

            // Use ProcessStartInfo class
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                FileName = executablePath,
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = options
            };

            try
            {
                using (var exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                Logger.LogError(new ErrorReportItem
                {
                    Location = "Process Utility (Integration Test)",
                    Severity = LogLevel.Fatal
                });
            }
        }
    }
}