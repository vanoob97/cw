using System.ComponentModel.DataAnnotations;

namespace auth5.Models
{
    public class AddItemModel
    {
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Title { get; set; }
        public int ItemCollectionId { get; set; }
        public List<int> NumFields { get; set; }
        public List<string> StrFields { get; set; }
        public List<string> TextFields { get; set; }
        public List<string> BoolFields { get; set; }
    }
}
