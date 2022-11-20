using Microsoft.EntityFrameworkCore;
using NotesApplication.API.Models.Entities;

namespace NotesApplication.API.Data
{
    public class NotesAppDbContext : DbContext
    {
        public NotesAppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Note> NotesApp { get; set; }
    }
}
