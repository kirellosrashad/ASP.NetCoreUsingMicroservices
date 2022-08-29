using CatalogService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace CatalogService.ViewModel
{
    public class ProductViewModel
    {
        public ProductViewModel(Product product, bool isBase64)
        {
            if (product != null)
            {
                if (isBase64)
                    DecodeImageFromBase64(product);
                else
                    EncodeImageToBase64(product);
            }
        }

        public void EncodeImageToBase64(Product product)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(product.ImageBase64);
            product.ImageBase64 = Convert.ToBase64String(imageArray);
        }

        public void DecodeImageFromBase64(Product product)
        {
            product.ImageBase64 = Image.FromStream(new MemoryStream(Convert.FromBase64String(product.ImageBase64))).ToString();
        }

        ~ProductViewModel()
        {
            
        }
        //public void Dispose()
        //{
        //    this.Dispose();
        //}
    }
}
