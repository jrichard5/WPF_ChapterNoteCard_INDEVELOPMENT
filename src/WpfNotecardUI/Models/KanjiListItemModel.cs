using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Models
{
    public class KanjiListItemModel
    {
        public string TopicName { get; set; }
        public string TopicDefinition { get; set; }
        public int? GradeLevel { get; set; }
        public DateTime? LastTimeAccess { get; set; }
        public int? NewspaperRank { get; set; }
        public int? JLPTLevel { get; set; }
        public string KunReadings { get; set; }
        public string OnReadings { get; set; }
        public string OtherReadings { get; set; }
        public bool IsSelectedForDeletion { get; set; }
        public KanjiListItemModel(KanjiNoteCard card)
        {
            TopicName = card.TopicName;
            TopicDefinition = card.ChapterNoteCard.TopicDefinition;
            GradeLevel = card.ChapterNoteCard?.GradeLevel;
            LastTimeAccess = card.ChapterNoteCard?.LastTimeAccess;
            NewspaperRank = card.NewspaperRank;
            JLPTLevel = card.JLPTLevel;

            foreach (var reading in card.KanjiReadings)
            {
                if (reading.TypeOfReading == "kun")
                {
                    if (String.IsNullOrEmpty(KunReadings))
                    {
                        KunReadings = reading.Reading;
                    }
                    else
                    {
                        KunReadings += " | " + reading.Reading;
                    }
                }
                else if (reading.TypeOfReading == "on")
                {
                    if (String.IsNullOrEmpty(OnReadings))
                    {
                        OnReadings = reading.Reading;
                    }
                    else
                    {
                        OnReadings += " | " + reading.Reading;
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(OtherReadings))
                    {
                        OtherReadings = reading.Reading;
                    }
                    else
                    {
                        OtherReadings += " ? " + reading.Reading;
                    }
                }
            }

        }
    }
}
