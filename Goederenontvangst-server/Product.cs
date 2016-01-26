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
        private string EAN;
        private string location;
        private string name;

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

        public Product setEAN(string EAN)
        {
            this.EAN = EAN;

            return this;
        }

        public Product setLocation(string location)
        {
            this.location = location;

            return this;
        }

        public Product setName(string name)
        {
            this.name = name;

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

        public string getEAN()
        {
            return this.EAN;
        }

        public string getLocation()
        {
            return this.location;
        }

        public string getName()
        {
            return this.name;
        }
    }
}
