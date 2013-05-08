using System;
using System.ComponentModel;
using System.Windows;
using System.Xml;
using Catel.LogAnalyzer.ViewModels;
using Catel.MVVM;
using Catel.Windows.Controls.MVVMProviders.Logic;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace Catel.LogAnalyzer.Views
{
    using Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class ShellView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellView"/> class.
        /// </summary>
        public ShellView()
            : base(DataWindowMode.Custom)
        {
            //IHighlightingDefinition highlightingDefinition;
            //using (var reader = new XmlTextReader("HighlightingDefinition.xshd")) 
            //{
            //    highlightingDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            //}

            //HighlightingManager.Instance.RegisterHighlighting("CatelHighlighting", new[] { ".cool" }, highlightingDefinition);

            InitializeComponent();
        }
        #endregion
    }
}
