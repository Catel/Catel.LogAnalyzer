// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellView.xaml.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Catel.LogAnalyzer.Views
{
    using System.Windows;
    using IoC;
    using MVVM.Services;
    using Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class ShellView
    {
        #region Constants
        public static readonly DependencyProperty ShowBusyIndicatorProperty = DependencyProperty.Register("ShowBusyIndicator", typeof (bool), typeof (ShellView), new PropertyMetadata(false));
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellView"/> class.
        /// </summary>
        public ShellView()
            : base(DataWindowMode.Custom, setOwnerAndFocus: false)
        {
            InitializeComponent();

            var serviceLocator = ServiceLocator.Default;

            serviceLocator.RegisterInstance(this);
            serviceLocator.RegisterType<IPleaseWaitService, Services.PleaseWaitService>();
            serviceLocator.RegisterInstance(textEditor.TextArea.TextView.LineTransformers);
        }
        #endregion

        #region Properties
        public bool ShowBusyIndicator
        {
            get { return (bool) GetValue(ShowBusyIndicatorProperty); }
            set { SetValue(ShowBusyIndicatorProperty, value); }
        }
        #endregion
    }
}