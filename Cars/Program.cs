﻿using System;
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
            var manufacturers = ProcessManufacturers("manufacturers.csv");
            // Query syntax
            var query =
                from car in cars
                group car by car.Manufacturer into carGroup
                select new
                {
                    Name = carGroup.Key,
                    Max = carGroup.Max(c => c.Combined),
                    Min = carGroup.Min(c => c.Combined),
                    Avg = carGroup.Average(c => c.Combined)
                } into result
                orderby result.Max descending
                select result;

            // Extension method syntax, in this case a little bit simplier.

            var query2 =
                manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer,  
                            (m, g) =>
                                new {
                                    Manufacturer = m,
                                    Cars = g
                                })
                .GroupBy(m => m.Manufacturer.Headquaters); // same as above, flattening both collections into one.

            foreach (var result in query)
            {

                Console.WriteLine(result.Name);
                Console.WriteLine($"\t Max: {result.Max}");
                Console.WriteLine($"\t Min: {result.Min}");
                Console.WriteLine($"\t Avg: {result.Avg}");
            }
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query =
                File.ReadAllLines(path)
                    .Where(L => L.Length > 1)
                    .Select(L => 
                    {
                        var columns = L.Split(",");
                        return new Manufacturer
                        {
                            Name = columns[0],
                            Headquaters = columns[1],
                            Year = int.Parse(columns[2])
                        };
                    });
            return query.ToList();
        }

        private static List<Car> ProcessFile(string path)
        {
            // Query syntax:
            //var query =
            //    from line in File.ReadAllLines(path).Skip(1)
            //    where line.Length > 1
            //    select Car.ParseFromCsv(line);

            // Extension method syntax
            var query2 =
                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(line => line.Length > 1)
                    .ToCar();

            return query2.ToList();
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                yield return new Car // without yield statement the code will be trying to 
                // return a first new Car obj and it generates a compiler error
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
    }
}
