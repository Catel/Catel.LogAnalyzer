// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellView.xaml.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer.Views
{
    using IoC;
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
            : base(DataWindowMode.Custom, setOwnerAndFocus: false)
        {
            InitializeComponent();

            ServiceLocator.Default.RegisterInstance(textEditor.TextArea.TextView.LineTransformers);
        }
        #endregion
    }
}