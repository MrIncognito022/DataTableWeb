﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Datatables.ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public CustomerController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        public IActionResult GetCustomers()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                 int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //var customerData = (from tempcustomer in context.Customers select tempcustomer);
                var customerData = context.Customers.Skip(skip).Take(pageSize).ToList();
                stopwatch.Stop();
                long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                stopwatch.Start();
                recordsTotal = (from tempcustomer in context.Customers select tempcustomer).Count();
                stopwatch.Stop();
                elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    //customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //}
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    customerData = customerData.Where(m => m.FirstName.Contains(searchValue)
                //                                || m.LastName.Contains(searchValue)
                //                                || m.Contact.Contains(searchValue)
                //                                || m.Email.Contains(searchValue));
                //}
                //recordsTotal = customerData.Count();
                var data = customerData;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}