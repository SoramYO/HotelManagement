using ClosedXML.Excel;
using FUMiniHotelDAL.Models;
using Microsoft.Win32;
using StudentNameWPF;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
namespace FUMiniHotelDAL
{


    public class RevenueReportViewModel : INotifyPropertyChanged
    {
        private readonly FuminiHotelManagementContext _context;
        private DateOnly _startDate;
        private DateOnly _endDate;
        private ObservableCollection<RevenueReportItem> _reportItems;
        private decimal _totalRevenue;
        private int _totalBookings;

        public event PropertyChangedEventHandler PropertyChanged;

        public RevenueReportViewModel(FuminiHotelManagementContext context)
        {
            _context = context;
            ReportItems = new ObservableCollection<RevenueReportItem>();
            StartDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1);
            EndDate = StartDate.AddMonths(1).AddDays(-1);
            GenerateReportCommand = new RelayCommand(_ => GenerateReport());
            ExportToExcelCommand = new RelayCommand(_ => ExportToExcel());

            // Generate initial report
            GenerateReport();
        }

        public DateOnly StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public DateOnly EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RevenueReportItem> ReportItems
        {
            get => _reportItems;
            set
            {
                _reportItems = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set
            {
                _totalRevenue = value;
                OnPropertyChanged();
            }
        }

        public int TotalBookings
        {
            get => _totalBookings;
            set
            {
                _totalBookings = value;
                OnPropertyChanged();
            }
        }

        public ICommand GenerateReportCommand { get; }
        public ICommand ExportToExcelCommand { get; }
        public ICommand CloseCommand { get; }

        private void GenerateReport()
        {
            var query = from booking in _context.BookingReservations
                        where booking.BookingDate.HasValue &&
                              booking.BookingDate.Value >= StartDate &&
                              booking.BookingDate.Value <= EndDate
                        group booking by booking.BookingDate.Value into g
                        select new RevenueReportItem
                        {
                            Date = g.Key,
                            BookingCount = g.Count(),
                            Revenue = g.Sum(b => b.TotalPrice ?? 0)
                        };

            var results = query.OrderByDescending(x => x.Revenue).ToList();

            ReportItems.Clear();
            foreach (var item in results)
            {
                ReportItems.Add(item);
            }

            TotalRevenue = results.Sum(x => x.Revenue);
            TotalBookings = results.Sum(x => x.BookingCount);
        }

        private void ExportToExcel()
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FilterIndex = 1,
                    DefaultExt = "xlsx",
                    FileName = $"RevenueReport_{StartDate:yyyy-MM-dd}_to_{EndDate:yyyy-MM-dd}"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Revenue Report");

                        // Add title
                        worksheet.Cell("A1").Value = $"Revenue Report ({StartDate:d} - {EndDate:d})";
                        worksheet.Range("A1:D1").Merge().Style.Font.SetBold();

                        // Add headers
                        worksheet.Cell("A3").Value = "Date";
                        worksheet.Cell("B3").Value = "Number of Bookings";
                        worksheet.Cell("C3").Value = "Total Revenue";
                        worksheet.Cell("D3").Value = "Average Revenue Per Booking";
                        worksheet.Range("A3:D3").Style.Font.SetBold();

                        // Add data
                        var row = 4;
                        foreach (var item in ReportItems)
                        {
                            worksheet.Cell($"A{row}").Value = item.Date.ToString();
                            worksheet.Cell($"B{row}").Value = item.BookingCount;
                            worksheet.Cell($"C{row}").Value = item.Revenue;
                            worksheet.Cell($"D{row}").Value = item.AverageRevenue;
                            row++;
                        }

                        // Add summary
                        worksheet.Cell($"A{row + 1}").Value = "Total:";
                        worksheet.Cell($"B{row + 1}").Value = TotalBookings;
                        worksheet.Cell($"C{row + 1}").Value = TotalRevenue;
                        worksheet.Range($"A{row + 1}:D{row + 1}").Style.Font.SetBold();

                        // Format columns
                        worksheet.Column("A").Style.DateFormat.SetFormat("dd/MM/yyyy");
                        worksheet.Column("C").Style.NumberFormat.SetFormat("$#,##0.00");
                        worksheet.Column("D").Style.NumberFormat.SetFormat("$#,##0.00");

                        // Auto-fit columns
                        worksheet.Columns().AdjustToContents();

                        workbook.SaveAs(saveFileDialog.FileName);
                    }

                    MessageBox.Show("Report exported successfully!", "Export Complete",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Export Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class RevenueReportItem
    {
        public DateOnly Date { get; set; }
        public int BookingCount { get; set; }
        public decimal Revenue { get; set; }
        public decimal AverageRevenue => BookingCount > 0 ? Revenue / BookingCount : 0;
    }
}
