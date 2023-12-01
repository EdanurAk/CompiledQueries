using Bogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiledQueries
{
    public class AppDbContext : DbContext
    {
        //EF.CompileAsyncQuery ile derlenmiş bir sorgu oluşturuyor.
        private static readonly Func<AppDbContext, long, Customer?> GetById =
            EF.CompileQuery((AppDbContext context, long id) =>
            context.Set<Customer>().FirstOrDefault(p => p.Id == id));


        private static readonly Func<AppDbContext, long, Customer?> GetByIdNoTracking =
            EF.CompileQuery((AppDbContext context, long id) =>
            context.Set<Customer>().AsNoTracking().FirstOrDefault(p => p.Id == id));


        private static readonly Func<AppDbContext, string,int, Task<Customer?>> GetByIdAsync =
             EF.CompileAsyncQuery((AppDbContext context, string name, int age) =>
             context.Set<Customer>().FirstOrDefault(p => p.Name == name && p.Age==age ));


        // Derlenmiş sorgusuz bir şekilde müşteriyi id'ye göre asenkron olarak getirir.
        public Customer GetCustomerById(long id)
        {
            return Set<Customer>().FirstOrDefault(p => p.Id == id);
        }

        // Derlenmiş sorguyu kullanarak müşteriyi id'ye göre asenkron olarak getirir.
        public Customer? GetCustomerByIdCompiled(long id)
        {
            return  GetById(this, id);
        }

        //NoTracking

        public Customer GetCustomerByIdNoTracking(long id)
        {
            return Set<Customer>().AsNoTracking().FirstOrDefault(p => p.Id == id);
        }

        public Customer? GetCustomerByIdNoTrackingCompiled(long id)
        {
            return GetByIdNoTracking(this, id);
        }

        public async Task<Customer?> GetCustomerByIdAsync(string name, int age)
        {
            return await Set<Customer>().FirstOrDefaultAsync(p => p.Name == name && p.Age == age);
        }


        public async Task<Customer?> GetCustomerByIdCompiledAsync(string name,int age)
        {
            return await GetByIdAsync(this, name,age);
        }


        // Veritabanı modelini oluşturan metot
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(builder =>
            {
              // Veritabanına eklemek için random müşteri verisi oluşturan Faker kullanılıyor.(Bogus kütüphanesinden)
                var faker = new Faker();
                var customers = new List<Customer>();
                for (var i = 0; i < 10_000; i++)
                {
                    customers.Add(new Customer
                    {
                        Id = i + 1,
                        Age = faker.Random.Number(10, 100),
                        Name = faker.Name.FullName()
                    });
                }
                // Müşteri verileri veritabanına ekleniyor.
                builder.HasData(customers);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=PC_3829\SQLEXPRESS;Initial Catalog=EFCompiledQuery;Integrated Security=True;Connect Timeout=30;Encrypt=False;");

        }
    }
}
