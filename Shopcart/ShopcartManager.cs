namespace Shopcart
{    public interface IShopcart
    {
        void AddProduct(Product product);
        decimal GetTotal();
        int ProductCount();
    }

    public class ShopcartManager
    {
        private readonly IShopcart _shopcart;

        public ShopcartManager(IShopcart shopcart)
        {
            _shopcart = shopcart;
        }

        public void AddItemsAndCheck(Product[] products)
        {
            foreach (var product in products)
            {
                _shopcart.AddProduct(product);
            }
        }

        public bool IsOverLimit(decimal limit)
        {
            return _shopcart.GetTotal() > limit;
        }
    }
}
