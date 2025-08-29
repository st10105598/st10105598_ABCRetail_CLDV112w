using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using st10105598_ABCRetail_CLDV112w.Models;
using st10105598_ABCRetail_CLDV112w.Services;
using System;
using System.Threading.Tasks;

namespace st10105598_ABCRetail_CLDV112w.Controllers
{
    public class CustomerController : Controller
    {
        private readonly TableStorageService _tableService;

        public CustomerController(TableStorageService tableService)
        {
            _tableService = tableService ?? throw new ArgumentNullException(nameof(tableService));
        }

        // Display Customers (GET)
        public IActionResult Index()
        {
            var customers = _tableService.GetCustomers();
            return View(customers);
        }

        // Add New Customer (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Add New Customer (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomerEntity customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            customer.PartitionKey = "Customer";
            customer.RowKey = Guid.NewGuid().ToString();

            _tableService.UpsertCustomer(customer);

            return RedirectToAction(nameof(Index));
        }
    }
}
