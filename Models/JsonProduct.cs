using System.Collections.Generic;

namespace Import.Models
{

    public class JsonProduct
    {
        public List<Product>? products { get; set; }
    }

    public class Product
    {
        public IList<string>? Categories { get; set; }
        public string? Twitter { get; set; }
        public string? Title { get; set; }
    }
}
