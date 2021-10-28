using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Words_Learning.Models
{
    public class UserWordsContext : DbContext
    {
        public DbSet<Words> Words { get; set; }
        public DbSet<User> Users { get; set; }

        public UserWordsContext(DbContextOptions<UserWordsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
