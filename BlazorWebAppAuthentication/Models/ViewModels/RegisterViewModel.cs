using System.ComponentModel.DataAnnotations;

namespace BlazorWebAppAuthentication.Models.ViewModels;

public class RegisterViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide user name.")]
    public string? Username { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide email.")]
    public string? Email { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide password.")]
    public string? Password { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please confirm password.")]
    public string? ConfirmPassword { get; set; }
}