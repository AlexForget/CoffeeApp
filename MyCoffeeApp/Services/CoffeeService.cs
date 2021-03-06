using MyCoffeeApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyCoffeeApp.Services {
    public static class CoffeeService {
        static SQLiteAsyncConnection db;

        static async Task Init() {
            if (db != null) {
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyDatabase.bd");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Coffee>();
        }

        public static async Task AddCoffee(string name, string roaster) {
            await Init();
            var image = "https://www.yesplz.coffee/app/uploads/2020/11/emptybag-min.png";

            var coffee = new Coffee {
                Name = name,
                Roaster = roaster,
                Image = image
            };

            var id = await db.InsertAsync(coffee);
        }

        public static async Task RemoveCoffee(int id) {
            await Init();

            await db.DeleteAsync<Coffee>(id);
        }

        public static async Task<IEnumerable<Coffee>> GetCoffee() {
            await Init();

            var coffee = await db.Table<Coffee>().ToListAsync();
            return coffee;
        }

        public static async Task<Coffee> GetCoffee(int id) {
            await Init();

            var coffee = await db.Table<Coffee>()
                .FirstOrDefaultAsync(c => c.Id == id);

            return coffee;
        }
    }
}
