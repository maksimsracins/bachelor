using System.ComponentModel.DataAnnotations;

namespace BlazorWebAppAuthentication.Client.Models.ViewModels;

public class LoginViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide email name.")]
    public string? Email { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide username.")]
    public string? Username { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide password.")]
    public string? Password { get; set; }
    public int CustomerId { get; set; }
}