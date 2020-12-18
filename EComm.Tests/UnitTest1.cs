using EComm.Data.Entities;
using EComm.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EComm.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, 2 + 2);
        }

        [Fact]
        public async Task ProductDetails()
        {
            // Arrange
            var repo = new StubRepository();
            var pc = new ProductController(repo);

            // Act
            var result = await pc.Details(1);

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            var vr = result as ViewResult;
            Assert.IsAssignableFrom<Product>(vr.Model);
            var model = vr.Model as Product;
            Assert.Equal("Bread", model.ProductName);
        }
    }
}
