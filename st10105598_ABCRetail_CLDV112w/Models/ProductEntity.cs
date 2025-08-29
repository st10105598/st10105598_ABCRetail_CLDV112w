using Azure;
using Azure.Data.Tables;
using System;

namespace st10105598_ABCRetail_CLDV112w.Models
{
    public class ProductEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = string.Empty;
        public string RowKey { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public double Price { get; set; }

        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        public ProductEntity() { }

        public ProductEntity(string partitionKey, string rowKey, string productName, double price)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            ProductName = productName;
            Price = price;
        }
    }
}
