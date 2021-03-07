namespace WhiskyWine.AccountService.Domain.Models
{
    public interface IAccount
    {
        public string AccountCode { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string UserEmail { get; set; }
    }
}