using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Data.Tables;
using st10105598_ABCRetail_CLDV112w.Models;

namespace st10105598_ABCRetail_CLDV112w.Services
{
    public class TableStorageService
    {
        private readonly TableClient _customerTable;
        private readonly TableClient _productTable;

        public TableStorageService(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
            }

            _customerTable = new TableClient(connectionString, "CustomerProfiles");
            _productTable = new TableClient(connectionString, "ProductInfo");

            // Create tables if they do not exist (sync method)
            _customerTable.CreateIfNotExists();
            _productTable.CreateIfNotExists();
        }

        // Insert or update a customer entity synchronously
        public void UpsertCustomer(CustomerEntity customer)
        {
            _customerTable.UpsertEntity(customer);
        }

        // Insert or update a product entity synchronously
        public void UpsertProduct(ProductEntity product)
        {
            _productTable.UpsertEntity(product);
        }

        // Get all customers synchronously
        public List<CustomerEntity> GetCustomers()
        {
            return _customerTable.Query<CustomerEntity>().ToList();
        }

        // Get all products synchronously
        public List<ProductEntity> GetProducts()
        {
            return _productTable.Query<ProductEntity>().ToList();
        }

        // Insert or update a customer entity asynchronously
        public async Task UpsertCustomerAsync(CustomerEntity customer)
        {
            await _customerTable.UpsertEntityAsync(customer);
        }

        // Insert or update a product entity asynchronously
        public async Task UpsertProductAsync(ProductEntity product)
        {
            await _productTable.UpsertEntityAsync(product);
        }

        // Get all customers asynchronously
        public async Task<List<CustomerEntity>> GetCustomersAsync()
        {
            var customers = new List<CustomerEntity>();
            await foreach (var customer in _customerTable.QueryAsync<CustomerEntity>())
            {
                customers.Add(customer);
            }
            return customers;
        }

        // Get all products asynchronously
        public async Task<List<ProductEntity>> GetProductsAsync()
        {
            var products = new List<ProductEntity>();
            await foreach (var product in _productTable.QueryAsync<ProductEntity>())
            {
                products.Add(product);
            }
            return products;
        }
    }
}
