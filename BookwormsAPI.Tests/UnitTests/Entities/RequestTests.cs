using BookwormsAPI.Entities.Borrowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookwormsAPI.Tests.UnitTests.Entities
{
    public class RequestTests
    {
        [Fact]
        public void CreateRequest_ReturnsAddressWithExpectedBorrowerEmail()
        {
            // Arrange
            var testAddress = new Address()
            {
                FirstName = "firstName",
                LastName = "lastName",
                Street = "Test Street",
                City = "city",
                County = "county",
                PostCode = "postCode",
            };

            var borrowerEmail = "test@test.com";
            var bookId = 1;

            // Act
            var request = new Request(bookId, borrowerEmail, testAddress);

            // Assert
            Assert.NotNull(request);
            Assert.Equal("test@test.com", request.BorrowerEmail);
        }

        [Fact]
        public void CreateRequestWithEmptyConstructor_ReturnsAddressWithExpectedBorrowerEmail()
        {
            // Arrange
            var testAddress = new Address()
            {
                FirstName = "firstName",
                LastName = "lastName",
                Street = "Test Street",
                City = "city",
                County = "county",
                PostCode = "postCode",
            };

            var borrowerEmail = "test@test.com";
            var bookId = 1;

            // Act
            var request = new Request()
            {
                BookId = bookId,
                BorrowerEmail = borrowerEmail,
                SendToAddress = testAddress
            };

            // Assert
            Assert.NotNull(request);
            Assert.Equal("test@test.com", request.BorrowerEmail);
        }

        [Fact]
        public void GetDueDate_ReturnsDate28DaysFromNow()
        {
            // Arrange
            var testAddress = new Address()
            {
                FirstName = "firstName",
                LastName = "lastName",
                Street = "Test Street",
                City = "city",
                County = "county",
                PostCode = "postCode",
            };

            var borrowerEmail = "test@test.com";
            var bookId = 1;

            // Act
            var request = new Request()
            {
                BookId = bookId,
                BorrowerEmail = borrowerEmail,
                SendToAddress = testAddress
            };

            var dueDate = request.GetDateDue(DateTime.Now).Date;
            var dateToday = DateTime.Now.AddDays(28).Date;

            // Assert
            Assert.Equal(dateToday, dueDate);
        }
    }
}
