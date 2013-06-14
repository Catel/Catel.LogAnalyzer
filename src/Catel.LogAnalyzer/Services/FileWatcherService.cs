// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileWatcherService.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer
{
    using System;
    using System.IO;
    using System.Reactive.Linq;

    public class FileWatcherService : IFileWatcherService
    {
        #region IFileWatcherService Members
        /// <summary>
        /// Observes the folder changes.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="throttle">The throttle.</param>
        /// <returns></returns>
        public IObservable<FileSystemEventArgs> ObserveFolderChanges(string path, string filter, TimeSpan throttle)
        {
            Argument.IsNotNullOrWhitespace(() => path);
            Argument.IsNotNullOrWhitespace(() => filter);
            Argument.IsNotNull(() => throttle);

            return Observable.Create<FileSystemEventArgs>(
                observer =>
                    {
                        var fileSystemWatcher = new FileSystemWatcher(path, filter) {EnableRaisingEvents = true};

                        return Observable.FromEventPattern<FileSystemEventArgs>(fileSystemWatcher, "Changed")
                                         .Select(ev => ev.EventArgs)
                                         .Throttle(throttle)
                                         .Finally(fileSystemWatcher.Dispose)
                                         .Subscribe(observer);
                    }
                );
        }
        #endregion
    }
}