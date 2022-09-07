using System.ComponentModel.DataAnnotations;

namespace ResourceMagnat.Models;

public class Session
{
    [Key] 
    public string Id { get; set; }

    public int UserId { get; set; }

    public DateTime LastAccess { get; set; }
}