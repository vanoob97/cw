using auth5.Models;

namespace auth5.ViewModels
{
    public class UserCollections
    {
        public string Email { get; set; }
        public IList<ItemCollection> Collections { get; set; }
    }
}
