﻿using ChatServerHw2.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServerHw2.DataAccess
{
    public class ChatContext : DbContext
    {
        public ChatContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-TALUG4B\SQLEXPRESS;Database=ChatDb;Trusted_Connection=True;");
        }
    }
}
