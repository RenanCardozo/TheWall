#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TheWall.Models;

public class MessageBoardView
{
    public User User { get; set; }
    public List<User> Users { get; set; }
    public Comment Comment { get; set; }
    public List<Comment>? Comments { get; set; }
    public Message Message { get; set; }
    public List<Message>? Messages  { get; set; }
}