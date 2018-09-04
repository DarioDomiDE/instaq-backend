﻿namespace AutoTagger.Crawler.V3.Crawler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Contract;

    class UserPageCrawler : BaseImagePageCrawler
    {
        private const int MinFollowerCount = 1000;

        public UserPageCrawler()
        {
            this.MinHashTagCount  = 5;
            this.MinCommentsCount = 10;
            this.MinLikes         = 300;
        }

        public IEnumerable<IImage> Parse(string url)
        {
            var data = this.GetData(url);

            if (!HasUserEnoughFollower(data, out int followerCount, out int followingCount, out int postsCount))
            {
                yield break;
            }

            var nodes = GetNodes(data);
            var images = this.GetImages(nodes);

            foreach (IImage image in images)
            {
                image.Follower  = followerCount;
                image.Following = followingCount;
                image.Posts = postsCount;
            }

            images = this.RemoveImagesWithDuplicateHashtags(images);

            foreach (IImage image in images)
            {
                yield return image;
            }
        }

        private IEnumerable<IImage> RemoveImagesWithDuplicateHashtags(IList<IImage> images)
        {
            var newImages = new Dictionary<string, IImage>();
            for (int i = images.Count-1; i >= 0; i--)
            {
                var image = images[i];
                var hashTags = string.Join("", image.HumanoidTags);
                if (!newImages.ContainsKey(hashTags))
                    newImages.Add(hashTags, image);
            }
            foreach (var newImage in newImages)
            {
                yield return newImage.Value;
            }
        }

        private static bool HasUserEnoughFollower(dynamic data, out int followerCount, out int followingCount, out int postsCount)
        {
            var node      = data?.entry_data?.ProfilePage?[0]?.graphql?.user;
            followerCount = Convert.ToInt32(node?.edge_followed_by?.count.ToString());
            followingCount = Convert.ToInt32(node?.edge_follow?.count.ToString());
            postsCount = Convert.ToInt32(node?.edge_owner_to_timeline_media?.count.ToString());
            return followerCount >= MinFollowerCount;
        }

        private dynamic GetNodes(dynamic data)
        {
            return data?.entry_data?.ProfilePage?[0]?.graphql?.user?.edge_owner_to_timeline_media?.edges;
        }
    }
}