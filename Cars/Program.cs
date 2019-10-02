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
                         where car.Manufacturer == "BMW" && car.Year == 2016
                         orderby car.Combined descending, car.Name ascending
                         select new
                         {
                             car.Manufacturer,
                             car.Name,
                             car.Combined
                         };
            // Select statement above uses shortened syntax and returns a new anonymous type
            // object; good for working with files which contains a plenty of columns to 
            // deal with

            // SelectMany below flattens the data; in the case below it selects specific 
            // car's name from the list of car objects. It flattens the multi-dimension collections.
            var result3 = cars.SelectMany(c => c.Name);

                foreach (var character in result3)
                {
                    Console.WriteLine(character);
                }
           

            //foreach (var car in query2.Take(10))
            //{
            //    Console.WriteLine($"{car.Manufacturer} {car.Name} : {car.Combined}");
            //}
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
