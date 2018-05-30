using System.Windows;
using Conferences.modelView;

namespace Conferences
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ConferenceViewModel();
        }
    }
}
