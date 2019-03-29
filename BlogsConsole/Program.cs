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
                    Console.WriteLine("\nWhat would you like to do? ");
                    Console.Write("\n(1) Display all blogs (2) Add a blog (3) Create a post (4) Display posts: ");
                    choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        // Display all Blogs and Posts from the database
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);

                        Console.WriteLine("\nAll blogs in the database:");
                        Console.WriteLine();
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId + ")\t" + item.Name);
                        }
                        Console.WriteLine();
                        logger.Info("\nBlogs displayed ");
                    }
                    else if (choice == "2")
                    {
                        // Create and save a new Blog
                        Console.Write("\nEnter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };

                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        Console.WriteLine();
                        logger.Info("\nBlog added - {name}", name);
                    }
                    else if (choice == "3")
                    {
                        // Display all Blogs from the database
                        var showDb = new BloggingContext();
                        var query = showDb.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("\nAll blogs in the database:");
                        Console.WriteLine();
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId + ")\t " + item.Name);
                        }
                        // Select the Blog
                        Console.Write("\nSelect the id of the blog you want to post to? ");
                        int blogId = int.Parse(Console.ReadLine());
                        var blogCntxtDb = new BloggingContext();
                        var name = blogCntxtDb.Blogs.Where(b => b.BlogId.Equals(blogId));

                        foreach (var thing in name)
                        {
                            Console.Write("\nTitle your post for " + thing.Name + ": ");
                            var postTitle = Console.ReadLine();

                            Console.Write("\nEnter your post to " + postTitle + ": ");
                            var postContent = Console.ReadLine();

                            var postDb = new Post { Title = postTitle, Content = postContent, BlogId = thing.BlogId };

                            var db = new BloggingContext();
                            db.AddPost(postDb);
                        }
                        Console.WriteLine();
                        logger.Info("\nPost added - {name}", name);
                    }
                    else if (choice == "4")
                    {
                        // Display all Blogs from the database
                        var showDb = new BloggingContext();
                        var query = showDb.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine();

                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId + ")\t Posts from " + item.Name);
                        }

                        // Select the Blog
                        Console.Write("\nSelect the blog's post to display: ");
                        int blogId = int.Parse(Console.ReadLine());
                        Console.WriteLine();

                        var blogCntxtDb = new BloggingContext();
                        var name = blogCntxtDb.Blogs.Where(b => b.BlogId.Equals(blogId));

                        // Display all Blogs from the database
                        var showPostDb = new BloggingContext();
                        var blogger = showPostDb.Posts.Where(b => b.BlogId.Equals(blogId));
                        foreach (var thing in name)
                        {
                            Console.WriteLine("\nBlog: " + thing.Name);
                        }
                        foreach (var posts in blogger)
                        {
                            Console.WriteLine("\n      Title: " + posts.Title);
                            Console.WriteLine("\n             Content: " + posts.Content);
                        }
                        Console.WriteLine();
                        logger.Info("\nPosts displayed - {name}", name);
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
