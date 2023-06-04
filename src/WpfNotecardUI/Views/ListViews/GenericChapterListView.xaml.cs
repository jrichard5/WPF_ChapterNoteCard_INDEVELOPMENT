using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfNotecardUI.Models;
using WpfNotecardUI.ViewModels.ListVModels;

namespace WpfNotecardUI.Views.ListViews
{
    /// <summary>
    /// Interaction logic for GenericChapterListView.xaml
    /// </summary>
    public partial class GenericChapterListView : UserControl
    {
        public GenericChapterListView()
        {
            InitializeComponent();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as DataGrid)?.SelectedItem;
            if (item != null)
            {
                (this.DataContext as GenericChapterListViewModel).SwitchToGenericSentenceView((ChapterItemModel)item);
            }
        }
    }
}
