using System;
using System.Configuration;
using System.IO;
using System.Linq;
using FixedFormPackager.Common.Extensions;
using FixedFormPackager.Common.Models;
using LibGit2Sharp;
using NLog;

namespace ItemRetriever.GitLab
{
    public class ResourceRetriever
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Retrieve(GitLabInfo gitLabInfo, string identifier)
        {
            var cloneOptions = new CloneOptions
            {
                CredentialsProvider = (url, user, cred) =>
                    GenerateCredentials(gitLabInfo)
            };

            var resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                identifier.Contains("Item") ? "Items" : "Stimuli",
                identifier);
            if (!Directory.Exists(resourcePath))
            {
                Logger.LogInfo(new ProcessingReportItem
                {
                    Destination = resourcePath,
                    Type = identifier.Contains("Item") ? "Item" : "Stimulus",
                    UniqueId = identifier
                }, $"Local repository not found. Cloning {resourcePath.Split('\\').LastOrDefault() ?? string.Empty}");
                Repository.Clone(
                    $"{gitLabInfo.BaseUrl}{gitLabInfo.Group}/{identifier}.git",
                    resourcePath, cloneOptions);
            }
            else
            {
                using (var repository = new Repository(resourcePath))
                {
                    var pullOptions = new PullOptions
                    {
                        FetchOptions = new FetchOptions
                        {
                            CredentialsProvider = (url, user, cred) =>
                                GenerateCredentials(gitLabInfo)
                        }
                    };
                    Logger.LogInfo(new ProcessingReportItem
                    {
                        Destination = resourcePath,
                        Type = identifier.Contains("Item") ? "Item" : "Stimulus",
                        UniqueId = identifier
                    }, $"Local repository found. Pulling {resourcePath.Split('\\').LastOrDefault() ?? string.Empty}");
                    Commands.Pull(repository,
                        new Signature(new Identity(
                                ConfigurationManager.AppSettings["userName"],
                                ConfigurationManager.AppSettings["userEmail"])
                            , DateTimeOffset.Now), pullOptions);
                }
            }
        }

        private static UsernamePasswordCredentials GenerateCredentials(GitLabInfo gitLabInfo)
        {
            return new UsernamePasswordCredentials
            {
                Username = gitLabInfo.Username,
                Password = gitLabInfo.Password
            };
        }
    }
}