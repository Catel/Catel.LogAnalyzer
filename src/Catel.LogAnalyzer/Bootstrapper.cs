using Catel.IoC;
using Catel.LogAnalyzer.Views;
using Catel.Logging;
using Catel.MVVM;
using Catel.MVVM.ViewModels;
using Catel.Windows;
using Catel.Windows.Controls;

namespace Catel.LogAnalyzer
{
    /// <summary>
    ///     The bootstrapper.
    /// </summary>
    public class Bootstrapper : BootstrapperBase<ShellView>
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Bootstrapper" /> class.
        /// </summary>
        public Bootstrapper()
        {
#if DEBUG
            LogManager.RegisterDebugListener();
#endif

            UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;

            StyleHelper.CreateStyleForwardersForDefaultStyles();

            var viewLocator = ServiceLocator.Default.ResolveType<IViewLocator>();
            viewLocator.Register(typeof(ProgressNotifyableViewModel), typeof(SplashScreenView));

            var viewModelLocator = ServiceLocator.Default.ResolveType<IViewModelLocator>();
            viewModelLocator.Register(typeof(SplashScreenView), typeof(ProgressNotifyableViewModel));

        }

        #endregion

        #region Method Overrides

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog.Initialize();
        }

        /// <summary>
        ///     Configures the IoC container.
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<ILogAnalyzerService, LogAnalyzerService>();
        }

        #endregion
    }
}
