// ------------------------------------------------------------------------------------------------
//  <copyright file="ShellViewModel.cs" Company = "MUCODEC">
//    Copyright (c) 1987 - 2013 MUCODEC. All rights reserved.
//  </copyright>
//  ------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Catel.Collections;
using Catel.Data;
using Catel.Logging;
using Catel.MVVM;
using ICSharpCode.AvalonEdit.Document;

namespace Catel.LogAnalyzer.ViewModels
{
    /// <summary>
    ///     Shell view model.
    /// </summary>
    public class ShellViewModel : ViewModelBase
    {
        #region Fields

        private readonly ILogAnalyzerService _logAnalyzerService;

        /// <summary>
        ///     Register the AvailableTraceLevels property so it is known in the class.
        /// </summary>
        public static readonly PropertyData AvailableTraceLevelsProperty = RegisterProperty("AvailableTraceLevels",
                                                                                            typeof (
                                                                                                FastObservableCollection
                                                                                                <LogEvent>),
                                                                                            () =>
                                                                                            new FastObservableCollection
                                                                                                <LogEvent>(
                                                                                                Enum<LogEvent>
                                                                                                    .GetValues()
                                                                                                    .OrderBy(
                                                                                                        value => value)
                                                                                                    .ToArray()));

        /// <summary>
        ///     Register the SelectedTraceLevel property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SelectedLogEventProperty = RegisterProperty("SelectedLogEvent",
                                                                                        typeof (LogEvent),
                                                                                        LogEvent.Debug,
                                                                                        (sender, e) => { });

