using CommunityToolkit.Mvvm.ComponentModel;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Models.RandomNotecardPage
{
    public class ChapterDeck : ObservableObject
    {
        public ChapterNoteCard CurrentChapter { get; set; }
        public List<SentenceForDeck> Sentences { get; set; }
    }
}
