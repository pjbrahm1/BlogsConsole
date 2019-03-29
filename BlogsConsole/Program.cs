using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            string choice = "";
            do {

                try
                {
                    // Give user a choice
                    Console.WriteLine("What would you like to do? ");
                    Console.Write("(1) Display all blogs (2) Add a blog or (3) Create a post:   ");
                    choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        // Display all Blogs and Posts from the database
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId + "\t" + item.Name);

                            // Display all Blogs from the database
                            var showPostDb = new BloggingContext();
                            var blogger = showPostDb.Posts.Where(b => b.BlogId.Equals(item.BlogId));
  
                            foreach (var posts in blogger)
                            {
                                Console.WriteLine("\t\t" + posts.Title);
                                Console.WriteLine("\t\t\t"+  posts.Content);
                            }
                        }
                    }
                    else if (choice == "2")
                    {
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };

                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }
                    else if (choice == "3")
                    {
                        // Display all Blogs from the database
                        var showDb = new BloggingContext();
                        var query = showDb.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId + "\t " + item.Name);
                        }
                        // Select the Blog
                        Console.Write("Select the id of the blog you want to post to? ");
                        int blogId = int.Parse(Console.ReadLine());
                        var blogCntxtDb = new BloggingContext();
                        var name = blogCntxtDb.Blogs.Where(b => b.BlogId.Equals(blogId));

                        foreach (var thing in name)
                        {
                            Console.WriteLine("Title your post for " + thing.Name + "\t" + thing.BlogId);
                            var postTitle = Console.ReadLine();

                            Console.WriteLine("Enter your post to " + thing.Name);
                            var postContent = Console.ReadLine();

               //             var blogDb = new Blog { Name = thing.Name };
                            var postDb = new Post { Title = postTitle, Content = postContent, BlogId = thing.BlogId };

                            var db = new BloggingContext();
                            db.AddPost(postDb);
                        }     
                    
                        logger.Info("Post added - {name}", name);

                    }

                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            } while (choice != "");
            // Give error message
            Console.Write("You've made an invalid selection. ");

            Console.WriteLine("Press enter to quit");
            string x = Console.ReadLine();

            logger.Info("Program ended");
        }
    }
}
