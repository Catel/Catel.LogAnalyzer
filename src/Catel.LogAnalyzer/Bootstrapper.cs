﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Catel.LogAnalyzer
{
    using Catel.IoC;
    using Catel.LogAnalyzer.Views;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.MVVM.ViewModels;
    using Catel.Windows;
    using Catel.Windows.Controls;

    /// <summary>
    /// The bootstrapper.
    /// </summary>
    public class Bootstrapper : BootstrapperBase<ShellView>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
#if DEBUG
            LogManager.RegisterDebugListener();
#endif

            var serviceLocator = ServiceLocator.Default;
            Environment.RegisterDefaultViewModelServices();

            var viewLocator = serviceLocator.ResolveType<IViewLocator>();
            viewLocator.Register(typeof(ProgressNotifyableViewModel), typeof(SplashScreen));

            var viewModelLocator = serviceLocator.ResolveType<IViewModelLocator>();
            viewModelLocator.Register(typeof(SplashScreen), typeof(ProgressNotifyableViewModel));
        }
        #endregion

        #region Method Overrides
        /// <summary>
        /// Configures the IoC container.
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<ILogAnalyzerService, LogAnalyzerService>();
        }
        #endregion
    }
}