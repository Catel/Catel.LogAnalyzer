// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellViewModel.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Xml;
    using Collections;
    using Data;
    using Helpers;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Highlighting;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;
    using Logging;
    using MVVM;
    using MVVM.Services;
    using Models;

    /// <summary>
    /// Shell view model.
    /// </summary>
    public class ShellViewModel : ViewModelBase
    {
        #region Constants
        /// <summary>
        /// Register the IsLiveViewEnabled property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsLiveViewEnabledProperty = RegisterProperty("IsLiveViewEnabled", typeof (bool), null, (sender, args) =>
            {
                var viewModel = sender as ShellViewModel;

                if (viewModel == null)
                {
                    return;
                }

                viewModel.OnIsLiveViewEnabledChanged();
            });
        #endregion

        #region Fields
        private readonly IDispatcherService _dispatcherService;
        private readonly IFileWatcherService _fileWatcherService;
        private readonly ILogAnalyzerService _logAnalyzerService;
        private readonly IMessageService _messageService;
        private readonly IOpenFileService _openFileService;
        private readonly IPleaseWaitService _pleaseWaitService;

        /// <summary>
        /// The trace entries with all trace items.
        /// </summary>
        private FastObservableCollection<LogEntry> _logEntries;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        public ShellViewModel(ILogAnalyzerService logAnalyzerService, IFileWatcherService fileWatcherService, IDispatcherService dispatcherService, IOpenFileService openFileService, IPleaseWaitService pleaseWaitService, IMessageService messageService)
        {
            _logAnalyzerService = logAnalyzerService;
            _fileWatcherService = fileWatcherService;
            _dispatcherService = dispatcherService;
            _openFileService = openFileService;
            _pleaseWaitService = pleaseWaitService;
            _messageService = messageService;

            ParseCommand = new Command(OnParseCommandExecute, OnParseCommandCanExecute);

            LoadFile = new Command<string>(OnLoadFileExecute);

            OpenFileCommand = new Command(OnOpenFileCommandExecute);

            ExitCommand = new Command(OnExitCommandExecute);

            Document = new TextDocument();

            Filter = new LogFilter
                {
                    EnableDebug = true,
                    EnableError = true,
                    EnableInfo = true,
                    EnableWarning = true
                };

            Filter.PropertyChanged += OnFilterPropertyChanged;

            Document.Changed += DocumentChanged;

            _logEntries = new FastObservableCollection<LogEntry>();

            using (var reader = new XmlTextReader("Resources\\HighlightingDefinition.xshd"))
            {
                HighlightingDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }

            HighlightingManager.Instance.RegisterHighlighting("CatelHighlighting", new[] {".cool"}, HighlightingDefinition);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get { return "Catel Log Analyzer"; }
        }

        /// <summary>
        /// Gets the highlighting definition.
        /// </summary>
        /// <value>
        /// The highlighting definition.
        /// </value>
        public IHighlightingDefinition HighlightingDefinition { get; private set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public LogFilter Filter { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsLiveViewEnabled
        {
            get { return GetValue<bool>(IsLiveViewEnabledProperty); }
            set { SetValue(IsLiveViewEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the dropped file.
        /// </summary>
        /// <value>
        /// The dropped file.
        /// </value>
        public string DroppedFile { get; private set; }

        /// <summary>
        /// Gets or sets the top10 slowest methods.
        /// </summary>
        /// <value>
        /// The top10 slowest methods.
        /// </value>
        public FastObservableCollection<LogEntry> Top10SlowestMethods { get; set; }

        /// <summary>
        /// Gets or sets the top10 most common lines.
        /// </summary>
        /// <value>
        /// The top10 most common lines.
        /// </value>
        public FastObservableCollection<LogEntry> Top10MostCommonLines { get; set; }

        /// <summary>
        /// Gets or sets the top10 errors and warnings.
        /// </summary>
        /// <value>
        /// The top10 errors and warnings.
        /// </value>
        public FastObservableCollection<LogEntry> Top10ErrorsAndWarnings { get; set; }

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        public TextDocument Document { get; private set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public IDisposable FileChangesSubscription { get; private set; }
        #endregion

        #region Commands

        #region Properties
        /// <summary>
        /// Gets the Parse command.
        /// </summary>
        public Command ParseCommand { get; private set; }

        /// <summary>
        /// Gets the LoadFile command.
        /// </summary>
        public Command<string> LoadFile { get; private set; }

        /// <summary>
        /// Gets the OpenFileCommand command.
        /// </summary>
        public Command OpenFileCommand { get; private set; }

        /// <summary>
        /// Gets the ExitCommand command.
        /// </summary>
        public Command ExitCommand { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the ExitCommand command is executed.
        /// </summary>
        private void OnExitCommandExecute()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Method to invoke when the OpenFileCommand command is executed.
        /// </summary>
        private void OnOpenFileCommandExecute()
        {
            if (!_openFileService.DetermineFile())
            {
                return;
            }

            var fileName = _openFileService.FileName;

            OnLoadFileExecute(fileName);
        }

        /// <summary>
        /// Method to invoke when the LoadFile command is executed.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private void OnLoadFileExecute(string fileName)
        {
            Argument.IsNotNullOrWhitespace(() => fileName);

            DroppedFile = fileName;

            var fileLines = FileHelper.ReadAllLines(DroppedFile)
                                      .Where(line => !string.IsNullOrWhiteSpace(line));

            var lines = fileLines as string[] ?? fileLines.ToArray();

            if (!lines.Any())
            {
                _messageService.ShowInformation(string.Format("The file '{0}' contains no line.", fileName));

                return;
            }

            _pleaseWaitService.Show("Adding logs, please be patient...");

            var textToAdd = lines.Aggregate((line1, line2) => string.Format("{0}\n{1}", line1, line2))
                                 .Trim();

            if (Document == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(Document.Text))
            {
                Document.Text = textToAdd;
            }
            else
            {
                Document.Text += string.Format("\n{0}", textToAdd);
            }

            if (IsLiveViewEnabled)
            {
                SubscribeToFileChanges();
            }

            _pleaseWaitService.Hide();
        }

        /// <summary>
        /// Subscribes to file changes.
        /// </summary>
        private void SubscribeToFileChanges()
        {
            if (!IsLiveViewEnabled || DroppedFile == null)
            {
                return;
            }

            var fileInfo = new FileInfo(DroppedFile);

            FileChangesSubscription = _fileWatcherService.ObserveFolderChanges(fileInfo.DirectoryName, string.Format("*{0}", fileInfo.Extension), TimeSpan.FromMilliseconds(100))
                                                         .Subscribe(eventArgs => _dispatcherService.BeginInvoke(() =>
                                                             {
                                                                 var fileLines = FileHelper.ReadAllLines(DroppedFile)
                                                                                           .Where(line => !string.IsNullOrWhiteSpace(line));

                                                                 var textToAdd = fileLines.Aggregate((line1, line2) => string.Format("{0}\n{1}", line1, line2)).Trim();

                                                                 if (Document == null)
                                                                 {
                                                                     return;
                                                                 }

                                                                 Document.Text = textToAdd;
                                                             }));
        }

        /// <summary>
        /// Called when live view changed.
        /// </summary>
        private void OnIsLiveViewEnabledChanged()
        {
            if (IsLiveViewEnabled)
            {
                SubscribeToFileChanges();
            }
            else
            {
                if (FileChangesSubscription != null)
                {
                    FileChangesSubscription.Dispose();
                }
            }
        }

        /// <summary>
        /// Method to check whether the ParseCommand command can be executed.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the command can be executed; otherwise <c>false</c>.
        /// </returns>
        private bool OnParseCommandCanExecute()
        {
            return Document != null && !string.IsNullOrWhiteSpace(Document.Text);
        }

        /// <summary>
        /// Method to invoke when the ParseCommand command is executed.
        /// </summary>
        private void OnParseCommandExecute()
        {
            _logEntries = new FastObservableCollection<LogEntry>(_logAnalyzerService.Parse(Filter, Document.Text));

            var top10SlowestMethods = _logEntries.OrderByDescending(log => log.Time).Take(10);

            Top10SlowestMethods = new FastObservableCollection<LogEntry>(top10SlowestMethods);

            var top10MostCommonLines = _logAnalyzerService.Filter(_logEntries, 10, log => log.LogEvent != LogEvent.Error && log.LogEvent != LogEvent.Warning, null, key => key.Time);

            Top10MostCommonLines = new FastObservableCollection<LogEntry>(top10MostCommonLines);

            var top10ErrorsAndWarnings = _logAnalyzerService.Filter(_logEntries, 10, log => log.LogEvent == LogEvent.Error || log.LogEvent == LogEvent.Warning, null, key => key.Time);

            Top10ErrorsAndWarnings = new FastObservableCollection<LogEntry>(top10ErrorsAndWarnings);
        }
        #endregion

        #endregion

        #region Methods
        private void DocumentChanged(object sender, DocumentChangeEventArgs e)
        {
            if (ParseCommand.CanExecute())
            {
                ParseCommand.Execute();
            }
        }

        private void OnFilterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ParseCommand.CanExecute())
            {
                ParseCommand.Execute();
            }
        }
        #endregion
    }
}