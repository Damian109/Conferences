using System.Windows;
using Conferences.bdWork;
using Conferences.modelView;

namespace Conferences
{
    public partial class GroupView : Window
    {
        public GroupView(DbContextEntityes db)
        {
            InitializeComponent();
            DataContext = new GroupViewModel(db);
        }
    }
}
