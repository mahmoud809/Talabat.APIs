using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order() // For EFCore Migration : In Migration EFcore Chaine on Auto Generated Parameterless Constructor
        {
            
        }

        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; set; }
       
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
       
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
       
        public Address ShippingAddress { get; set; }
       
        public DeliveryMethod DeliveryMethod { get; set; } //Navigational Property [ONE] Cuz I have a property From Mapped Table in my Class.
       
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); //Navigational Property [Many].

        public decimal SubTotal { get; set; }
        
        //[NotMapped]
        //public decimal Total { get => SubTotal + DeliveryMethod.Cost; } // Computed Attribute

        public decimal GetTotal()
            => SubTotal+ DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; } = string.Empty;
    }
}
