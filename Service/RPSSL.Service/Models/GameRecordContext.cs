﻿using Microsoft.EntityFrameworkCore;

namespace Mmicovic.RPSSL.Service.Models
{
    public class GameRecordContext : DbContext
    {
        public GameRecordContext() : base() { }
        public GameRecordContext(DbContextOptions<GameRecordContext> options)
            : base(options)
        { }

        public virtual DbSet<GameRecord> GameRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameRecord>(entity =>
            {
                entity.ToTable("GameRecords");
                entity.HasKey(e => e.Id);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
