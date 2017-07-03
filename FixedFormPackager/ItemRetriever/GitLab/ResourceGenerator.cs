using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using FixedFormPackager.Common.Extensions;
using FixedFormPackager.Common.Models;
using ItemRetriever.Utilities;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using NLog;
using LogLevel = NLog.LogLevel;

namespace ItemRetriever.GitLab
{
    public class ResourceGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string Retrieve(GitLabInfo gitLabInfo, string identifier)
        {
            var cloneOptions = new CloneOptions
            {
                CredentialsProvider = (url, user, cred) =>
                    GenerateCredentials(gitLabInfo)
            };
            var remoteRepoPath = GenerateValidRemoteRepositoryPath(gitLabInfo, identifier);
            var resourcePath = PathHelper.RetrievePathForId(identifier);
            if (!Directory.Exists(resourcePath))
            {
                Logger.LogInfo(new ProcessingReportItem
                {
                    Destination = resourcePath,
                    Type = identifier.Contains("Item") ? "Item" : "Stimulus",
                    UniqueId = identifier
                }, $"Local repository not found. Cloning {resourcePath.Split('\\').LastOrDefault() ?? string.Empty}");
                Repository.Clone(remoteRepoPath, resourcePath, cloneOptions);
                return remoteRepoPath;
            }
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
            return remoteRepoPath.Split('/').LastOrDefault()?.Replace(".git", string.Empty);
        }

        private static string GenerateValidRemoteRepositoryPath(GitLabInfo gitLabInfo, string identifier)
        {
            string locationBase = $"{gitLabInfo.BaseUrl}{gitLabInfo.Group}";
            const string defaultBankKey = "187";
            switch (identifier.Count(x => x == '-'))
            {
                case 0: // This is missing both the bank key and the 'Item-' prefix - add both
                    identifier = $"Item-{defaultBankKey}-{identifier}";
                    break;
                case 1: // This is missing the bank key - we add the default
                    identifier = $"Item-{defaultBankKey}-{identifier.Split('-').Last()}";
                    break;
                case 2: // This is the expected format. We do nothing
                    break;
                default: // This  is an error
                    return null;
            }
               var potentialItemFormats = new Dictionary<string, string>
            {
                {"SBAC content package format", $"{locationBase}/{identifier}.git" },
                {"IAT item format", $"{locationBase}/{identifier.Split('-').LastOrDefault() ?? string.Empty}.git" },
                {"No bank key", $"{locationBase}/Item-{identifier.Split('-').LastOrDefault() ?? string.Empty}.git" }
                   // Potentially adding item type mappings for IAT
            };
            return potentialItemFormats.Values.FirstOrDefault(x =>
            {
                try
                {
                    Repository.ListRemoteReferences(x, (url, user, cred) =>
                        GenerateCredentials(gitLabInfo));
                    return true;
                }
                catch (Exception exception)
                {
                    Logger.LogError(new ErrorReportItem
                    {
                        Location = "Resource Generator - locate repository",
                        Severity = LogLevel.Fatal
                    },
                    $"An error occurred when attempting to resolve remote repository references: {exception.Message}");
                    return false;
                }
            });
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