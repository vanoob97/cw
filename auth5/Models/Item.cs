namespace auth5.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int ItemCollectionId { get; set; }
        public string Title { get; set; }
        public string NumFields { get; set; }
        public string StrFields { get; set; }
        public string TextFields { get; set; }
        public string BoolFields { get; set; }
        public string Comments { get; set; }
        public string Likes { get; set; }
    }
}
