using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Models
{
    class SentenceNoteCardModel
    {
        public string ItemQuestion { get; set; }
        public string ItemAnswer { get; set; }
        public string Hint { get; set; }
        public int MemorizationLevel { get; set; }
        public bool IsUserWantsToFocusOn { get; set; }
        public DateTime LastTimeAccessed { get; set; }

        public SentenceNoteCardModel(SentenceNoteCard noteCard)
        {
            ItemQuestion = noteCard.ItemQuestion;
            ItemAnswer = noteCard.ItemAnswer;
            Hint = noteCard.Hint;
            MemorizationLevel = noteCard.MemorizationLevel;
            IsUserWantsToFocusOn = noteCard.IsUserWantsToFocusOn;
            LastTimeAccessed = noteCard.LastTimeAccess;
        }
    }
}
