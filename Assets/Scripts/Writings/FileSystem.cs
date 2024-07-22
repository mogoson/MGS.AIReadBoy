/*************************************************************************
 *  Copyright (C) 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  FileSystem.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/22
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections.Generic;
using System.IO;

namespace MGS.Bookboy
{
    public class Entry
    {
        public string path;
        public bool file;
        public ICollection<Entry> children;
    }

    public sealed class FileSystem
    {
        public static ICollection<Entry> GetEntries(string path)
        {
            if (!Directory.Exists(path))
            {
                return null;
            }

            var entries = new List<Entry>();
            var paths = Directory.GetFileSystemEntries(path);
            foreach (var item in paths)
            {
                if (File.Exists(path))
                {
                    var entry = new Entry()
                    {
                        path = item,
                        file = true
                    };
                    entries.Add(entry);
                }
                else if (Directory.Exists(path))
                {
                    var entry = new Entry()
                    {
                        path = item,
                        children = GetEntries(item)
                    };
                    entries.Add(entry);
                }
            }
            return entries;
        }
    }
}