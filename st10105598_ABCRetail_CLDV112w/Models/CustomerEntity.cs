namespace st10105598_ABCRetail_CLDV112w.Models
{
    using Azure;
    using Azure.Data.Tables;
    using System;

    public class CustomerEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = string.Empty;
        public string RowKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        public CustomerEntity() { }

        public CustomerEntity(string partitionKey, string rowKey, string name, string email)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Name = name;
            Email = email;
        }
    }
}
