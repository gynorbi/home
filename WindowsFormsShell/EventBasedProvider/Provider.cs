using CommonThings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBasedProvider
{
    public class Provider
    {
        FileSystemWatcher fsw;
        string path = @"c:\temp\fsw";
        IDictionary<string, List<FileData>> data;

        public int Count { get { return data.Aggregate(0, (count, kvp) => count + kvp.Value.Count); } }

        public Provider()
        {
            fsw = new FileSystemWatcher(path);
            fsw.IncludeSubdirectories = true;
            fsw.EnableRaisingEvents = true;
            data = new Dictionary<string, List<FileData>>();
        }

        public async Task ReadAllFiles()
        {
            var allFileInfo = (new DirectoryInfo(path)).EnumerateFiles("*.txt", SearchOption.AllDirectories);
            var tasks = new List<Task<string>>();
            foreach (var p in allFileInfo)
            {
                var content = await ReadTextAsync(p.FullName);
                ProcessResult(p.Name, p.FullName, content);
            }

            var results = await Task.WhenAll(tasks);
        }

        private void ProcessResult(string name, string fullName, string content)
        {
            if (data.ContainsKey(name))
            {
                var matchingFileData = data[name].FirstOrDefault(fd => fd.FullPath == fullName);
                if (matchingFileData != null)
                {
                    matchingFileData.Content = content;
                }
                else
                {
                    data[name].Add(new FileData { FullPath = fullName, Content = content });
                }
            }
            else
            {
                data.Add(name, new List<FileData>() { new FileData { FullPath = fullName, Content = content } });
            }
        }

        private async Task<string> ReadTextAsync(string filePath)
        {
            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                StringBuilder sb = new StringBuilder();

                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                return sb.ToString();
            }
        }
    }
}
