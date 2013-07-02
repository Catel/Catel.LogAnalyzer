// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterViewModel.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer.ViewModels
{
    using MVVM;
    using Models;

    public class FilterViewModel : ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterViewModel"/> class.
        /// </summary>
        /// <param name="logFilter">The log filter.</param>
        public FilterViewModel(LogFilter logFilter)
        {
            Argument.IsNotNull(() => logFilter);

            LogFilter = logFilter;
        }
        #endregion

        #region Properties
        [Model]
        [Expose("Filter")]
        [Expose("EnableDebug")]
        [Expose("EnableInfo")]
        [Expose("EnableWarning")]
        [Expose("EnableError")]
        public LogFilter LogFilter { get; private set; }
        #endregion
    }
}