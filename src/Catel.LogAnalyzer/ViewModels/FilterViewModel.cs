using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catel.LogAnalyzer.ViewModels
{
    using MVVM;
    using Models;

    public  class FilterViewModel : ViewModelBase
    {
        public FilterViewModel(LogFilter logFilter)
        {
            Argument.IsNotNull(() => logFilter);

            LogFilter = logFilter;
        }

        [Model]
        [Expose("Filter")]
        [Expose("EnableDebug")]
        [Expose("EnableInfo")]
        [Expose("EnableWarning")]
        [Expose("EnableError")]
        public LogFilter LogFilter { get; private set; }

        //[ViewModelToModel("LogFilter")]
        //public string Filter { get; set; }

    }
}
