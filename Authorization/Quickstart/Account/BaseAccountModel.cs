using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Quickstart.UI;

public abstract class BaseAccountModel : RedirectViewModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
}