using System;
using System.Collections.Generic;
using System.Text;

namespace Goederenontvangst
{
    class ScannedProduct
    {
        String product;
        String count;

        public ScannedProduct(String product, String count)
        {
            this.product = product;
            this.count = count;
        }

        public ScannedProduct setProduct(String product)
        {
            this.product = product;

            return this;
        }

        public ScannedProduct setCount(String count)
        {
            this.count = count;

            return this;
        }

        public String getProduct()
        {
            return this.product;
        }

        public String getCount()
        {
            return this.count;
        }
    }
}
