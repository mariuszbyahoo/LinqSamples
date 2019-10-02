using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = new List<Movie>
            {
                new Movie { Title = "The Dark Knight", Rating = 8.9f, Year = 2010 },
                new Movie { Title = "The King's speech", Rating = 8.0f, Year = 2011 },
                new Movie { Title = "Casablanca", Rating = 8.5f, Year = 1995 },
                new Movie { Title = "Star Wars V", Rating = 8.7f, Year = 1990 }
            };

            var query = movies.Filter(m => m.Year > 2000);
            // later LINQ queries can be either modified:
            query = query.Take(1);

            var enumerator = query.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }
        }
    }
}
