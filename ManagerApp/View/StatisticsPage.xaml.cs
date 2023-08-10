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
        private const string BY_MONTH = "Filter by month";
        private const string BY_YEAR = "Filter by year";

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
                case BY_MONTH:
                    yearDatePicker.MonthVisible = true;
                    break;
                case BY_YEAR:
                    yearDatePicker.MonthVisible = false;
                    break;
                default:
                    throw new Exception($"Invalid argument: {option}");
            }
        }
    }
}
