#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TheWall.Models;
using TheWall.Context;

public class Comment
{
    [Key]
    public int CommentId { get; set; }

    [Required]
    [MinLength(5, ErrorMessage = "Message must be at least 5 characters long.")]
    [Display(Name ="Post a Message")]
    public string? CommentText { get; set; }

    public int UserId { get; set; }
    public int MessageId { get; set; }
    public User? Creator { get; set; } 
    public Message? Messages { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}