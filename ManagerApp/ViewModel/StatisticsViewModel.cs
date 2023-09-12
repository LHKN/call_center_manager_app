﻿using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore;
using Microsoft.UI.Xaml;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LiveChartsCore.SkiaSharpView;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using HarfBuzzSharp;
using System.Threading.Tasks;

namespace ManagerApp.ViewModel
{
    partial class StatisticsViewModel : ViewModelBase
    {
        private const string BY_MONTH = "Filter by month";
        private const string BY_YEAR = "Filter by year";

        // fields
        private int _customerCount;
        private int _driverCount;
        private double _tripPercentage;

        private ObservableCollection<string> _cbOptions;
        private string _selectedOption;

        private List<string> _monthLabels;
        private List<string> _yearLabels;

        private IStatisticsRepository _statisticsRepository;

        // constructor
        public StatisticsViewModel() { 
        
            //Initial info
            _statisticsRepository = new StatisticsRepository();

            _cbOptions = new ObservableCollection<string>
            {
                BY_MONTH,
                BY_YEAR             
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
        private async void Load_Statistics(RoutedEventArgs e)
        {
            // real-time update
            while (true)
            {
                await Task.Run(() =>
                {
                    var task = _statisticsRepository.GetCustomerCount();
                    _customerCount = task.Result;

                    task = _statisticsRepository.GetDriverCount();
                    _driverCount = task.Result;

                    var task2 = _statisticsRepository.GetTripPercentage();
                    _tripPercentage = task2.Result;
                });
            }
        }

        private async void FilterByMonth()
        {
            // total income of the month
            int month = 1;
            int year = 1;

            var weeksTotalIncome = await _statisticsRepository.GetIncomeByMonth(month, year);
            
            if (weeksTotalIncome == null)
            {
                weeksTotalIncome = new List<Tuple<string, int>>
                {
                    new Tuple<string, int>("Week 1", 10),
                    new Tuple<string, int>("Week 2", 220),
                    new Tuple<string, int>("Week 3", 100),
                    new Tuple<string, int>("Week 4", 300)
                };
            }

            XAxes[0].Labels = _monthLabels;
            XAxes[0].Name = "Total income of this month";

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

        private async void FilterByYear()
        {
            int year = 1;

            var yearsTotalIncome = await _statisticsRepository.GetIncomeByYear(year);

            if(yearsTotalIncome == null) 
            { 
                yearsTotalIncome = new List<Tuple<string, int>>
                {
                    new Tuple<string, int>("JAN", 0),
                    new Tuple<string, int>("FEB", 0),
                    new Tuple<string, int>("MAR", 0),
                    new Tuple<string, int>("APR", 0),
                    new Tuple<string, int>("MAY", 0),
                    new Tuple<string, int>("JUN", 0),
                    new Tuple<string, int>("JUL", 0 ),
                    new Tuple<string, int>("AUG", 0),
                    new Tuple<string, int>("SEP", 0),
                    new Tuple<string, int>("OCT", 0),
                    new Tuple<string, int>("NOV", 0),
                    new Tuple<string, int>("DEC", 0)
                };
            }

            XAxes[0].Labels = _yearLabels;
            XAxes[0].Name = "Total income of this year";

            IncomeSeries = new List<ISeries>
            {
                new ColumnSeries<Tuple<string, int>>
                {
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
                    Values = yearsTotalIncome,

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
                switch (SelectedOption)
                {
                    case BY_MONTH:
                        FilterByMonth();
                        break;
                    case BY_YEAR:
                        FilterByYear();
                        break;
                    default:
                        throw new Exception($"Invalid argument: {SelectedOption}");
                }
                OnPropertyChanged(nameof(SelectedOption));
            }
        }

        public DateTime? SelectedTime { get; set; }

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
        
        public event PropertyChangedEventHandler PropertyChanged;

        // commands
        public ICommand Load_Page { get; set; }

    }
}
