using System;
using Xunit;
using Moq;
using SomeIntrestingService.Interfaces;
using SomeIntrestingService;

namespace TestExecise
{
    public class UnitTests
    {
        private readonly Mock<IAccountService> moq;

        public UnitTests()
        {
            moq = new Mock<IAccountService>();
            moq.Setup(x => x.GetAccountAmount(1)).Returns(333.2);
            moq.Setup(x => x.GetAccountAmount(5)).Returns(32);

            moq.Setup(x => x.GetAccountAmount(It.IsNotIn( new int[] { 1, 5} )))
                .Throws(new InvalidOperationException());
        }

        [Theory]
        [InlineData(1, 333.2)]
        [InlineData(5, 32)]
        public void AccountInfo_Successes(int accountId, double ammount)
        {
            var testee = new AccountInfo(accountId, moq.Object);
            testee.RefreshAmount();
            Assert.Equal(ammount, testee.Amount);
            moq.Verify(x => x.GetAccountAmount(accountId));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(7)]
        public void AccountInfo_Fail(int accountId)
        {
            var testee = new AccountInfo(accountId, moq.Object);
            Assert.Throws<InvalidOperationException>(() => testee.RefreshAmount());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void AccouuntInfo_Amount_DidntChange(int accountId)
        {
            var testee = new AccountInfo(accountId, moq.Object);
            Assert.Equal(0, testee.Amount);
        }
    }
}
