using System.ComponentModel.DataAnnotations;

namespace EC07_CMS_Umbraco_vs.ViewModels
{
    public class ContactFormViewModel
    {
        [Required]
        [MaxLength(80, ErrorMessage = "Limit 80 characters")]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(255, ErrorMessage = " Limit 500 characters")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string EmailAddress { get; set; } = null!;
        [Required]
        [MaxLength(500, ErrorMessage = " Limit 500 characters")]
        public string Comment { get; set; } = null!;

    }
}
