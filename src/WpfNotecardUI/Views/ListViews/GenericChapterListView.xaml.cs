using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private bool _isEditToggle = false;

        public GenericChapterListView()
        {
            InitializeComponent();
            ContextMenu = (ContextMenu)Resources["contextMenu"];
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!_isEditToggle)
            {
                var item = (sender as DataGrid)?.SelectedItem;
                if (item != null)
                {
                    (this.DataContext as GenericChapterListViewModel).SwitchToGenericSentenceView((ChapterItemModel)item);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_isEditToggle)
            {
                _isEditToggle = false;
                EditToggle.Text = string.Empty;
                DataList.IsReadOnly = true;
            }
            else
            {
                _isEditToggle = true;
                EditToggle.Text = "Edit Toggle Engaged";
                DataList.IsReadOnly = false;
            }

        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                e.Column.Header = descriptor.DisplayName ?? descriptor.Name;
            }
        }

        private void DataList_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var datagrid = sender as DataGrid;
            if (datagrid is null)
            {
                return;
            }
            var columnHeader = datagrid.CurrentColumn.Header;
            if (columnHeader.ToString() == "GradeLevel")
            {
                e.Handled = !int.TryParse(e.Text, out _);
            }
            Debug.WriteLine("hi");
        }
    }
}
