using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore;
using ManagerApp.Model;
using Microsoft.UI.Xaml;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LiveChartsCore.SkiaSharpView;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace ManagerApp.ViewModel
{
    partial class StatisticsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // fields
        private int _customerCount;
        private int _driverCount;
        private double _tripPercentage;

        private ObservableCollection<string> _cbOptions;
        private string _selectedOption;

        private List<string> _monthLabels;
        private List<string> _yearLabels;

        [Obsolete]
        // constructor
        public StatisticsViewModel() { 
        
            //Initial info
            _customerCount = 0;
            _driverCount = 0;
            _tripPercentage = 0;

            _cbOptions = new ObservableCollection<string>
            {
                "Filter by month",
                "Filter by year"             
            };
            
            _monthLabels = new List<string>
            {
                "Week 1","Week 2","Week 3","Week 4",
            };

            _yearLabels = new List<string>
            {
                "JAN",
                "FEB",
                "MAR",
                "APR",
                "MAY",
                "JUN",
                "JUL",
                "AUG",
                "SEP",
                "OCT",
                "NOV",
                "DEC"
            };

            // commands
            Load_Page = new RelayCommand<RoutedEventArgs>(Load_Statistics);
        }

        // execute commands

        // total income of the month/year vvv

        private async void Load_Statistics(RoutedEventArgs e)
        {
            // total income of the month

            int month = 1;

            var weeksTotalIncome = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("Week 1", 10),
                new Tuple<string, int>("Week 2", 220),
                new Tuple<string, int>("Week 3", 100),
                new Tuple<string, int>("Week 4", 300)
            };

            List<string> labels = new List<string>();

            weeksTotalIncome.ForEach(week =>
            {
                labels.Add(week.Item1);
            });

            XAxes[0].Labels = labels;
            IncomeSeries = new List<ISeries>
            {
                new ColumnSeries<Tuple<string, int>>
                {
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
                    Values = weeksTotalIncome,

                    Fill = new SolidColorPaint(SKColors.Blue),

                    Mapping = (taskItem, point) =>
                    {
                        point.PrimaryValue = (int)taskItem.Item2;
                        point.SecondaryValue = point.Index;
                    },
                    TooltipLabelFormatter = point => $"{point.Model.Item1.ToString()}: {point.PrimaryValue.ToString()}"
                }
            };
        }

        //private async void Load_Statistics(RoutedEventArgs e)
        //{
        //    //top 5 best selling books of the year
        //    month = 1;
        //    year_month_day = new StringBuilder().Append(year).Append(seperator).Append(month).Append(seperator).Append(day).ToString();
        //    DateTime startYearlyDate = DateTime.Parse(year_month_day);
        //    var top5YearlyBook = await _statisticRepository.GetTop5ProductStatistic(startYearlyDate.Date, DateTimeOffset.Now.Date);

        //    if (top5YearlyBook == null)
        //    {
        //        top5YearlyBook = new List<Tuple<string, int>>();
        //        top5YearlyBook.Add(new Tuple<string, int>("Book 1", 0));
        //        top5YearlyBook.Add(new Tuple<string, int>("Book 2", 0));
        //        top5YearlyBook.Add(new Tuple<string, int>("Book 3", 0));
        //        top5YearlyBook.Add(new Tuple<string, int>("Book 4", 0));
        //        top5YearlyBook.Add(new Tuple<string, int>("Book 5", 0));
        //    }

        //    List<string> labels = new List<string>();

        //    top5YearlyBook.ForEach(book =>
        //    {
        //        labels.Add(book.Item1);
        //    });

        //    YearlyXAxes[0].Labels = labels;
        //    TopYearlyBestSellerSeries = new List<ISeries>();

        //    TopYearlyBestSellerSeries.Add(new ColumnSeries<Tuple<string, int>>
        //    {
        //        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
        //        Values = top5YearlyBook,

        //        Fill = new SolidColorPaint(SKColors.Blue),

        //        Mapping = (taskItem, point) =>
        //        {
        //            point.PrimaryValue = (int)taskItem.Item2;
        //            point.SecondaryValue = point.Context.Index;
        //        },
        //        TooltipLabelFormatter = point => $"{point.Model.Item1.ToString()}: {point.PrimaryValue.ToString()}"
        //    });
        //}

        // getters, setters
        public int CustomerCount { get => _customerCount; set => _customerCount = value; }
        public int DriverCount { get => _driverCount; set => _driverCount = value; }
        public double TripPercentage { get => _tripPercentage; set => _tripPercentage = value; }
        public string SelectedOption 
        { 
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                //OnPropertyChanged(nameof(SelectedOption));
            }
        }

        public List<ISeries> IncomeSeries { get; set; }

        public Axis[] XAxes { get; set; } =
        {
            new Axis
            {
                Name = "Total income of this month/year",
                NameTextSize=15,
                Labels = null,
                TextSize=10
            }
        };

        public Axis[] YAxes { get; set; } =
        {
            new Axis
            {
                Name = "",
                NamePadding = new LiveChartsCore.Drawing.Padding(0, 15),

                LabelsPaint = new SolidColorPaint
                {
                    Color = SKColors.CadetBlue,
                    FontFamily = "Times New Roman",
                    SKFontStyle = new SKFontStyle(SKFontStyleWeight.ExtraBold, SKFontStyleWidth.Normal, SKFontStyleSlant.Italic)
                },
            }
        };

        public ObservableCollection<string> CBOptions { get => _cbOptions; set => _cbOptions = value; }
        public List<string> MonthLabels { get => _monthLabels; set => _monthLabels = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        // commands
        public ICommand Load_Page { get; set; }

    }
}
