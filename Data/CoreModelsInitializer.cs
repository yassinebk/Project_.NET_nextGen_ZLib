using Project.Models;
using Newtonsoft.Json; // Nuget Package

namespace Project.Data;

public class CoreModelsInitializer
{
    public static class DbInitializer
    {
        public static void Initialize(CoreModelsDataContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Authors.Any())
            {
                return; // DB has been seeded
            }


            List<Author>? authors = null;
            using (StreamReader r = new StreamReader(@"Data/RawData/authors.json"))
            {
                string json = r.ReadToEnd();
                authors = JsonConvert.DeserializeObject<List<Author>>(json);
            }

            if (authors != null)
                foreach (var s in authors)
                {
                    context.Authors.Add(s);
                }

            context.SaveChanges();

            List<Publisher>? publishers = null;
            using (StreamReader r = new StreamReader(@"Data/RawData/publishers.json"))
            {
                string json = r.ReadToEnd();
                publishers = JsonConvert.DeserializeObject<List<Publisher>>(json);
            }

            if (publishers != null)
                foreach (var p in publishers)
                {
                    context.Publishers.Add(p);
                }

            context.SaveChanges();

            List<Book>? books = null;

            
            using (StreamReader r = new StreamReader(@"Data/RawData/books.json"))
            {
                string json = r.ReadToEnd();
                books = JsonConvert.DeserializeObject<List<Book>>(json);
            }
            
            

            var dbPublishers = context.Publishers.ToList();
            var dbAuthors = context.Authors.ToList();
            var rand = new Random(DateTime.Now.ToString().GetHashCode());
            if (books != null)
                foreach (var e in books)
                {

                    e.BookPublisher = dbPublishers[rand.Next(0,dbPublishers.Count)];
                    e.BookAuthor = dbAuthors[rand.Next(0,dbAuthors.Count)];
                    context.Books.Add(e);
                }

            context.SaveChanges();
        }
    }
}