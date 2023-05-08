#pragma warning disable CS8618
using TheWall.Models;
using Microsoft.EntityFrameworkCore;

namespace TheWall.Context;

public class TheWallContext : DbContext
{
    public TheWallContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Message> Messages { get; set; }
}
