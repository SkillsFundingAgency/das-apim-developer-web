using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Apim.Developer.Web.Models
{
    public class RenewKeyViewModel
    {
        [Required(ErrorMessage = "Select whether you want to renew this key")]
        public bool? ConfirmRenew { get; set; }
    }
}