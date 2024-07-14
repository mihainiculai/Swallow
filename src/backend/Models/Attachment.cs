using System.ComponentModel.DataAnnotations;

namespace Swallow.Models;

public class Attachment
{
    public int AttachmentId { get; set; }
    [MaxLength(100)]
    public required string FileName { get; set; }
    [MaxLength(100)]
    public required string ContentType { get; set; }
    public required byte[] Content { get; set; }
    public required DateTime UploadDate { get; set; }
    
    public required Guid TripId { get; set; }
    public virtual Trip Trip { get; set; } = null!;
}