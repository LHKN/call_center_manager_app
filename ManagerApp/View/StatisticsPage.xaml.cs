// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ManagerApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        public StatisticsPage()
        {
            this.InitializeComponent();
        }

        private void DateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string option = e.AddedItems[0].ToString();
            yearDatePicker.Visibility = Visibility.Visible;

            switch (option)
            {
                case "Filter by month":
                    yearDatePicker.MonthVisible = true;
                    break;
                case "Filter by year":
                    yearDatePicker.MonthVisible = false;
                    break;
                default:
                    throw new Exception($"Invalid argument: {option}");
            }
        }
    }
}
