using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Exceptions.CustomExceptions;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations;

public class AttachmentRepository(ApplicationDbContext context) : IAttachmentRepository
{
    public async Task<Dictionary<int, string>> GetByTripIdAsync(Guid tripId)
    {
        var attachments = await context.Attachments
            .Where(a => a.TripId == tripId)
            .Select(a => new { a.AttachmentId, a.FileName })
            .ToDictionaryAsync(a => a.AttachmentId, a => a.FileName);
        
        return attachments;
    }
    
    public async Task<Attachment> GetByIdAsync(int attachmentId)
    {
        return await context.Attachments.FindAsync(attachmentId) ?? throw new NotFoundException("Attachment not found");
    }
    
    public async Task<Attachment> CreateAsync(Attachment attachment)
    {
        await context.Attachments.AddAsync(attachment);
        await context.SaveChangesAsync();
        return attachment;
    }
    
    public async Task DeleteAsync(int attachmentId)
    {
        var attachment = await GetByIdAsync(attachmentId);
        context.Attachments.Remove(attachment);
        await context.SaveChangesAsync();
    }
}