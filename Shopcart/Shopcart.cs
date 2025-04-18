namespace Shopcart
{
    public class Product
    {
        public string Name { get; }
        public decimal Price { get; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    public class Shopcart : IShopcart
    {
        private readonly List<Product> _products = new();

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public decimal GetTotal()
        {
            return _products.Sum(p => p.Price);
        }

        public int ProductCount()
        {
            return _products.Count;
        }
    }
}
