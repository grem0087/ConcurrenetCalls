using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SomeIntrestingService.Interfaces
{
    public interface IAccountService
    {
        double GetAccountAmount(int accountId);

        Task<double> GetAccountAmountAsync(int accountId);
    }
}
