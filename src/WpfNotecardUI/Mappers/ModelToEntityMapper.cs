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
    }
}
