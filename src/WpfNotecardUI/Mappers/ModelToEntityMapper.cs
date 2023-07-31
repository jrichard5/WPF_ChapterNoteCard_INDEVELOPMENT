using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNotecardUI.Models;

namespace WpfNotecardUI.Mappers
{
    public static class ModelToEntityMapper
    {
        public static ChapterNoteCard ToChapterNoteCardPrimaryKey(KanjiListItemModel model)
        {
            return new ChapterNoteCard{ TopicName = model.TopicName };
        }

        public static SentenceNoteCard ToSentenceNoteCardPrimaryKey(JapaneseWordListItemModel model)
        {
            return new SentenceNoteCard { ItemQuestion = model.ItemQuestion };
        }

        public static ChapterNoteCard FromChapterItemToChapterNoteCard(ChapterItemModel model, int categoryId) 
        {
            return new ChapterNoteCard
            {
                TopicName = model.TopicName,
                TopicDefinition = model.TopicDefinition,
                GradeLevel = model.GradeLevel ?? 0,
                LastTimeAccess = model.LastTimeAccess ?? DateTime.MinValue,
                CategoryId = categoryId
            };
        }

        public static List<KanjiReading> GetKanjiReadingsFromItem(string topicName, string kun, string on)
        {
            List<KanjiReading> kanjiReadings = new List<KanjiReading>();
            if (kun != null)
            {
                string[] readings = kun.Split('|', ' ');
                foreach (var item in readings)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    kanjiReadings.Add(new KanjiReading { Reading = item, TypeOfReading = "kun", KanjiNoteCardTopicName= topicName });
                }
            }
            

            if (on != null)
            {
                string[] onReadings = on.Split('|', ' ');
                foreach (var item in onReadings)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    kanjiReadings.Add(new KanjiReading { Reading = item, TypeOfReading = "on", KanjiNoteCardTopicName = topicName });
                }
            }
            
            return kanjiReadings;
        }
    }
}
