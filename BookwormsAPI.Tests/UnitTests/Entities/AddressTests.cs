using BookwormsAPI.Entities.Borrowing;
using Xunit;

namespace BookwormsAPI.Tests.UnitTests.Entities
{
    public class AddressTests : TestBase
    {
        [Fact]
        public void CreateAddress_ReturnsAddressWithExpectedStreet()
        {
            // Arrange

            // Act
            var testAddress = new Address()
            {
                FirstName = "firstName",
                LastName = "lastName",
                Street = "Test Street",
                City = "city",
                County = "county",
                PostCode = "postCode",
            };

            // Assert
            Assert.NotNull(testAddress);
            Assert.Equal("Test Street", testAddress.Street);
        }
    }
}
