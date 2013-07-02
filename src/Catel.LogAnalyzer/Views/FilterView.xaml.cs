// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterView.xaml.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer.Views
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using AvalonEdit;
    using ICSharpCode.AvalonEdit.Rendering;
    using IoC;

    /// <summary>
    /// Interaction logic for FilterView.xaml.
    /// </summary>
    public partial class FilterView
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterView"/> class.
        /// </summary>
        public FilterView()
        {
            InitializeComponent();

            Observable.FromEventPattern(searchBox, "TextChanged")
                      .DistinctUntilChanged()
                      .Subscribe(change =>
                          {
                              var lineTransformers = ServiceLocator.Default.ResolveType<IList<IVisualLineTransformer>>();

                              var colorizeAvalonEdit = new ColorizeAvalonEdit(searchBox.Text);

                              if (!lineTransformers.Contains(colorizeAvalonEdit))
                              {
                                  lineTransformers.Add(colorizeAvalonEdit);
                              }
                          });
        }
        #endregion
    }
}