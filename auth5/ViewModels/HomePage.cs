using auth5.Models;

namespace auth5.ViewModels
{
    public class HomePage
    {
        public IList<Item> Items { get; set; }

        public IList<ItemCollection> Collections { get; set; }
    }
}
