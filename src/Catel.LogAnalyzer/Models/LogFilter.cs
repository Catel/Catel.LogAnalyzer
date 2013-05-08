// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilter.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Catel.LogAnalyzer.Models
{
    using Catel.Data;

    public class LogFilter : ModelBase
    {
        #region Properties
        public bool EnableDebug { get; set; }

        public bool EnableInfo { get; set; }

        public bool EnableWarning { get; set; }

        public bool EnableError { get; set; }

        public string Filter { get; set; }
        #endregion
    }
}