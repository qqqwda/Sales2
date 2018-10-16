using Sales.Common.Models;
using Sales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sales.API.Controllers
{
    [Authorize]
    public class CategoriesController : ApiController
    {
        private DataContext db = new DataContext();

        public IQueryable<Category> GetCategories()
        {
            return db.Categories.OrderBy(c => c.Description);
        }
    }
}
