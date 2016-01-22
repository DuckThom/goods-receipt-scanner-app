using System;
using System.Collections.Generic;
using System.Text;

namespace Goederenontvangst_server
{
    class Product
    {
        private string product;
        private string count;
        private bool print = true; 

        public Product setProduct(string product)
        {
            this.product = product;

            return this;
        }

        public Product setCount(string count)
        {
            this.count = count;

            return this;
        }

        public Product setPrint(bool print)
        {
            this.print = print;

            return this;
        }

        public string getProduct()
        {
            return this.product;
        }

        public string getCount()
        {
            return this.count;
        }

        public bool getPrint()
        {
            return this.print;
        }
    }
}
