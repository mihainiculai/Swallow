using Swallow.Models;

namespace Swallow.Repositories.Interfaces;

public interface IAttachmentRepository
{
    Task<Dictionary<int, string>> GetByTripIdAsync(Guid tripId);
    Task<Attachment> GetByIdAsync(int attachmentId);
    Task<Attachment> CreateAsync(Attachment attachment);
    Task DeleteAsync(int attachmentId);
}