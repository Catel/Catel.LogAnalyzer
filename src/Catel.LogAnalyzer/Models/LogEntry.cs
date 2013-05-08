// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEntry.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Catel.LogAnalyzer
{
    using System;

    using Catel.Data;
    using Catel.Logging;

    public class LogEntry : ModelBase
    {
        // Compared to previous log message
        #region Properties
        public TimeSpan Duration { get; set; }

        public TimeSpan Time { get; set; }

        public LogEvent LogEvent { get; set; }

        public string Message { get; set; }
        #endregion
    }
}