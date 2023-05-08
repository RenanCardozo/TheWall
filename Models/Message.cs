#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TheWall.Models;
using TheWall.Context;

public class Message
{
    [Key]
    public int MessageId { get; set; }

    [Required]
    [MinLength(2, ErrorMessage ="Comment must be at least 5 characters long.")]
    public string MessageText { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public int UserId { get; set; }
    public User? Creator { get; set; }

    public List<Comment> Commenter { get; set; } = new List<Comment>();
}