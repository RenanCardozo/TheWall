#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TheWall.Models;

public class Login
{
    [Required (ErrorMessage = "Email is required")]
    [EmailAddress]
    [Display (Name = "Email Address :")]
    public string LEmail { get; set; }

    [Required (ErrorMessage = "Password is required")]
    [MinLength (8, ErrorMessage = "Password must be at least 8 characters")]
    [DataType (DataType.Password)]
    [Display (Name = "Password :")]
    public string LPassword { get; set; }
}
