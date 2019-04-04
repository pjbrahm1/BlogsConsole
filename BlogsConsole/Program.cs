using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace BlogsConsole
{    // Programming Assignment 10 - EF Blogs & Posts Review
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started ");
            string choice = "";
            bool valid = false;
            do {

                try
                {
                    // Give user a choice
                    Console.WriteLine("\nEnter your selection: ");
                    Console.Write("\n1) Display all blogs \n2) Add a blog \n3) Create a post \n4) Display posts");
                    Console.WriteLine("\nEnter q to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info("Option " + choice + " selected");

                    if (choice == "1")
                    {
                        // Display all Blogs and Posts from the database
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        var total = db.Blogs.Count();
                        Console.WriteLine("\n" + total + " Blogs displayed ");

                        Console.WriteLine();
                        foreach (var item in query)
                        {
                            Console.WriteLine("  " + item.Name);
                        }

                    }
                    else if (choice == "2")
                    {
                        // Create and save a new Blog
                        Console.Write("\nEnter a name for a new Blog: ");
                        var name = Console.ReadLine();
                        while (name.Length == 0)
                        {
                            Console.WriteLine("Blog name cannot be blank");
                            Console.Write("\nEnter a name for a new Blog or q to quit: ");
                            name = Console.ReadLine();
                        }
                        if (name == "q" || name == "Q")
                        { }
                        else
                        {
                            var blog = new Blog { Name = name };

                            var db = new BloggingContext();
                            db.AddBlog(blog);
                            Console.WriteLine();
                            logger.Info("\nBlog added - {name}", name);
                        }
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
                            Console.WriteLine(item.BlogId + ") " + item.Name);
                        }
                        // Select the Blog
                        Console.Write("\nSelect the id of the blog you want to post to? ");
                        int blogId = int.Parse(Console.ReadLine());
                        var blogCntxtDb = new BloggingContext();
                        var name = blogCntxtDb.Blogs.Where(b => b.BlogId.Equals(blogId));

                        foreach (var item in query)
                        {
                            if (item.BlogId == blogId)
                            {
                                valid = true;
                            }
                        }

                        if (!valid)
                        {
                            Console.WriteLine("You have made an invalid selection");
                        }
                        else
                        {
                            foreach (var thing in name)
                            {
                                Console.Write("\nTitle your post for " + thing.Name + ": ");
                                var postTitle = Console.ReadLine();
                                while (postTitle.Length == 0)
                                {
                                    Console.WriteLine("Post title can not be blank");
                                    Console.Write("\nTitle your post for " + thing.Name + " or enter q to quit: ");
                                    postTitle = Console.ReadLine();
                                }
                                if (postTitle == "q" || postTitle == "Q")
                                {
                                }
                                else
                                {
                                    Console.Write("\nEnter your post to " + postTitle + ": ");
                                    var postContent = Console.ReadLine();
                                    while (postContent.Length == 0)
                                    {
                                        Console.WriteLine("Post content can not be blank");
                                        Console.Write("\nEnter your post to " + postTitle + ":");
                                        postContent = Console.ReadLine();
                                    }
                                    var postDb = new Post { Title = postTitle, Content = postContent, BlogId = thing.BlogId };

                                    var db = new BloggingContext();
                                    db.AddPost(postDb);

                                    logger.Info("\nPost added - {postTitle}", postTitle);
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                    else if (choice == "4")
                    {
                        // Display all Blogs from the database
                        var showDb = new BloggingContext();
                        var query = showDb.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine();

                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId + ") Posts from " + item.Name);
                        }

                        // Select the Blog
                        Console.Write("\nSelect the blog's posts to display: ");
                        int blogId = int.Parse(Console.ReadLine());
                        Console.WriteLine();
                       
                        foreach (var item in query)
                        {
                            if (item.BlogId == blogId)
                            { valid = true; }
                        }

                        if (!valid)
                        {
                            Console.WriteLine("You have made an invalid selection");
                        }
                        else
                        {
                            var blogCntxtDb = new BloggingContext();
                            var name = blogCntxtDb.Blogs.Where(b => b.BlogId.Equals(blogId));

                            // Display all posts from the database
                            var showPostDb = new BloggingContext();
                            var blogger = showPostDb.Posts.Where(b => b.BlogId.Equals(blogId));
                            var total = blogger.Count();
                            Console.WriteLine("\n" + total + " Posts displayed ");

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
                            foreach (var thing in name)
                            {
                                logger.Info("\nPosts displayed - {name}", thing.Name);
                            }
                        }
                    } else
                    {
                        Console.WriteLine("Invalid selection. Choose options 1 - 4 or q to quit");
                    }
                }  
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            } while (choice != "q" && choice != "Q");
    
            logger.Info("Program ended");
        }
    }
}
