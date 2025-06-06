﻿namespace RepoDemo.DTO
{
    public class CustomerDetail : CustomerSummary
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public ICollection<OrderDetail> Orders { get; set; } = [];
    }
}
