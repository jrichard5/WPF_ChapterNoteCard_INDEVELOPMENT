using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WpfNotecardUI.Models;
using WpfNotecardUI.Models.DataObjects;
//using Xamarin.Forms;

namespace WpfNotecardUI.Views.ListViews.ListViewComponents
{
    /// <summary>
    /// Interaction logic for JapaneseWordListComponent.xaml
    /// </summary>
    public partial class JapaneseWordListComponent : UserControl
    {
        //public static readonly BindableProperty itemAnswerDictionary = BindableProperty.Create("CharExistList", typeof(List<CharacterExist>), typeof(JapaneseWordListItemModel), null);

        public JapaneseWordListComponent()
        {
            InitializeComponent();
        }



        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = (JapaneseWordListItemModel)this.DataContext;
            var charExistList = vm.CharExistList;
            var itemQuestion = vm.ItemQuestion;
            RichTextBox box = richTextBox;

            //Dictoinary may not preseve order
            for (var i = 0; i < itemQuestion.Length; i++)
            {
                if (itemQuestion[i].ToString() != charExistList[i].Character)
                {
                    //This means the list from charExistList is not in the correct order
                    throw new Exception();
                }
            }
            FlowDocument flowDocument = new FlowDocument();
            Paragraph para = new Paragraph();
            foreach (var singleCharacter in charExistList)
            {

                Run run = new Run(singleCharacter.Character.ToString());
                if (!singleCharacter.IsExist)
                {
                    run.Foreground = Brushes.Red;
                }

                para.Inlines.Add(run);

                //TextRange tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
                //tr.Text = singleCharacter.Character.ToString();
                //if (!singleCharacter.IsExist)
                //{
                //    tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
                //}
                //else
                //{
                //    tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
                //}
            }
            flowDocument.Blocks.Add(para);
            box.Document = flowDocument;
            string testString = new TextRange(box.Document.ContentStart, box.Document.ContentEnd).Text.ToString();
            if (!string.IsNullOrEmpty(testString))
            {
                var formattedText = new FormattedText(testString, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(box.FontFamily, box.FontStyle, box.FontWeight, box.FontStretch),
                    box.FontSize,
                    Brushes.Black,
                    new NumberSubstitution(),
                    1);
                box.Width = formattedText.Width > 250 ? 250 : formattedText.Width + 10;
            }
        }
    }
}
