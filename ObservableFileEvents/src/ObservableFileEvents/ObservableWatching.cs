using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace ObservableFileEvents
{
    public class ObservableWatching
    {
        FileSystemWatcher fsw;

        public ObservableWatching()
        {
            fsw = new FileSystemWatcher(@"c:\temp\fsw");
            fsw.IncludeSubdirectories = true;
            fsw.EnableRaisingEvents = true;
            var changeEvents = Observable
                .FromEventPattern<FileSystemEventArgs>(fsw, "Changed");
            var createdEvents = Observable
                .FromEventPattern<FileSystemEventArgs>(fsw, "Created");
            var renamedEvents = Observable
                .FromEventPattern<FileSystemEventArgs>(fsw, "Renamed");
            var events = Observable
                .Merge(changeEvents, createdEvents, renamedEvents);
            events.Subscribe(e =>
               {
                   Console.WriteLine($"'{e.EventArgs.Name}' has been {e.EventArgs.ChangeType}");
               });
            Console.WriteLine($"Watching of '{fsw.Path}' has started...");
        }
    }
}
