// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PleaseWaitService.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Catel.LogAnalyzer.Services
{
    using Catel.LogAnalyzer.Views;
    using Catel.MVVM.Services;

    public class PleaseWaitService : Catel.MVVM.Services.PleaseWaitService, IPleaseWaitService
    {
        private readonly ShellView _shellView;

        public PleaseWaitService(ShellView shellView)
        {
            Argument.IsNotNull(() => shellView);

            _shellView = shellView;
        }

        public override void Hide()
        {
            _shellView.ShowBusyIndicator = false;
        }

        public override void Show(string status = "")
        {
            _shellView.ShowBusyIndicator = true;
        }

        public override void Show(PleaseWaitWorkDelegate workDelegate, string status = "")
        {
            _shellView.ShowBusyIndicator = true;
        }

        public override void UpdateStatus(string status)
        {
            _shellView.ShowBusyIndicator = true;
        }

        public override void UpdateStatus(int currentItem, int totalItems, string statusFormat = "")
        {
            _shellView.ShowBusyIndicator = true;
        }
    }
}