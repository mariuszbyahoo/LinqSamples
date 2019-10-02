using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {

            var numbers = MyLinq.Random().Where(n => n > 0.5).Take(10);
            foreach (var item in numbers)
            {
                Console.WriteLine(item);
            }

            var movies = new List<Movie>
            {
                new Movie { Title = "The Dark Knight", Rating = 8.9f, Year = 2010 },
                new Movie { Title = "The King's speech", Rating = 8.0f, Year = 2011 },
                new Movie { Title = "Casablanca", Rating = 8.5f, Year = 1995 },
                new Movie { Title = "Star Wars V", Rating = 8.7f, Year = 1990 }
            };
            var query = from movie in movies
                        where movie.Year > 2000
                        orderby movie.Rating descending
                        select movie;

            var enumerator = query.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }
        }
    }
}
