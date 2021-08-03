using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse
{
    public class Good
    {
        public int ID { get; set; }
        public int ProviderId { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public Good(int id, int providerId, int typeId, string name, double price, double quantity)
        {
            ID = id;
            ProviderId = providerId;
            TypeId = typeId;
            Name = name;
            Price = price;
            Quantity = quantity;
        }
        public Good()
        {

        }
        public void UpdateGood(int providerId, int typeId, string name, double price, double quantity)
        {
            ProviderId = providerId;
            TypeId = typeId;
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }
}