        /// <summary>
        ///     The trace entries with all trace items.
        /// </summary>
        private FastObservableCollection<LogEntry> _logEntries;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShellViewModel" /> class.
        /// </summary>
        public ShellViewModel(ILogAnalyzerService logAnalyzerService)
        {
            Argument.IsNotNull(() => logAnalyzerService);

            _logAnalyzerService = logAnalyzerService;

            ParseCommand = new Command(OnParseCommandExecute, OnParseCommandCanExecute);

            CopyTopTenSlowestToClipboard = new Command(OnCopyTopTenSlowestToClipboardExecute,
                                                       OnCopyTopTenSlowestToClipboardCanExecute);

            SelectedLogEvent = AvailableLogEvents.FirstOrDefault();

            Document = new TextDocument();

            _logEntries = new FastObservableCollection<LogEntry>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return "Catel Log Analyzer"; }
        }

        /// <summary>
        ///     Gets or sets the available trace levels.
        /// </summary>
        public FastObservableCollection<LogEvent> AvailableLogEvents
        {
            get { return GetValue<FastObservableCollection<LogEvent>>(AvailableTraceLevelsProperty); }
            set { SetValue(AvailableTraceLevelsProperty, value); }
        }

        public FastObservableCollection<LogEntry> Top10SlowestMethods { get; set; }

        public FastObservableCollection<LogEntry> Top10MostCommonLines { get; set; }

        public FastObservableCollection<LogEntry> Top10ErrorsAndWarnings { get; set; }

        /// <summary>
        ///     Gets or sets the selected log event.
        /// </summary>
        /// <value>
        ///     The selected log event.
        /// </value>
        public LogEvent SelectedLogEvent
        {
            get { return GetValue<LogEvent>(SelectedLogEventProperty); }
            set { SetValue(SelectedLogEventProperty, value); }
        }

        public TextDocument Document { get; set; }

        #endregion

        #region Commands

        /// <summary>
        ///     Gets the ParseCommand command.
        /// </summary>
        public Command ParseCommand { get; private set; }

        /// <summary>
        ///     Gets the CopyToClipboard command.
        /// </summary>
        public Command CopyTopTenSlowestToClipboard { get; private set; }

        /// <summary>
        ///     Method to check whether the ParseCommand command can be executed.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the command can be executed; otherwise <c>false</c>
        /// </returns>
        private bool OnParseCommandCanExecute()
        {
            return Document != null && !string.IsNullOrWhiteSpace(Document.Text);
        }

        /// <summary>
        ///     Method to invoke when the ParseCommand command is executed.
        /// </summary>
        private void OnParseCommandExecute()
        {
            _logEntries =
                new FastObservableCollection<LogEntry>(_logAnalyzerService.Parse(SelectedLogEvent, Document.Text));

            var top10SlowestMethods = _logEntries.OrderByDescending(log => log.Time).Take(10);

            Top10SlowestMethods = new FastObservableCollection<LogEntry>(top10SlowestMethods);

            var top10MostCommonLines = _logAnalyzerService.Filter(_logEntries, 10,
                                                                  log =>
                                                                  log.LogEvent != LogEvent.Error &&
                                                                  log.LogEvent != LogEvent.Warning, null,
                                                                  key => key.Time);

            Top10MostCommonLines = new FastObservableCollection<LogEntry>(top10MostCommonLines);

            var top10ErrorsAndWarnings = _logAnalyzerService.Filter(_logEntries, 10,
                                                                    log =>
                                                                    log.LogEvent == LogEvent.Error ||
                                                                    log.LogEvent == LogEvent.Warning,
                                                                    null, key => key.Time);

            Top10ErrorsAndWarnings = new FastObservableCollection<LogEntry>(top10ErrorsAndWarnings);
        }

        /// <summary>
        ///     Method to check whether the CopyToClipboard command can be executed.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the command can be executed; otherwise <c>false</c>
        /// </returns>
        private bool OnCopyTopTenSlowestToClipboardCanExecute()
        {
            //if (SelectedTraceEntryCollection == null)
            //{
            //    return false;
            //}

            //if (SelectedTraceEntryCollection.Count == 0)
            //{
            //    return false;
            //}

            return true;
        }

        /// <summary>
        ///     Method to invoke when the CopyToClipboard command is executed.
        /// </summary>
        private void OnCopyTopTenSlowestToClipboardExecute()
        {
            //var text = LogEntriesToString(SelectedTraceEntryCollection);
            //if (!string.IsNullOrEmpty(text))
            //{
            //    Clipboard.SetText(text, TextDataFormat.Text);
            //}
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Converts a list of trace entries to a string.
        /// </summary>
        /// <param name="entries">The entries.</param>
        /// <returns>STring representing the trace entries.</returns>
        private string LogEntriesToString(IEnumerable<LogEvent> entries)
        {
            const string columnText = " | ";

            //int maxTypeLength = AvailableTraceLevels.Max(c => c.ToString("G").Length);
            var rv = new StringBuilder();
            //var rxMultiline = new Regex(@"(?<=(^|\n)).*", RegexOptions.Multiline | RegexOptions.Compiled);

            //if (entries != null)
            //{
            //    foreach (var entry in entries)
            //    {
            //        string date = entry.Time.ToString(CultureInfo.CurrentUICulture);
            //        string type = entry.TraceLevel.ToString("G").PadRight(maxTypeLength, ' ');
            //        string datefiller = new String(' ', date.Length);
            //        string typefiller = new String(' ', type.Length);

            //        string message = entry.Message;
            //        var matches = rxMultiline.Matches(message);

            //        if (matches.Count > 0)
            //        {
            //            rv.AppendFormat("{0}{4}{1}{4}{2}{3}", date, type, matches[0].Value, System.Environment.NewLine, columnText);

            //            if (matches.Count > 1)
            //            {
            //                for (int idx = 1, max = matches.Count; idx < max; idx++)
            //                {
            //                    rv.AppendFormat("{0}{4}{1}{4}{2}{3}", datefiller, typefiller, matches[idx].Value, System.Environment.NewLine, columnText);
            //                }
            //            }
            //        }
            //    }
            //}

            return rv.ToString();
        }

        /// <summary>
        ///     Updates the trace level and rebuilds the trace list.
        /// </summary>
        private void UpdateTraceLevel()
        {
            //TraceEntryCollection.Clear();

            //if (SelectedTraceEntryCollection != null)
            //{
            //    SelectedTraceEntryCollection.Clear();
            //}

            //foreach (var entry in _traceEntries)
            //{
            //    if (EntryMatchesLevel(entry))
            //    {
            //        TraceEntryCollection.Add(entry);
            //    }
            //}
        }

        /// <summary>
        ///     Determines if the given entry matches the filter log event.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        /// <returns>
        ///     true if matches of if filter is 'Off', false if not
        /// </returns>
        private bool EntryMatches(LogEntry logEntry)
        {
            return (logEntry.LogEvent <= SelectedLogEvent);
        }

        #endregion
    }
}