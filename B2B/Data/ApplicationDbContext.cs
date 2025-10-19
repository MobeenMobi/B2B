﻿using B2B.Models;
using Microsoft.EntityFrameworkCore;

namespace B2B.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        // Example table
        public DbSet<Users> Users { get; set; }
    }
}
