﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OutputTypeConverter.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer.Converters
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using Logging;
    using Windows.Data.Converters;

    /// <summary>
    /// Converts an output type to a status that is displayable to the user.
    /// </summary>
#if NET
    [ValueConversion(typeof(LogEvent), typeof(Image))]
#endif
    public class OutputTypeConverter : ValueConverterBase
    {
        #region Constants
        /// <summary>
        /// Assembly name of the assembly containing the images.
        /// </summary>
        private static readonly string AssemblyName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly.FullName;
        #endregion

        #region Fields
        private static BitmapImage _debugImage;
        private static BitmapImage _errorImage;
        private static BitmapImage _infoImage;
        private static BitmapImage _warningImage;
        #endregion

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        protected override object Convert(object value, Type targetType, object parameter)
        {
            Image image = null;

            if (!(value is LogEvent))
            {
                return image;
            }

            var logEvent = (LogEvent)value;
            switch (logEvent)
            {
                case LogEvent.Debug:
                    if (_debugImage == null)
                    {
                        _debugImage = new BitmapImage(new Uri(string.Format("/{0};component/Resources/Images/Debug.png", AssemblyName), UriKind.RelativeOrAbsolute));
                    }

                    image = new Image { Source = _debugImage };
                    break;

                case LogEvent.Error:
                    if (_errorImage == null)
                    {
                        _errorImage = new BitmapImage(new Uri(string.Format("/{0};component/Resources/Images/Error.png", AssemblyName), UriKind.RelativeOrAbsolute));
                    }

                    image = new Image {Source = _errorImage};
                    break;

                case LogEvent.Info:
                    if (_infoImage == null)
                    {
                        _infoImage = new BitmapImage(new Uri(string.Format("/{0};component/Resources/Images/Info.png", AssemblyName), UriKind.RelativeOrAbsolute));
                    }

                    image = new Image { Source = _infoImage };
                    break;

                case LogEvent.Warning:
                    if (_warningImage == null)
                    {
                        _warningImage = new BitmapImage(new Uri(string.Format("/{0};component/Resources/Images/Warning.png", AssemblyName), UriKind.RelativeOrAbsolute));
                    }

                    image = new Image {Source = _warningImage};
                    break;
            }

            if (image != null)
            {
                //// Or hardcoded on 16 x 16?
                //Binding b = new Binding("Width");
                //b.Source = image.Source;
                //image.SetBinding(Image.MaxWidthProperty, b);
                image.MaxWidth = 16;

                //b = new Binding("Height");
                //b.Source = image.Source;
                //image.SetBinding(Image.MaxHeightProperty, b);
                image.MaxHeight = 16;
            }

            return image;
        }
    }
}