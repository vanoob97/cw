using auth5.Models;

namespace auth5.ViewModels
{
    public class UserCollections
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public IList<ItemCollection> Collections { get; set; }
    }
}
