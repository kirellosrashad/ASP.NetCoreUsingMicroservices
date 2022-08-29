using CatalogService.Controllers;
using CatalogService.Model;
using CatalogService.RabbitMQProducer;
using CatalogService.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ProductTest
{
    public class ProductTest
    {
        IRepository<Product> productRepository = new Repository<Product>();
        IMessageProducer msgProducer = new MessageProducer();

        #region Test Get All Products
        [Fact]
        public void TestGetAll_ReturnNotNullValue()
        {
            var controller = new ProductController(productRepository, msgProducer);
            var result = controller.Get();
            Assert.NotNull(result);
        }

        [Fact]
        public void TestGetAll_RetuenOKResult()
        {
            var controller = new ProductController(productRepository, msgProducer);
            var result = controller.Get();
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void TestGetAll_ReturnAllItems()
        {
            var controller = new ProductController(productRepository, msgProducer);
            var result = controller.Get();
            // Assert
            List<Product> allProducts = ((ObjectResult)result.Result).Value as List<Product>;
            Assert.Equal(6, allProducts.Count);  // change the number as per exiting number of items stored in DB
        }
        #endregion

        #region Test Get Product
        
        [Fact]
        public void TestGetProduct_ReturnNotFound()
        {
            var controller = new ProductController(productRepository, msgProducer);
            int id = 1;  // id which is not exist in in DB
            var result = controller.Getproduct(id);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void TestGetProduct_ReturnOKResult()
        {
            var controller = new ProductController(productRepository, msgProducer);
            int id = 6;
            var result = controller.Getproduct(id);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void TestGetProduct_ReturnSameRequestdID()
        {
            var controller = new ProductController(productRepository, msgProducer);
            int id = 6;
            var result = controller.Getproduct(id);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(id, ((Product)((ObjectResult)result.Result).Value).ID);
        }

        #endregion

        #region TestAddNewProduct

        [Fact]
        public void TestAddNewProduct_NullInput()
        {
            var controller = new ProductController(productRepository, msgProducer);
            Product product = null;
            var result = controller.Post(product);
            //Assert.Equal(StatusCodes.Status400BadRequest, ((StatusCodeResult)((result).Result).Result).StatusCode);
            Assert.IsType<BadRequestResult>((BadRequestResult)result.Result.Result);
            //var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            //Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void TestAddNewProduct_LeakageInfo()
        {
            var controller = new ProductController(productRepository, msgProducer);
            Product product = new Product()
            {
                ID = 0,
                Name = "Spagetti Red Sause",
                Cost = 10,
                Price = 20
            };
            var result = controller.Post(product);
            Assert.IsType<BadRequestResult>((BadRequestResult)result.Result.Result);
        }


        [Fact]
        public void TestAddNewProduct_NotSaved()
        {
            //tested by removing the call to DB
            var controller = new ProductController(productRepository, msgProducer);
            Product product = new Product()
            {
                ID = 0,
                Name = "Spagetti Red Sause",
                Cost = 10,
                //Price = 20,
                ImageBase64 = @"C:\Visual Studio Projects\ASP.NetCoreUsingMicroservices\CatalogService\wwwroot\Images\Spagetti Red Sause.jpg"
            };
            var result = controller.Post(product);
            Assert.Equal(StatusCodes.Status422UnprocessableEntity, ((StatusCodeResult)result.Result.Result).StatusCode);
        }

        [Fact]
        public void TestAddNewProduct_ReturnOKResult()
        {
            var controller = new ProductController(productRepository, msgProducer);
            Product product = new Product()
            {
                Name = "Spagetti Red Sause",
                Cost = 10,
                Price = 20,
                ImageBase64 = @"C:\Visual Studio Projects\ASP.NetCoreUsingMicroservices\CatalogService\wwwroot\Images\Spagetti Red Sause.jpg"
            };
            var result = controller.Post(product);
            Assert.IsType<OkObjectResult>(result.Result.Result);
        }

        #endregion

        #region Test Update Product

        [Fact]
        public void TestUpdateProduct_NullInput()
        {
            var controller = new ProductController(productRepository, msgProducer);
            Product product = null;
            var result = controller.Put(product);
            //Assert.Equal(StatusCodes.Status400BadRequest, ((StatusCodeResult)((result).Result).Result).StatusCode);
            Assert.IsType<BadRequestResult>((BadRequestResult)result.Result.Result);
            //var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            //Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void TestUpdateProduct_LeakageInfo()
        {
            var controller = new ProductController(productRepository, msgProducer);
            Product product = new Product()
            {
                ID = 6,
                Name = "Spagetti Red Sause",
                Cost = 10,
                Price = 20
            };
            var result = controller.Put(product);
            Assert.IsType<BadRequestResult>((BadRequestResult)result.Result.Result);
        }


        [Fact]
        public void TestUpdateProduct_NotSaved()
        {
            //tested by removing the call to DB
            var controller = new ProductController(productRepository, msgProducer);
            Product product = new Product()
            {
                ID = 6,
                Name = "Spagetti Red Sause",
                Cost = 10,
                Price = 20,
                ImageBase64 = @"C:\Visual Studio Projects\ASP.NetCoreUsingMicroservices\CatalogService\wwwroot\Images\Spagetti Red Sause.jpg"
            };
            var result = controller.Put(product);
            Assert.Equal(StatusCodes.Status304NotModified, ((StatusCodeResult)result.Result.Result).StatusCode);
        }

        [Fact]
        public void TestUpdateProduct_ReturnOKResult()
        {
            var controller = new ProductController(productRepository, msgProducer);
            Product product = new Product()
            {
                ID = 9,
                Name = "Spagetti Red Sause",
                Cost = 12,
                Price = 20,
                ImageBase64 = @"C:\Visual Studio Projects\ASP.NetCoreUsingMicroservices\CatalogService\wwwroot\Images\Spagetti Red Sause.jpg"
            };
            var result = controller.Put(product);
            Assert.IsType<OkObjectResult>(result.Result.Result);
        }

        #endregion

        #region Test Delete Product

        [Fact]
        public void TestDeleteProduct_NotExist()
        {
            var controller = new ProductController(productRepository, msgProducer);
            int id = 0;
            var result = controller.Delete(id);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void TestDeleteProduct_NotDeleted()
        {
            var controller = new ProductController(productRepository, msgProducer);
            int id = 4;
            var result = controller.Delete(id);
            Assert.Equal(StatusCodes.Status422UnprocessableEntity, ((StatusCodeResult)result.Result).StatusCode);
        }

        [Fact]
        public void TestDeleteProduct_ReturnOKResult()
        {
            var controller = new ProductController(productRepository, msgProducer);
            int id = 5;
            var result = controller.Delete(id);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        #endregion

    }
}
