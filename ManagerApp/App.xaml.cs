// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using ManagerApp.View;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ManagerApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Microsoft.UI.Xaml.Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow()
            {
                Content = new RootPage()
            };
            //m_window.ExtendsContentIntoTitleBar = true;
            //m_window.SetTitleBar(null);
            m_window.Activate();
            MainRoot = m_window.Content as FrameworkElement;
        }

        private Window m_window;
        public static FrameworkElement MainRoot { get; private set; }
    }
}
