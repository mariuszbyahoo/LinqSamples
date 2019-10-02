﻿using System;
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
            Func<int, int> square = x => x * x;
            Func<int, int, int> add = (x, y) =>
            {
                int temp = x + y;
                return temp;
            };
            Action<int> write = x => Console.WriteLine(x);

            write(square(add(3, 5))); // 8^2 = 64

            var developers = new Employee[]
            {
                new Employee {Id = 1, Name = "Scott" },
                new Employee {Id = 2, Name = "Chris" }
            };

            var sales = new List<Employee>()
            {
                new Employee {Id = 3, Name = "Alex"}
            };

            var query = developers.Where(e => e.Name.Length == 5)
                                               .OrderBy(e => e.Name);

            foreach (var employee in query)
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
