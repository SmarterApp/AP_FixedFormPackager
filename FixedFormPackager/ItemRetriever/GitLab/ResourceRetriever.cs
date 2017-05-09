using System;
using System.Configuration;
using System.IO;
using FixedFormPackager.Common.Models;
using LibGit2Sharp;

namespace ItemRetriever.GitLab
{
    public class ResourceRetriever
    {
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