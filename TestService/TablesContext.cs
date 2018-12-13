using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TestService
{
    public class TablesContext : DbContext
    {
        public TablesContext() : base("DbConnection") { }

        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }

    }
}