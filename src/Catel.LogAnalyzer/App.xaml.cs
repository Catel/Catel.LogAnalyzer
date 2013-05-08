using System.Windows;
using Catel.MVVM.ViewModels;

namespace Catel.LogAnalyzer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
             var bootstrapper = new Bootstrapper();
            bootstrapper.RunWithSplashScreen<ProgressNotifyableViewModel>();           
        }
    }
}