using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Title { get; set; }
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Category? ParentCategory { get; set; }

        [InverseProperty(nameof(ParentCategory))]
        public ICollection<Category> SubCategories { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
