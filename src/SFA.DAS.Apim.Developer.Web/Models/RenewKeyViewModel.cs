using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Apim.Developer.Web.Models
{
    public class RenewKeyViewModel
    {
        [Required(ErrorMessage = "Select yes or no to continue")]
        public bool? ConfirmRenew { get; set; }
    }
}