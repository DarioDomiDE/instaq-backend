﻿namespace AutoTagger.FileHandling.Standard
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using AutoTagger.Contract;

    public class DiskFileHander : IFileHandler
    {
        private readonly string Ext = ".jpg";

        private readonly string PathDefect = @"C:\Instagger\Defect\";
        private readonly string PathUnused = @"C:\Instagger\Unused\";
        private readonly string PathUsed = @"C:\Instagger\Used\";
        private readonly string PathUser = @"C:\Instagger\User\";

        public void Delete(string name)
        {
            var path = this.PathUnused + name + this.Ext;
            File.Delete(path);
        }

        public bool FileExists(FileType fileType, string name)
        {
            var folder = this.GetFolder(fileType);
            var path   = folder + name + this.Ext;
            return File.Exists(path);
        }

        public void FlagAsDefect(string name)
        {
            this.FlagAs(FileType.Unused, FileType.Defect, name);
        }

        public void FlagAsUsed(string name)
        {
            this.FlagAs(FileType.Unused, FileType.Used, name);
        }

        private void FlagAs(FileType from, FileType to, string name)
        {
            var fromPath = this.GetFolder(from) + name + this.Ext;
            var toPath   = this.GetFolder(to) + name + this.Ext;
            File.Move(fromPath, toPath);
        }

        public IList<string> GetAllUnusedImages()
        {
            var files = Directory.GetFiles(this.PathUnused, "*" + this.Ext);
            return files.Select(x => x.Replace(this.PathUnused, "").Replace(this.Ext, "")).ToList();
        }

        public int GetFileSize(FileType fileType, string filename)
        {
            var path = this.GetFolder(fileType) + filename + this.Ext;
            return File.ReadAllBytes(path).Length;
        }

        public string GetFullPath(string name)
        {
            return this.PathUnused + name + this.Ext;
        }

        public void Save(FileType fileType, byte[] bytes, string filename)
        {
            var path = this.GetFolder(fileType) + filename;
            File.WriteAllBytes(path, bytes);
        }

        public byte[] GetFile(FileType fileType, string fileNameAndExt)
        {
            var path = this.GetFolder(fileType) + fileNameAndExt;
            return File.ReadAllBytes(path);
        }

        private string GetFolder(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Unused:
                    return this.PathUnused;
                case FileType.Used:
                    return this.PathUsed;
                case FileType.Defect:
                    return this.PathDefect;
                case FileType.User:
                    return this.PathUser;
            }
            return "";
        }
    }
}
