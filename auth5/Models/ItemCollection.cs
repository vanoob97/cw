namespace auth5.Models
{
    public class ItemCollection
    {
        public int Id { get; set; }
        public string OwnerEmail { get; set; }
        public string Title { get; set; }
        public string Theme { get; set; }
        public string Description { get; set; }
        public string ImgSrc { get; set; }
        public string NumFields { get; set; }
        public string StrFields { get; set; }
        public string TextFields { get; set; }
        public string BoolFields { get; set; }
        public IList<Item> Items { get; set; }

    }
}
