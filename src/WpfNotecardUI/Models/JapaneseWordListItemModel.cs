using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Models
{
    public class JapaneseWordListItemModel : SentenceNoteCardModel
    {
        public bool IsCommonWord { get; set; }
        public int? JLPTLevel { get; set; }
        public int? PageNumber { get; set; }
        public int? Order { get; set; }
        public int ExtraJishoPrimaryId { get; set; }

        public JapaneseWordListItemModel(JapaneseWordNoteCard jnoteCard) : base(jnoteCard.SentenceNoteCard)
        {
            IsCommonWord = jnoteCard.IsCommonWord;
            JLPTLevel = jnoteCard.JLPTLevel;
            var bridgeTable = jnoteCard.SentenceNoteCard.ChapterSentences.First();
            if (bridgeTable != null)
            {
                PageNumber = bridgeTable.ExtraJishoInfo?.PageNumber;
                Order = bridgeTable.ExtraJishoInfo?.Order;
                ExtraJishoPrimaryId = bridgeTable.ExtraJishoInfo.Id;
            }

        }

    }
}
