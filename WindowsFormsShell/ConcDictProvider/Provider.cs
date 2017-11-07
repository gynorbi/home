using CommonThings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcDictProvider
{
    public class Provider : IProvider
    {
        FileSystemWatcher fsw;
        string path = @"c:\temp\fsw";
        ConcurrentDictionary<string, ConcurrentBag<FileData>> data;

        public int Count { get { return data.Aggregate(0, (count, kvp) => count + kvp.Value.ToList().Count()); } }

        public Provider()
        {
            fsw = new FileSystemWatcher(path);
            fsw.IncludeSubdirectories = true;
            fsw.EnableRaisingEvents = true;
            data = new ConcurrentDictionary<string, ConcurrentBag<FileData>>();
        }

        public async Task ReadAllFiles()
        {
            var allFileInfo = (new DirectoryInfo(path)).EnumerateFiles("*.txt", SearchOption.AllDirectories);
            var tasks = new List<Task<string>>();
            foreach (var p in allFileInfo)
            {
                var content = await FileHelper.ReadTextAsync(p.FullName);
                ProcessResult(p.Name, p.FullName, content);
            }

            var results = await Task.WhenAll(tasks);
        }

        private void ProcessResult(string name, string fullName, string content)
        {
            var newContent = fullName + "\r\n" + content;

            data.AddOrUpdate(name, 
                new ConcurrentBag<FileData>() { new FileData { FullPath = fullName, Content = newContent } },
                (key,existingBag) => {
                    var clonedBag = new ConcurrentBag<FileData>(existingBag);
                    var matchedElement = clonedBag.FirstOrDefault(fd => fd.FullPath == fullName);
                    if (matchedElement != null)
                    {
                        matchedElement.Content = newContent;
                    }
                    else
                    {
                        clonedBag.Add(new FileData { FullPath = fullName, Content = newContent });
                    }
                    return clonedBag;
            });
        }

        public void Clear()
        {
            data.Clear();
        }
    }
}
