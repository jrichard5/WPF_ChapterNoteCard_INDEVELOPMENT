using DataLayer;
using DataLayer.Entities;
using DataLayer.IRepos;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Mappers
{
    public static class AddKanjiToKanjiNoteCard
    {
        public static async Task<KanjiNoteCard> MapToNotecard(string parentName, string topicName, string topicDefinition, DateTime lastTime, int gradeLevel, int newsPaperRank, int JLPTLevel, string kunReadings, string onReadings)
        {
            KanjiNoteCard newNotecard = new KanjiNoteCard();
            newNotecard.ChapterNoteCard = new ChapterNoteCard();

            using(var context = new KanjiDbContext())
            {
                ICategoryRepo cateRepo = new CategoryRepo(context);
                var cate = await cateRepo.GetFirstCategoryByName(parentName);

                newNotecard.ChapterNoteCard.CategoryId = cate.Id;
            }
            newNotecard.ChapterNoteCard.TopicName = topicName;
            newNotecard.ChapterNoteCard.TopicDefinition = topicDefinition;
            newNotecard.ChapterNoteCard.GradeLevel = gradeLevel;
            newNotecard.ChapterNoteCard.LastTimeAccess = lastTime;
            newNotecard.NewspaperRank = newsPaperRank;
            newNotecard.JLPTLevel = JLPTLevel;

            newNotecard.KanjiReadings = new List<KanjiReading>();
            newNotecard.KanjiReadings.AddRange(SplitReadings("kun", kunReadings));
            newNotecard.KanjiReadings.AddRange(SplitReadings("on", onReadings));
            return newNotecard;

        }

        private static List<KanjiReading> SplitReadings(string typeOfReading, string allReading)
        {
            List<KanjiReading> newList = new List<KanjiReading>();
            string[] readings = allReading.Split(' ', ',');
            foreach (string reading in readings)
            {
                if(string.IsNullOrEmpty(reading)) continue;
                newList.Add(new KanjiReading { Reading = reading, TypeOfReading = typeOfReading });
            }
            return newList;
        }
    }
}
