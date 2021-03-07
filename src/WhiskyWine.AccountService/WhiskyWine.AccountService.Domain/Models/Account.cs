using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WhiskyWine.AccountService.Domain.Models
{
    public class Account : IAccount
    {
        [JsonIgnore]
        public string AccountCode { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string UserEmail { get; set; }
    }
}