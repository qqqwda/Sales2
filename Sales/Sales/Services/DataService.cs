using Sales.Common.Models;
using Sales.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sales.Services
{
    public class DataService
    {
        private SQLiteAsyncConnection connection;

        public DataService()
        {
            this.OpenOrCreateDB();
        }

        private async void OpenOrCreateDB()
        {
            var databasePath = DependencyService.Get<IPathService>().GetDatabasePath(); //Obtiene la ruta, si no la tiene, la crea
            this.connection = new SQLiteAsyncConnection(databasePath);

            await connection.CreateTableAsync<Product>().ConfigureAwait(false);
            
        }

        public async Task Insert<T>(T model)
        {
            await this.connection.InsertAsync(model);
        }

        public async Task InsertAll<T>(List<T> models)
        {
            await this.connection.InsertAllAsync(models);
        }

        public async Task Update<T>(T model)
        {
            await this.connection.UpdateAsync(model);
        }

        public async Task Update<T>(List<T> models)
        {
            await this.connection.UpdateAllAsync(models);
        }

        public async Task Delete<T>(T model)
        {
            await this.connection.DeleteAsync(model);
        }

        public async Task DeleteAllProducts()
        {
            await this.connection.QueryAsync<Product>("delete from [Product]");
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var query = await this.connection.QueryAsync<Product>("select * from [Product]"); // Hace la query y la guarda en la variable query
            var array = query.ToArray();// transforma la query a array

            var list = array.Select(
                p => new Product
                {
                    Description = p.Description,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,
                }).ToList(); // transforma el array a list
            return list;
        }
    }
}
