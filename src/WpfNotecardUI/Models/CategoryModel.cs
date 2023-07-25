using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public bool IsSelectedForDeletion { get; set; }

        public CategoryModel(Category category)
        {
            Id = category.Id;
            CategoryName = category.CategoryName;
        }
    }
}
