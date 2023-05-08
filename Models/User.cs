#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TheWall.Models;
using TheWall.Context;
public class User
{
    [Key]
    public int UserId { get; set; }
    
    [Required(ErrorMessage ="{0} is needed")]
    [Display(Name = "First Name")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name can only contain letters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage ="{0} is needed")]
    [Display(Name = "Last Name")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name can only contain letters")]
    public string LastName { get; set; }

    [UniqueEmail]
    [EmailAddress]
    [Required(ErrorMessage ="Email is needed")]
    public string Email { get; set; }

    [Required(ErrorMessage ="Password is needed")]
    [MinLength(8, ErrorMessage ="Password must be at least 8 characters")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Password can only contain letters")]
    // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
    public string? Password { get; set; }


    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    List<Comment> Comments { get; set; } = new List<Comment>();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Though we have Required as a validation, sometimes we make it here anyways
        // In which case we must first verify the value is not null before we proceed
        if(value == null)
        {
            // If it was, return the required error
            return new ValidationResult("Email is required!");
        }
    
        // This will connect us to our database since we are not in our Controller
        TheWallContext _context = (TheWallContext)validationContext.GetService(typeof(TheWallContext));
        // Check to see if there are any records of this email in our database
        if(_context.Users.Any(e => e.Email == value.ToString()))
        {
            // If yes, throw an error
            return new ValidationResult("Email must be unique!");
        } else {
            // If no, proceed
            return ValidationResult.Success;
        }
    }
}
}
