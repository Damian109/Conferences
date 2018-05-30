using System.Windows;
using Conferences.bdWork;
using Conferences.modelView;

namespace Conferences
{
    public partial class ServiceView : Window
    {
        public ServiceView(DbContextEntityes db)
        {
            InitializeComponent();
            DataContext = new ServiceViewModel(db);
        }
    }
}
