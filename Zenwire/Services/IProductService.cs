using System.Collections.Generic;
using Zenwire.Domain;

namespace Zenwire.Services
{
    public interface IProductService
    {
        Product Get(int id);
        List<Product> Get();
        Product Add(Product product);
        void Update(Product product);
        void Remove(int id);
    }
}