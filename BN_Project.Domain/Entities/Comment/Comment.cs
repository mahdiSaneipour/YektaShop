using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities.Comment
{
    public class Comment : BaseEntity
    {
        public int BuildQuality { get; set; }
        public int ValueForMoneyComparedToTHePrice { get; set; }
        public int Innovation { get; set; }
        public int FeaturesAndCapabilities { get; set; }
        public int EaseOfUse { get; set; }
        public int DesignAndAppearance { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public bool? IsConfirmed { get; set; }

        #region Relations 
        public ICollection<Strength> Strengths { get; set; }
        public ICollection<WeakPoint> WeakPoints { get; set; }
        public ICollection<Impression> Impressions { get; set; }
        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        #endregion
    }
}