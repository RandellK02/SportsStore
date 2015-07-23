﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup( m => m.Products ).Returns( new Product[]{
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            } );

            ProductController c = new ProductController( mock.Object );
            c.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)c.List( null, 2 ).Model;

            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue( prodArray.Length == 2 );
            Assert.AreEqual( prodArray[0].Name, "P4" );
            Assert.AreEqual( prodArray[1].Name, "P5" );
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model() {
        // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            });
        // Arrange
        ProductController controller = new ProductController(mock.Object);
        controller.PageSize = 3;
        // Act
        ProductsListViewModel result =(ProductsListViewModel)controller.List(null, 2).Model;
        // Assert
        PagingInfo pageInfo = result.PagingInfo;
        Assert.AreEqual(pageInfo.CurrentPage, 2);
        Assert.AreEqual(pageInfo.ItemsPerPage, 3);
        Assert.AreEqual(pageInfo.TotalItems, 5);
        Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup( m => m.Products ).Returns( new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            });
            // Arrange - create a controller and make the page size 3 items
            ProductController controller = new ProductController( mock.Object );
            controller.PageSize = 3;
            // Action
            Product[] result = ((ProductsListViewModel)controller.List( "Cat2", 1 ).Model).Products.ToArray();
            // Assert
            Assert.AreEqual( result.Length, 2 );
            Assert.IsTrue( result[0].Name == "P2" && result[0].Category == "Cat2" );
            Assert.IsTrue( result[1].Name == "P4" && result[1].Category == "Cat2" );
        }
    }
}
