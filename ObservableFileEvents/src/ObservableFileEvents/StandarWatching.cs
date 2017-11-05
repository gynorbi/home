using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ObservableFileEvents
{
    public class StandarWatching
    {
        FileSystemWatcher fsw;

        public StandarWatching()
        {
            fsw = new FileSystemWatcher(@"c:\temp\fsw");
            fsw.Changed += Fsw_Changed;
            fsw.Created += Fsw_Changed;
            fsw.Renamed += Fsw_Changed;
            fsw.IncludeSubdirectories = true;
            fsw.EnableRaisingEvents = true;
            Console.WriteLine($"Watching of '{fsw.Path}' has started...");

        }

        private void Fsw_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            Console.WriteLine($"'{e.Name}' has been {e.ChangeType}");
        }
    }
}
