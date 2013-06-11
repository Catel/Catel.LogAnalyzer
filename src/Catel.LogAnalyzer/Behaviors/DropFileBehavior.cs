// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropFileBehavior.cs" company="Catel development team">
//   Copyright (c) 2008 - 2013 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.LogAnalyzer.Behaviors
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// This is an Attached Behavior and is intended for use with
    /// XAML objects to enable binding a drag and drop event to
    /// an ICommand.
    /// </summary>
    public static class DropFileBehavior
    {
        #region Constants
        /// <summary>
        /// The Dependency property. To allow for Binding, a dependency
        /// property must be used.
        /// </summary>
        private static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached
                (
                    "Command",
                    typeof (ICommand),
                    typeof (DropFileBehavior),
                    new PropertyMetadata(CommandPropertyChangedCallBack)
                );
        #endregion

        #region Methods
        /// <summary>
        /// The setter. This sets the value of the PreviewDropCommandProperty
        /// Dependency Property. It is expected that you use this only in XAML
        ///
        /// This appears in XAML with the "Set" stripped off.
        /// XAML usage:
        ///
        /// <Grid mvvm:DropFileHelper.Command="{Binding DropCommand}" />
        ///
        /// </summary>
        /// <param name="inUiElement">A UIElement object. In XAML this is automatically passed
        /// in, so you don't have to enter anything in XAML.</param>
        /// <param name="inCommand">An object that implements ICommand.</param>
        public static void SetCommand(this UIElement inUiElement, ICommand inCommand)
        {
            inUiElement.SetValue(CommandProperty, inCommand);
        }

        /// <summary>
        /// Gets the PreviewDropCommand assigned to the PreviewDropCommandProperty
        /// DependencyProperty. As this is only needed by this class, it is private.
        /// </summary>
        /// <param name="inUiElement">A UIElement object.</param>
        /// <returns>An object that implements ICommand.</returns>
        private static ICommand GetCommand(UIElement inUiElement)
        {
            Argument.IsNotNull(() => inUiElement);

            return (ICommand) inUiElement.GetValue(CommandProperty);
        }

        /// <summary>
        /// The OnCommandChanged method. This event handles the initial binding and future
        /// binding changes to the bound ICommand
        /// </summary>
        /// <param name="inDependencyObject">A DependencyObject</param>
        /// <param name="inEventArgs">A DependencyPropertyChangedEventArgs object.</param>
        private static void CommandPropertyChangedCallBack(
            DependencyObject inDependencyObject, DependencyPropertyChangedEventArgs inEventArgs)
        {
            var uiElement = inDependencyObject as UIElement; // Remove the handler if it exist to avoid memory leaks
            if (uiElement != null)
            {
                uiElement.Drop -= UIElement_Drop;
            }

            var command = inEventArgs.NewValue as ICommand;
            if (command == null)
            {
                return;
            }
            // the property is attached so we attach the Drop event handler
            if (uiElement != null)
            {
                uiElement.Drop += UIElement_Drop;
            }
        }

        private static void UIElement_Drop(object sender, DragEventArgs e)
        {
            var uiElement = sender as UIElement;

            // Sanity check just in case this was somehow send by something else
            if (uiElement == null)
            {
                return;
            }

            var dropFileCommand = GetCommand(uiElement);

            // There may not be a command bound to this after all
            if (dropFileCommand == null)
            {
                return;
            }

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            var droppedFilePaths =
                e.Data.GetData(DataFormats.FileDrop, true) as string[];

            if (droppedFilePaths == null)
            {
                return;
            }

            foreach (var droppedFilePath in droppedFilePaths)
            {
                // Check whether this attached behaviour is bound to a RoutedCommand
                if (dropFileCommand is RoutedCommand)
                {
                    // Execute the routed command
                    (dropFileCommand as RoutedCommand).Execute(droppedFilePath, uiElement);
                }
                else
                {
                    // Execute the Command as bound delegate
                    dropFileCommand.Execute(droppedFilePath);
                }
            }
        }
        #endregion
    }
}