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
using WpfNotecardUI.ViewModels.DialogViewModels;
using WpfNotecardUI.Views.Dialogs;

namespace WpfNotecardUI.Views.ListViews
{
    /// <summary>
    /// Interaction logic for JapaneseWordListView.xaml
    /// </summary>
    public partial class JapaneseWordListView : UserControl
    {
        public JapaneseWordListView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            AddJapanWordViewModel vm = new AddJapanWordViewModel(TopicName.Text) ;
            JapanWordDialog dialog = new JapanWordDialog
            {
                DataContext = vm
            };
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }
    }
}
