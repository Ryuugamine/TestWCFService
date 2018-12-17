using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TestService.Models;

namespace TestService
{
    public class TablesContext : DbContext
    {
        public TablesContext() : base("DbConnection") { }

        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BooksInOrder> BooksInOrders { get; set; }

        public DbSet<Order> Orders { get; set; }

    }
}