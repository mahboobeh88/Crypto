using System.ComponentModel.DataAnnotations;

namespace Crypto.Infrastructure.Models;
public class BaseExternalApiSettings
{
    [Required]
    public required string BaseUrl { get; set; }
    [Required]
    public required string Key { get; set; }
}
