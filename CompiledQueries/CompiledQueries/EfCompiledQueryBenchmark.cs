using Azure;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;//iki farklı Entity Framework sorgu performansını karşılaştırmak için bir benchmark (performans ölçüm) sınıfı 
using BenchmarkDotNet.Reports;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiledQueries
{
    [Config(typeof(Config))]
    public class EfCompiledQueryBenchmark
    {
        private const long Id = 7000;
        private const string name = "Humberto Haley";
        private const int age = 75;
        public class Config : ManualConfig
        {
            public Config()
            {
                SummaryStyle = BenchmarkDotNet.Reports.SummaryStyle.Default.WithRatioStyle(BenchmarkDotNet.Columns.RatioStyle.Trend);
            }
        }

        //[Benchmark(Baseline = true)]
        //public Customer? GetById()
        //{
        //    using var context = new AppDbContext();
        //    return context.GetCustomerById(Id);
        //}

        //[Benchmark]
        //public Customer? GetByIdCompiled()
        //{
        //    using var context = new AppDbContext();
        //    return context.GetCustomerByIdCompiled(Id);
        //}

        //[Benchmark(Baseline = true)]
        //public Customer? GetByIdNoTracking()
        //{
        //    using var context = new AppDbContext();
        //    return context.GetCustomerByIdNoTracking(Id);
        //}

        //[Benchmark]
        //public Customer? GetByIdCompiledNoTracking()
        //{
        //    using var context = new AppDbContext();
        //    return context.GetCustomerByIdNoTrackingCompiled(Id);
        //}


        [Benchmark(Baseline = true)]
        public async Task<Customer?> GetByIdAsync()
        {
            using var context = new AppDbContext();
            return await context.GetCustomerByIdAsync(name,age);
        }

        [Benchmark]
        public async Task<Customer?> GetByIdCompiledAsync()
        {
            using var context = new AppDbContext();
            return await context.GetCustomerByIdCompiledAsync(name,age);
        }
    }
}
