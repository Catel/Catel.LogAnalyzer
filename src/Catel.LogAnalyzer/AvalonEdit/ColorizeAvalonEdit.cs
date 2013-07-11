// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorizeAvalonEdit.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer.AvalonEdit
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Rendering;

    public class ColorizeAvalonEdit : DocumentColorizingTransformer
    {
        #region Fields
        private readonly string _textToHighlight;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorizeAvalonEdit"/> class.
        /// </summary>
        /// <param name="textToHighlight">The text to highlight.</param>
        public ColorizeAvalonEdit(string textToHighlight)
        {
            _textToHighlight = textToHighlight;
        }
        #endregion

        #region Methods
        protected override void ColorizeLine(DocumentLine line)
        {
            var lineStartOffset = line.Offset;
            var text = CurrentContext.Document.GetText(line);
            var start = 0;
            int index;
            var endOffset = _textToHighlight.ToCharArray().Length;

            try
            {
                while ((index = text.IndexOf(_textToHighlight, start, StringComparison.Ordinal)) >= 0)
                {
                    ChangeLinePart(
                        lineStartOffset + index,
                        lineStartOffset + index + endOffset,
                        element =>
                        {
                            var typeface = element.TextRunProperties.Typeface;

                            element.TextRunProperties.SetTypeface(new Typeface(
                                                                      typeface.FontFamily,
                                                                      FontStyles.Normal,
                                                                      FontWeights.UltraBold,
                                                                      typeface.Stretch
                                                                      ));
                        });
                    start = index + 1;
                }
            }
            catch (Exception)
            {
                //Swallow the exception
            }
            
        }
        #endregion
    }
}