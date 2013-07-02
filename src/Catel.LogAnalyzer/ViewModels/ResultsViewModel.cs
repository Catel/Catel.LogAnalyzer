// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsViewModel.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using Collections;
    using Logging;
    using MVVM;

    public class ResultsViewModel : ViewModelBase
    {
        #region Fields
        private readonly IEnumerable<LogEvent> _logEvents = Enum<LogEvent>.GetValues().OrderBy(value => value).ToArray();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsViewModel"/> class.
        /// </summary>
        /// <param name="logEntries">The log entries.</param>
        public ResultsViewModel(IEnumerable<LogEntry> logEntries)
        {
            Argument.IsNotNull(() => logEntries);

            LogEntries = new FastObservableCollection<LogEntry>(logEntries);

            CopyToClipboard = new Command(OnCopyToClipboardExecute, OnCopyToClipboardCanExecute);
        }
        #endregion

        #region Properties
        public FastObservableCollection<LogEntry> LogEntries { get; private set; }

        /// <summary>
        /// Gets the CopyToClipboard command.
        /// </summary>
        public Command CopyToClipboard { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the CopyToClipboard command is executed.
        /// </summary>
        private void OnCopyToClipboardExecute()
        {
            var text = LogEntriesToString(LogEntries);

            if (!string.IsNullOrEmpty(text))
            {
                Clipboard.SetText(text, TextDataFormat.Text);
            }
        }

        private bool OnCopyToClipboardCanExecute()
        {
            return LogEntries != null && LogEntries.Any();
        }

        /// <summary>
        /// Converts a list of trace entries to a string.
        /// </summary>
        /// <param name="entries">
        /// The entries.
        /// </param>
        /// <returns>
        /// STring representing the trace entries.
        /// </returns>
        private string LogEntriesToString(IEnumerable<LogEntry> entries)
        {
            const string columnText = " | ";

            var maxTypeLength = _logEvents.Max(c => c.ToString("G").Length);
            var stringBuilder = new StringBuilder();

            var regexMultiline = new Regex(@"(?<=(^|\n)).*", RegexOptions.Multiline | RegexOptions.Compiled);

            if (entries != null)
            {
                foreach (var entry in entries)
                {
                    var date = entry.Time.ToString(CultureInfo.CurrentUICulture.ToString());
                    var type = entry.LogEvent.ToString("G").PadRight(maxTypeLength, ' ');
                    var datefiller = new String(' ', date.Length);
                    var typefiller = new String(' ', type.Length);

                    var message = entry.Message;
                    var matches = regexMultiline.Matches(message);

                    if (matches.Count <= 0)
                    {
                        continue;
                    }
                    stringBuilder.AppendFormat("{0}{4}{1}{4}{2}{3}", date, type, matches[0].Value, Environment.NewLine, columnText);

                    if (matches.Count <= 1)
                    {
                        continue;
                    }
                    for (int idx = 1, max = matches.Count; idx < max; idx++)
                    {
                        stringBuilder.AppendFormat("{0}{4}{1}{4}{2}{3}", datefiller, typefiller, matches[idx].Value, Environment.NewLine, columnText);
                    }
                }
            }

            return stringBuilder.ToString();
        }
        #endregion
    }
}