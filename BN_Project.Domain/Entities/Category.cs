using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Title { get; set; }
        public int ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Category Catagory { get; set; }
    }
}
