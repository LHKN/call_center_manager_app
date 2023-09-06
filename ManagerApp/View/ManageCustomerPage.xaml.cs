// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ManagerApp.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ManagerApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManageCustomerPage : Page
    {
        public ManageCustomerPage()
        {
            this.InitializeComponent();

            // Attach SizeChanged event handler
            dataGrid.SizeChanged += DataGrid_SizeChanged;

            // Add data to DataGrid (sample data)

            ManageCustomerViewModel viewModel = (ManageCustomerViewModel)this.DataContext;

            dataGrid.ItemsSource = viewModel.DisplayCustomerCollection; // Replace with the actual data source

        }

        private void DataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Reset row width when column is resized
            dataGrid.UpdateLayout();
        }
    }
}
