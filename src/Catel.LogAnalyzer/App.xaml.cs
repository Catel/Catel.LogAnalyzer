// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Catel.LogAnalyzer
{
    using Catel.MVVM.ViewModels;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App
    {
        #region Constructors
        public App()
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.RunWithSplashScreen<ProgressNotifyableViewModel>();
        }
        #endregion
    }
}