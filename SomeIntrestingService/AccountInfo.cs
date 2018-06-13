using SomeIntrestingService.Interfaces;

namespace SomeIntrestingService
{
    public class AccountInfo
    {
        private readonly int _accountId;
        private readonly IAccountService _accountService;

        public AccountInfo(int accountId, IAccountService accountService)
        {
            _accountId = accountId;
            _accountService = accountService;
        }

        public double Amount { get; set; }

        public void RefreshAmount()
        {
            Amount = _accountService.GetAccountAmount(_accountId);
        }
    }
}
