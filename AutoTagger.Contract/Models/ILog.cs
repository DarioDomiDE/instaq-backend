﻿namespace AutoTagger.Contract.Models
{
    using System;
    using System.Collections.Generic;

    public interface ILog
    {
        int Id { get; set; }

        string Data { get; set; }

        DateTime Created { get; set; }

        IDictionary<string, object> GetDataAsList();

        void SetData(IDictionary<string, object> data);
    }
}