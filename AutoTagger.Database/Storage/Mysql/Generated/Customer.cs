﻿using System;
using System.Collections.Generic;

namespace AutoTagger.Database
{
    public partial class Customer
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int PhotosCount { get; set; }
    }
}