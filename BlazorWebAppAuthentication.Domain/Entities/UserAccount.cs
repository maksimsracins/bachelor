using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorWebAppAuthentication.Domain.Entities;
[Table("UserAccount")]
public class UserAccount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserAccountId { get; set; }
    
    [MaxLength(100)]
    public string? Username { get; set; }
    
    [MaxLength(100)]
    public string? Password { get; set; }
    
    [MaxLength(20)]
    public string? Role { get; set; }
    
    [MaxLength(100)]
    public string? Email { get; set; }
}