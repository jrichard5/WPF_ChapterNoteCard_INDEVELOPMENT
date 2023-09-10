using System;
using System.Collections.Generic;
using System.Globalization;
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
using WpfNotecardUI.ViewModels;

namespace WpfNotecardUI.Views
{
    /// <summary>
    /// Interaction logic for RandomNotecardsView.xaml
    /// </summary>
    public partial class RandomNotecardsView : UserControl
    {
        public RandomNotecardsView()
        {
            InitializeComponent();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = (RandomNoteCardViewModel)this.DataContext;
            var currentSentence = vm.DisplayedNotecard;
            RichTextBox box = richTextBox;

            
        }

        //Needed to add NotifyOnTargetUpdated to true   <TextBlock Text="{Binding DisplayedNotecard.Question, NotifyOnTargetUpdated=True}"
        //I needed to add the character Exist List as the first thing that changes in the VM otherwise it used the old question when the notecard changes.
        private void TextBoxQuestion_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            var vm = (RandomNoteCardViewModel)this.DataContext;
            if (vm is null) 
            {
                return;
            }
            var currentSentence = vm.DisplayedNotecard;
            if (currentSentence.CharExistList is not null)
            {
                TextBoxQuestion.Visibility = Visibility.Collapsed;
                FlowDocument flowDocument = new FlowDocument();
                flowDocument.TextAlignment = TextAlignment.Center;
                Paragraph para = new Paragraph();
                //foreach (var singleCharacter in charExistList)
                for (int i = 0; i < currentSentence.Question.Length; i++)
                {

                    //Run run = new Run(singleCharacter.Character.ToString());
                    Run run = new Run(currentSentence.Question[i].ToString());
                    if (currentSentence.CharExistList[i] == false)
                    {
                        run.Foreground = Brushes.Red;
                    }

                    para.Inlines.Add(run);
                }
                flowDocument.Blocks.Add(para);
                richTextBox.Document = flowDocument;

                richTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                TextBoxQuestion.Visibility = Visibility.Visible;
                richTextBox.Visibility = Visibility.Collapsed;
            }
        }

    }

    
}
