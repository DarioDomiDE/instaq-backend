﻿namespace AutoTagger.Test.Core
{
    using System;
    using System.Collections.Generic;
    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Database.Standard.Storage.Mysql;
    using Xunit;
    using Image = Crawler.Standard.Image;

    public class MysqlTests
    {
        [Fact]
        public void MysqlInsertPhoto()
        {
            // Arrange
            var crawlerDb = new MysqlCrawlerStorage();
            crawlerDb.GetAllHumanoidTags<HumanoidTag>();
            var image = new Image
            {
                Comments = 10,
                Follower = 99,
                Following = 150,
                Posts = 42,
                HumanoidTags = new List<string> { "catlove", "instabeach", "hamburg" },
                //MachineTags = new List<string> { "cat", "beach", "city" },
                LargeUrl = "content.com/pic/ab12xy67laaaarge",
                ThumbUrl = "content.com/pic/ab12xthump",
                Shortcode = "ab12xy67",
                Likes = 1337,
                User = "DarioDomi",
                Uploaded = DateTime.Now
            };

            // Act

            crawlerDb.Upsert(image);

            // Assert
            Assert.NotEmpty(image.Shortcode);
        }

        [Fact]
        public void MysqlInsertITag()
        {
            // Arrange
            var crawlerDb = new MysqlCrawlerStorage();
            crawlerDb.GetAllHumanoidTags<HumanoidTag>();
            var name = "Altona";
            var posts = 14;
            var humanoidTag = new HumanoidTag {Name = name, Posts = posts};

            // Act
            crawlerDb.InsertOrUpdateHumanoidTag(humanoidTag);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void WhenGettingAllPhotos()
        {
            // Arrange
            var mysql = new MysqlUiStorage();

            // Act
            var machineTags = new List<IMachineTag> { new MachineTag {Name = "beach" }, new MachineTag {Name = "water" } };
            var (debug, htags) = mysql.FindMostRelevantHumanoidTags(machineTags);

            // Assert
            Assert.NotEmpty(debug);
            Assert.NotEmpty(htags);
        }

        [Fact]
        public void MysqlInsertMTag()
        {
            // Arrange
            var db = new MysqlImageProcessorStorage();
            var image = new Image();
            var mtags = new List<IMachineTag>
            {
                new MachineTag { Name = "test", Score = 1.337f, Source = "Testcase_Test-Hamburg4ever" }
            };
            image.MachineTags = mtags;
            image.Id = 1;

            // Act
            db.InsertMachineTagsWithoutSaving(image);
            db.Save();

            // Assert
            Assert.True(true);
        }
    }
}
