using System;
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
                CredentialsProvider =
                    (url, user, cred) =>
                        new UsernamePasswordCredentials
                        {
                            Username = gitLabInfo.Username,
                            Password = gitLabInfo.Password
                        }
            };

            Repository.Clone(
                $"{gitLabInfo.BaseUrl}{gitLabInfo.Group}/{identifier}.git",
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, identifier.Contains("Item") ? "Items" : "Stimuli",
                    identifier), cloneOptions);
        }
    }
}