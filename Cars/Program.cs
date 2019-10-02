using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Cars
{
    class Program
    {
        // CSVs moved to ../Bin/Debug/netcoreapp 3.0/..
        static void Main(string[] args)
        {
            // Changing the Thread's culture Needed because of the CSV's format
            CultureInfo culture = new CultureInfo("en-US"); 
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var cars = ProcessFile("fuel.csv");

            // Quantifying Data in LINQ:
            var result = cars.Any(c => c.Manufacturer == "Ford"); // Returns True/False
            var result2 = cars.All(c => c.Manufacturer == "Ford"); // Returns True/False


            Console.WriteLine(result);
            Console.WriteLine(result2);

            //Query syntax
            var query2 = from car in cars
                         where car.Manufacturer == "BMW"  && car.Year == 2016
                         orderby car.Combined descending, car.Name ascending
                         select car;

            foreach (var car in query2.Take(10))
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");
            }
        }

        private static List<Car> ProcessFile(string path)
        {
            // Query syntax:
            var query =
                from line in File.ReadAllLines(path).Skip(1)
                where line.Length > 1
                select Car.ParseFromCsv(line);
            
            // Extension method syntax
            var query2 =
                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(line => line.Length > 1)
                    .Select(Car.ParseFromCsv)
                    .ToList();

            return query.ToList();
        }
    }
}
