using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities.Comment
{
    public class Impression : BaseEntity
    {
        public bool LikeOrDislike { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
    }
}
