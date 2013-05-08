// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellView.xaml.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Catel.LogAnalyzer.Views
{
    using System.Xml;

    using Catel.Windows;

    using ICSharpCode.AvalonEdit.Highlighting;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;

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
            : base(DataWindowMode.Custom, setOwnerAndFocus: false)
        {
            IHighlightingDefinition highlightingDefinition;
            using (var reader = new XmlTextReader("Resources\\HighlightingDefinition.xshd"))
            {
                highlightingDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }

            HighlightingManager.Instance.RegisterHighlighting("CatelHighlighting", new[] { ".cool" }, highlightingDefinition);

            InitializeComponent();
        }
        #endregion
    }
}