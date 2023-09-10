using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNotecardUI.Models.DataObjects;

namespace WpfNotecardUI.Models.RandomNotecardPage
{
    public class SentenceForDeck
    {
        public SentenceNoteCard SentenceNoteCard { get; set; }
        public bool[] CharExistList { get; set; }
    }
}
