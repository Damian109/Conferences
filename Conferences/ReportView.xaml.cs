using System;
using System.Windows;
using Conferences.bdWork;
using Conferences.modelView;

namespace Conferences
{
    public partial class ReportView : Window
    {
        public ReportView(DbContextEntityes db, DateTime minDate, DateTime maxDate)
        {
            InitializeComponent();
            DataContext = new ReportViewModel(db, minDate, maxDate);
        }
    }
}
