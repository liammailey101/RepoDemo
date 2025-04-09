﻿namespace RepoDemo.Data.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
    }
}
