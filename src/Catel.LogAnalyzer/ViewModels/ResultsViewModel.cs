// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsViewModel.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer.ViewModels
{
    using System.Collections.Generic;
    using MVVM;

    public class ResultsViewModel : ViewModelBase
    {
        public ResultsViewModel(IEnumerable<LogEntry> logEntries)
        {
            Argument.IsNotNull(() => logEntries);

            LogEntries = logEntries;

            CopyToClipboard = new Command(OnCopyToClipboardExecute);
        }

        public IEnumerable<LogEntry> LogEntries { get; private set; }

        /// <summary>
        /// Gets the CopyToClipboard command.
        /// </summary>
        public Command CopyToClipboard { get; private set; }

        /// <summary>
        /// Method to invoke when the CopyToClipboard command is executed.
        /// </summary>
        private void OnCopyToClipboardExecute()
        {
            // TODO: Handle command logic here
        }
    }
}