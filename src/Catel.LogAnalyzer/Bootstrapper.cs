// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="Catel development team">
//   Copyright (c) 2008 - 2014 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Concurrency;
    using IoC;
    using Logging;
    using MVVM;
    using MVVM.Tasks;
    using MVVM.ViewModels;
    using Reflection;
    using Views;
    using Environment = Catel.Environment;

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
            LogManager.AddDebugListener();
#endif

            var serviceLocator = this.GetDependencyResolver().Resolve<IServiceLocator>();
            Environment.RegisterDefaultViewModelServices();

            var viewLocator = serviceLocator.ResolveType<IViewLocator>();
            viewLocator.Register(typeof (ProgressNotifyableViewModel), typeof (SplashScreen));

            var viewModelLocator = serviceLocator.ResolveType<IViewModelLocator>();
            viewModelLocator.Register(typeof (SplashScreen), typeof (ProgressNotifyableViewModel));
        }
        #endregion

        #region Method Overrides
        /// <summary>
        /// Configures the IoC container.
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterInstance<IScheduler>(TaskPoolScheduler.Default);
            Container.RegisterType<ILogAnalyzerService, LogAnalyzerService>();
            Container.RegisterType<IFileWatcherService, FileWatcherService>();
        }

        /// <summary>
        ///     Initialize boot tasks.
        /// </summary>
        /// <param name="bootTasks">The additional boot tasks.</param>
        /// <remarks>
        /// Override this method to add additional tasks that will be executed before shell initialization.
        /// </remarks>
        protected override void InitializeBootTasks(IList<ITask> bootTasks)
        {
            var scheduler = TaskPoolScheduler.Default;

            bootTasks.Add(new ActionTask("Preloading assemblies", taskProgressTracker => scheduler.Schedule(() => AppDomain.CurrentDomain.PreloadAssemblies())));
        }
        #endregion
    }
}