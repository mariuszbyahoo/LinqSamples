using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
//using Features.Linq;

namespace LinqSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee {Id = 1, Name = "Scott" },
                new Employee {Id = 2, Name = "Chris" }
            };

            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee {Id = 3, Name = "Alex"}
            };

            foreach (var employee in developers.Where
                (e => e.Name.StartsWith("S")))
            {
                Console.WriteLine(employee.Name);
            }
        }

        private static bool NameStartsWithS(Employee employee)
        {
            return employee.Name.StartsWith("S");
        }
    }
}
