using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.DTOs.Preference;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations;

public class UserActionRepository(ApplicationDbContext context) : IUserActionRepository
{
    public async Task CreateAsync(Guid userId, int attractionId, int actionType)
    {
        var userAction = new UserAction
        {
            UserId = userId,
            AttractionId = attractionId,
            UserActionTypeId = actionType
        };
        
        var existingUserAction = await context.UserActions.FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AttractionId == attractionId);
        if (existingUserAction != null)
        {
            existingUserAction.UserActionTypeId = actionType;
            context.UserActions.Update(existingUserAction);
        }
        else
        {
            await context.UserActions.AddAsync(userAction);
        }
        
        await context.SaveChangesAsync();
    }
    
    public async Task<int> GetUserPreferenceCountAsync(User user)
    {
        return await context.UserActions.CountAsync(ua => ua.UserId == user.Id);
    }
    
    public async Task<int> GetUserPreference(User user, int attractionId)
    {
        return await context.UserActions.Where(ua => ua.UserId == user.Id && ua.AttractionId == attractionId)
            .Select(ua => ua.UserActionTypeId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<UserCategoryPreference>> GetNormalizedUserCategoryPreferencesAsync(User user)
    {
        var query = from ua in context.UserActions
            join uat in context.UserActionTypes on ua.UserActionTypeId equals uat.UserActionTypeId
            join a in context.Attractions on ua.AttractionId equals a.AttractionId
            where ua.UserId == user.Id
            from ac in a.AttractionCategories
            group uat by ac.AttractionCategoryId
            into grouped
            select new
            {
                AttractionCategoryId = grouped.Key,
                Score = grouped.Sum(g => g.Points)
            };

        var categoryScores = await query.ToListAsync();

        var maxScore = categoryScores.Max(cs => cs.Score);
        var minScore = categoryScores.Min(cs => cs.Score);
        
        List<UserCategoryPreference> normalizedCategoryScores = new(categoryScores.Count + 1)
        {
            new UserCategoryPreference
            {
                AttractionCategoryId = 0,
                Score = (decimal)(0 - minScore) / (maxScore - minScore)
            }
        };

        normalizedCategoryScores.AddRange(categoryScores.Select(cs => new UserCategoryPreference
        {
            AttractionCategoryId = cs.AttractionCategoryId,
            Score = (decimal)(cs.Score - minScore) / (maxScore - minScore)
        }));
        
        return normalizedCategoryScores;
    }
}