using System.ComponentModel.DataAnnotations;

namespace Expenses.Models
{
    public class CategoryWishlist
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
