using System.Collections.Generic;
using System.Linq;
using Zenwire.Domain;
using Zenwire.Repositories;

namespace Zenwire.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public Product Get(int id)
        {
            return _productRepository.Get.ToList().FirstOrDefault(x => x.Id == id);
        }

        public List<Product> Get()
        {
            return _productRepository.Get.ToList();
        }

        public Product Add(Product product)
        {
            _productRepository.Add(product);
            return product.Id > 0 ? product : null;
        }

        public void Update(Product product)
        {
            _productRepository.Update(product);
        }

        public void Remove(int id)
        {
            Product product = Get(id);
            _productRepository.Remove(product);
        }
    }
}