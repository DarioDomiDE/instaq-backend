﻿using System;
using System.Collections.Generic;

namespace AutoTagger.Database.Mysql
{
    using AutoTagger.Contract;

    public partial class Photos
    {
        public Photos()
        {
            Itags = new HashSet<Itags>();
            Mtags = new HashSet<Mtags>();
        }

        public int Id { get; set; }
        public string Url { get; set; }
        public string Shortcode { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public int Follower { get; set; }
        public string User { get; set; }
        public DateTimeOffset? Uploaded { get; set; }
        public DateTimeOffset Created { get; set; }

        public ICollection<Itags> Itags { get; set; }
        public ICollection<Mtags> Mtags { get; set; }

        public static Photos FromImage(IImage image)
        {
            var photo = new Photos
            {
                Url      = image.Url,
                Shortcode = image.Shortcode,
                Likes    = image.Likes,
                Comments = image.CommentCount,
                Follower = image.Follower,
                User     = image.User,
                Uploaded = image.Uploaded
            };
            if (image.HumanoidTags != null)
            {
                foreach (var iTag in image.HumanoidTags)
                {
                    photo.Itags.Add(new Itags { Value = iTag });
                }
            }

            if (image.MachineTags != null)
            {
                foreach (var mTag in image.MachineTags)
                {
                    photo.Mtags.Add(new Mtags { Value = mTag });
                }
            }

            return photo;
        }

    }
}
