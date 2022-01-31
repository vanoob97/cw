using System.ComponentModel.DataAnnotations;

namespace auth5.Models
{
    public class AddCollectionModel
    {
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Theme { get; set; }
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }
        public string Email { get; set; }
        public List<string> StrFields { get; set; }
        public List<string> TextFields { get; set; }
        public List<string> NumFields { get; set; }
        public List<string> BoolFields { get; set; }
    }
}
