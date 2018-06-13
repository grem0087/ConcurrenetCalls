using SomeIntrestingService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SomeIntrestingService
{
    public class AccountInfoAsync
    {
        private readonly int _accountId;
        private readonly IAccountService _accountService;

        private double amount;

        private volatile Task refreshInProgress;

        private readonly object lockobject = new object();

        public AccountInfoAsync(int accountId, IAccountService accountService)
        {
            _accountId = accountId;
            _accountService = accountService;
        }

        public double Amount
        {
            get
            {
                return amount;
            }
            private set
            {
                Interlocked.Exchange(ref amount, value);
            }
        }

        public async Task RefreshAmountAsync()
        {
            if (refreshInProgress == null)
            {
                lock (lockobject)
                {
                    if (refreshInProgress == null)
                    {
                        refreshInProgress =
                            _accountService
                            .GetAccountAmountAsync(_accountId)
                            .ContinueWith(task => Amount = task.Result)
                            .ContinueWith(task => refreshInProgress = null);
                    }
                }
            }
            var localTask = refreshInProgress;
            if (localTask != null)
            {
                await localTask.ConfigureAwait(false);
            }

        }
    }
}
