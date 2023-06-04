using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Models
{
    public class ChapterItemModel
    {
        [DisplayName("Topic Name (renamed in the model)")]
        public string TopicName { get; set; }
        public string TopicDefinition { get; set; }
        [ReadOnly(true)]
        public int? GradeLevel { get; set; }
        [ReadOnly(true)]
        public DateTime? LastTimeAccess { get; set; }

        public ChapterItemModel(ChapterNoteCard chapterNoteCard) 
        {
            TopicName = chapterNoteCard.TopicName;
            TopicDefinition = chapterNoteCard.TopicDefinition;
            GradeLevel = chapterNoteCard.GradeLevel;
            LastTimeAccess = chapterNoteCard.LastTimeAccess;
        }
    }
}
