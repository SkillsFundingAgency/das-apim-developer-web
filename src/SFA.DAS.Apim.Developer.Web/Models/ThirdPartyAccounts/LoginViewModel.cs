using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SFA.DAS.Apim.Developer.Web.Models.ThirdPartyAccounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your email address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
        
        public static Dictionary<string, int> BuildPropertyOrderDictionary()
        {
            var itemCount = 0;
            var propertyOrderDictionary = typeof(LoginViewModel).GetProperties().Select(c => new
            {
                Order = itemCount++,
                c.Name
            }).ToDictionary(key => key.Name, value => value.Order);
            return propertyOrderDictionary;
        }
    }
}