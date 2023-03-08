using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Apim.Developer.Web.Models
{
    public class DeleteKeyViewModel
    {
        [Required(ErrorMessage = "Select yes or no to continue")]
        public bool? ConfirmDelete { get; set; }
    }
}