﻿using FixedFormPackager.Common.Models;
using ItemRetriever.GitLab;
using NUnit.Framework;

namespace FixedFormPackager.Test.ItemRetriever
{
    [TestFixture]
    public class ItemRetrieverTest
    {
        [Test]
        public void ItemRetrieverCanRetrieveItem()
        {
            ResourceGenerator.Retrieve(new GitLabInfo(), "Item-187-1072");
        }
    }
}