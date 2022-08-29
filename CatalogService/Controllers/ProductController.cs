using CatalogService.Model;
using CatalogService.RabbitMQProducer;
using CatalogService.Repository;
using CatalogService.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IRepository<Product> productRepository;
        ProductViewModel productVM;
        IMessageProducer msgProducer;

        public ProductController(IRepository<Product> _productRepository, IMessageProducer _msgProducer)
        {
            productRepository = _productRepository;
            msgProducer = _msgProducer;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public ActionResult<Product> Get()
        {
            try
            {
                IEnumerable<Product> productList = productRepository.GetAll();
                if (productList == null)
                    return NotFound();

                foreach (Product product in productList)
                {
                    productVM = new ProductViewModel(product, true);
                }
                return Ok(productList);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public ActionResult<Product> Getproduct(int id)
        {
            try
            {
                Product product = productRepository.GetByID(id);
                if (product == null)
                    return NotFound();

                productVM = new ProductViewModel(product, true);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        //POST api/<ProductController>
        [HttpPost]
        //[Route("Post")]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            try
            {
                if (product == null)
                    return BadRequest();

                productVM = new ProductViewModel(product, false);
                productRepository.AddNew(product);
                int result = await productRepository.SaveAsync();

                if (result == 1)
                {
                    msgProducer.SendMessageQueue("New Item Added : " + product.Name);
                    return Ok(product);
                }
                else
                    return StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //[HttpPost]
        //[Route("Post")]
        //public IActionResult Post(Product product)
        //{
        //    productVM = new ProductViewModel(product, false);
        //    productRepository.AddNew(product);
        //    productRepository.SaveAsync();
        //    msgProducer.SendMessageQueue("ProductItem");
        //    return Ok(new { id = product.Name });
        //}

        // PUT api/<ProductController>
        [HttpPut]
        public async Task<ActionResult<Product>> Put(Product product)
        {
            try
            {
                if (product == null)
                    return BadRequest();

                productVM = new ProductViewModel(product, false);
                productRepository.Update(product);
                int result = await productRepository.SaveAsync();
                
                if (result == 1)
                    return Ok(product);
                else
                    return StatusCode((int)HttpStatusCode.NotModified);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return NotFound();

                productRepository.Delete(id);
                int result = await productRepository.SaveAsync();
                
                if (result == 1)
                    return Ok("Prodeuct Deleted");
                else
                    return StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
