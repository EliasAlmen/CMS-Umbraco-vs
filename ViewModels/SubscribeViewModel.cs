using System.ComponentModel.DataAnnotations;

namespace EC07_CMS_Umbraco_vs.ViewModels
{
    public class SubscribeViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
