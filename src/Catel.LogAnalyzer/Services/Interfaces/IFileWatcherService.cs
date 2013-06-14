// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileWatcherService.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer
{
    using System;
    using System.IO;

    public interface IFileWatcherService
    {
        #region Methods
        /// <summary>
        /// Observes the folder changes.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="throttle">The throttle.</param>
        /// <returns></returns>
        IObservable<FileSystemEventArgs> ObserveFolderChanges(string path, string filter, TimeSpan throttle);
        #endregion
    }
}