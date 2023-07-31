using System;
using System.Collections.Generic;
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

namespace WpfNotecardUI.Views.ListViews
{
    /// <summary>
    /// Interaction logic for GenericSentenceListView.xaml
    /// </summary>
    public partial class GenericSentenceListView : UserControl
    {
        private bool _isEditToggle = false;

        public GenericSentenceListView()
        {
            InitializeComponent();
            ContextMenu = (ContextMenu)Resources["contextMenu"];
        }

        private void DataList_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var datagrid = sender as DataGrid;
            if (datagrid is null)
            {
                return;
            }
            var columnHeader = datagrid.CurrentColumn.Header;
            if (columnHeader.ToString() == "MemorizationLevel")
            {
                e.Handled = !int.TryParse(e.Text, out _);
            }
            if (columnHeader.ToString() == "IsUserWantsToFocusOn")
            {
                Debug.WriteLine("hi");
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
    }
}
