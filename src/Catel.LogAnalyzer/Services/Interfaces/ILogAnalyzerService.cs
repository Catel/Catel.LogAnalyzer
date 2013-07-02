// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogAnalyzerService.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Catel.LogAnalyzer
{
    using System;
    using System.Collections.Generic;

    using Models;

    public interface ILogAnalyzerService
    {
        #region Methods
        IEnumerable<LogEntry> Parse(LogFilter filter, string text);

        IEnumerable<LogEntry> GetSlowestMethods(IEnumerable<LogEntry> entries, int maximumItemsToReturn);

        IEnumerable<LogEntry> Filter(IEnumerable<LogEntry> entries, int maximumItemsToReturn = -1, Func<LogEntry, bool> filter = null, Func<LogEntry, object> orderBy = null, Func<LogEntry, object> orderByDescending = null);
        #endregion
    }
}