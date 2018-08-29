﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AutoTagger.API.Controllers
{
    using AutoTagger.Contract.Models;
    using AutoTagger.Contract.Storage;

    using Newtonsoft.Json;

    [Route("[controller]")]
    public class DebugController : Controller
    {
        private IDebugStorage debugStorage;

        public DebugController(IDebugStorage debugStorage)
        {
            this.debugStorage = debugStorage;
        }

        [Route("Stats/PhotosCount")]
        [HttpGet]
        public IActionResult GetPhotosCount()
        {
            var count = this.debugStorage.GetPhotosCount();
            return this.Ok(count);
        }

        [Route("Stats/HumanoidTagsCount")]
        [HttpGet]
        public IActionResult GetHumanoidTagsCount()
        {
            var count = this.debugStorage.GetHumanoidTagsCount();
            return this.Ok(count);
        }

        [Route("Stats/HumanoidTagRelationsCount")]
        [HttpGet]
        public IActionResult GetHumanoidTagRelationCount()
        {
            var count = this.debugStorage.GetHumanoidTagRelationCount();
            return this.Ok(count);
        }

        [Route("Stats/MachineTagsCount")]
        [HttpGet]
        public IActionResult GetMachineTagsCount()
        {
            var count = this.debugStorage.GetMachineTagsCount();
            return this.Ok(count);
        }

        [Route("Logs/{page}")]
        [HttpGet]
        public IEnumerable<Dictionary<string, object>> GetLogs(int page)
        {
            var count = 10;
            var logs = this.debugStorage.GetLogs(count, count * (page - 1));
            foreach (var log in logs)
            {
                var entries = JsonConvert.DeserializeObject<Dictionary<string, object>>(log.Data);
                foreach (var entry in entries)
                {
                    if (entry.Value is string valueAsStr && valueAsStr.Substring(0, 2) == "[{")
                    {
                        entries[entry.Key] = JsonConvert.DeserializeObject<Dictionary<string, object>>(valueAsStr);
                    }
                }
                entries.Add("id", log.Id);
                entries.Add("created", log.Created);
                yield return entries;
            }
        }

    }
}