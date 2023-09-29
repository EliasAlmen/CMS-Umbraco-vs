using System.ComponentModel.DataAnnotations;

namespace EC07_CMS_Umbraco_vs.Models
{
    public class SubscribersEntity
    {
        [Key]
        public string Email { get; set; } = null!;
    }
}
