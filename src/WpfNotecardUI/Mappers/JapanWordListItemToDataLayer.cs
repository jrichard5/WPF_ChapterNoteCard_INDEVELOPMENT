using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNotecardUI.Models;

namespace WpfNotecardUI.Mappers
{
    public static class JapanWordListItemToDataLayer
    {
        public static JapaneseWordNoteCard ConvertToNoteCard(JapaneseWordListItemModel model, string topicName)
        {
            JapaneseWordNoteCard japaneseWordNoteCard = new JapaneseWordNoteCard();
            SentenceNoteCard sentNoteCard = new SentenceNoteCard();
            ChapterNoteCardSentenceNoteCard ch = new ChapterNoteCardSentenceNoteCard();
            ExtraJishoInfoOnBridge ex = new ExtraJishoInfoOnBridge();

            ex.PageNumber = model.PageNumber ?? 1000;
            ex.Order = model.Order ?? 10000;
            //ex.ChapterNoteCardTopicName = topicName;
            //ex.SentenceNoteCardItemQuestion = model.ItemQuestion;
            ex.Id = model.ExtraJishoPrimaryId;

            ch.ChapterNoteCardTopicName = topicName;
            ch.SentenceNoteCardItemQuestion = model.ItemQuestion;
            ch.ExtraJishoInfo = ex;

            sentNoteCard.ItemQuestion = model.ItemQuestion;
            sentNoteCard.ItemAnswer = model.ItemAnswer;
            sentNoteCard.Hint = model.Hint;
            sentNoteCard.MemorizationLevel = model.MemorizationLevel;
            sentNoteCard.IsUserWantsToFocusOn = model.IsUserWantsToFocusOn;
            sentNoteCard.LastTimeAccess = model.LastTimeAccessed;
            sentNoteCard.ChapterSentences = new List<ChapterNoteCardSentenceNoteCard>
            {
                ch
            };

            japaneseWordNoteCard.ItemQuestion = model.ItemQuestion;
            japaneseWordNoteCard.SentenceNoteCard = sentNoteCard;
            japaneseWordNoteCard.IsCommonWord = model.IsCommonWord;
            japaneseWordNoteCard.JLPTLevel = model.JLPTLevel;

            return japaneseWordNoteCard;
        }
    }
}
