using Moq;
using SomeIntrestingService;
using SomeIntrestingService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestExercise
{
    public class UnitTestAsync
    {
        private readonly Mock<IAccountService> moq;

        public UnitTestAsync()
        {
            moq = new Mock<IAccountService>();
            moq.Setup(x => x.GetAccountAmountAsync(1)).Returns(Task.Delay(1000).ContinueWith(task => 333.2));
            moq.Setup(x => x.GetAccountAmountAsync(5)).Returns(Task.Delay(1000).ContinueWith(task => 32.0));

            moq.Setup(x => x.GetAccountAmount(It.IsNotIn(new int[] { 1, 5 })))
                .Throws(new InvalidOperationException());
        }

        [Fact]
        public async Task AccountInfo_SuccessesfulLocked()
        {
            var testee = new AccountInfoAsync(5, moq.Object);
            var tasks = Enumerable.Range(0, 3).
                Select(x => testee.RefreshAmountAsync());

            await Task.WhenAll(tasks);

            moq.Verify(x => x.GetAccountAmountAsync(5), Times.Once());
            Assert.Equal(32, testee.Amount);
        }

        [Fact]
        public async Task AccountInfo_MultipleCalls()
        {
            var testee = new AccountInfoAsync(5, moq.Object);

            await testee.RefreshAmountAsync();
            await testee.RefreshAmountAsync();
            await testee.RefreshAmountAsync();

            moq.Verify(x => x.GetAccountAmountAsync(5), Times.Exactly(3));
            Assert.Equal(32, testee.Amount);
        }


        [Theory]
        [InlineData(1, 333.2)]
        [InlineData(5, 32)]
        public async Task AccountInfo_Successes(int accountId, double ammount)
        {
            var testee = new AccountInfoAsync(accountId, moq.Object);
            await testee.RefreshAmountAsync();
            Assert.Equal(ammount, testee.Amount);
            moq.Verify(x => x.GetAccountAmountAsync(accountId));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(7)]
        public void AccountInfo_Fail(int accountId)
        {
            var testee = new AccountInfoAsync(accountId, moq.Object);
            Assert.ThrowsAsync<InvalidOperationException>(() => testee.RefreshAmountAsync());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void AccouuntInfo_Amount_DidntChange(int accountId)
        {
            var testee = new AccountInfoAsync(accountId, moq.Object);
            Assert.Equal(0, testee.Amount);
        }
    }
}
