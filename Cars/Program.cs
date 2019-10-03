using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {

            XDocument document;
            XElement cars;

            CreateXml(out document, out cars);
            QueryXml();
        }

        private static void QueryXml()
        {
            var document = XDocument.Load("fuel.xml");
            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";

            var query =
                from element in 
                    document.Element("Cars")?.Elements(ex + "Car") ?? Enumerable.Empty<XElement>()
                where element.Attribute("Manufacturer").Value == "BMW"
                select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
        }

        private static void CreateXml(out XDocument document, out XElement cars)
        {
            var records = ProcessFile("fuel.csv");

            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";

            document = new XDocument();
            cars = new XElement(ns + "Cars",

            from record in records
            select new XElement(ex + "Car",
                        new XAttribute("Name", record.Name),
                        new XAttribute("Combined", record.Combined),
                        new XAttribute("Manufacturer", record.Manufacturer))

            );

            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));

            document.Add(cars);
            document.Save("fuel.xml");
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

    public class CarStatistics
    {
        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }
        internal CarStatistics Accumulate(Car car)
        {
            Count += 1;
            Total += car.Combined;
            Max = Math.Max(Max, car.Combined);
            Min = Math.Min(Min, car.Combined);
            return this;
        }

        public CarStatistics Compute()
        {
            Average = Total / Count;
            return this;
        }

        public int Max { get; set; }
        public int Min { get; set; }
        public double Average { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
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
