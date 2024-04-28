using _Common.Fetch.Pagination;
using AcmePay.Application.UseCases.Queries;
using AcmePay.Infrastructure.Queries;
using AcmePay.Test.Fixtures;

namespace AcmePay.Test.Application.Queries
{
    public class ReadTransactionsTests
    {
        [Fact]
        public async Task ReadFacilitiesForCompanyByCompanyId_Succeed()
        {
            // Arrange
            var pageSize = 7;
            var currentPage = 1;
            var contract = new ReadTransactions.Contract();
            contract.PaginationRequest = new PaginationRequest(currentPage, pageSize); ;

            var repository = new ReadTransactionsQuery(ConnectionProvider.Connect());
            var useCase = new ReadTransactions.UseCase(repository);

            // Act
            var result = await useCase.Handle(contract, CancellationToken.None);

            // Assert
            Assert.Equal(pageSize, result.PageSize);
            Assert.Equal(currentPage, result.CurrentPageNumber);
            Assert.False(result.HasPreviousPage);

        }
    }
}
