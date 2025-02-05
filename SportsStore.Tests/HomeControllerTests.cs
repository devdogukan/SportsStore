using Microsoft.AspNetCore.Mvc;
using SportsStore.Controllers;
using SportsStore.Models;
using Moq;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests;

public class HomeControllerTests
{
    [Fact]
    public void Can_Use_Repository()
    {
        // Arrange
        Mock<IStoreRepository> mock = new();

        mock.SetupGet(m => m.Products).Returns((new Product[] {
            new Product { ProductID = 1, Name = "P1" },
            new Product { ProductID = 2, Name = "P2" }
        }).AsQueryable<Product>());

        HomeController controller = new(mock.Object);

        // Act
        ProductsListViewModel? result = (controller.Index() as ViewResult)?.ViewData.Model as ProductsListViewModel ?? new();

        // Assert
        Product[]? productArray = result?.Products?.ToArray() ?? Array.Empty<Product>();

        Assert.Equal(2, productArray.Length);
        Assert.Equal("P1", productArray[0].Name);
        Assert.Equal("P2", productArray[1].Name);
    }

    [Fact]
    public void Can_Paginate()
    {
        //Arrange
        Mock<IStoreRepository> mock = new();

        mock.SetupGet(m => m.Products).Returns((new Product[] {
            new Product { ProductID = 1, Name = "P1" },
            new Product { ProductID = 2, Name = "P2" },
            new Product { ProductID = 3, Name = "P3" },
            new Product { ProductID = 4, Name = "P4" },
            new Product { ProductID = 5, Name = "P5" }
        }).AsQueryable<Product>());

        HomeController controller = new(mock.Object);
        controller.PageSize = 3;

        // Act
        ProductsListViewModel? result = (controller.Index(2) as ViewResult)?.ViewData.Model as ProductsListViewModel ?? new();

        // Assert
        Product[]? productArray = result?.Products?.ToArray() ?? Array.Empty<Product>();

        Assert.Equal(2, productArray.Length);
        Assert.Equal("P4", productArray[0].Name);
        Assert.Equal("P5", productArray[1].Name);
    }

    [Fact]
    public void Can_Send_Pagination_View_Model()
    {
        // Arrange
        Mock<IStoreRepository> mock = new();

        mock.SetupGet(m => m.Products).Returns((new Product[] {
            new Product { ProductID = 1, Name = "P1" },
            new Product { ProductID = 2, Name = "P2" },
            new Product { ProductID = 3, Name = "P3" },
            new Product { ProductID = 4, Name = "P4" },
            new Product { ProductID = 5, Name = "P5" }
        }).AsQueryable<Product>());

        HomeController controller = new(mock.Object) { PageSize = 3 };

        // Act
        ProductsListViewModel? result = (controller.Index(2) as ViewResult)?.ViewData.Model as ProductsListViewModel;

        // Assert
        PagingInfo? pageInfo = result?.PagingInfo;
        Assert.Equal(2, pageInfo?.CurrentPage);
        Assert.Equal(3, pageInfo?.ItemsPerPage);
        Assert.Equal(5, pageInfo?.TotalItems);
        Assert.Equal(2, pageInfo?.TotalPages);
    }
}