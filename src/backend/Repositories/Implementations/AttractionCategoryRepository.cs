using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations
{
    public class AttractionCategoryRepository(ApplicationDbContext context) : IRepository<AttractionCategory, int>
    {
        public async Task<AttractionCategory> CreateAsync(AttractionCategory entity)
        {
            await context.AttractionCategories.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<AttractionCategory?> DeleteAsync(int id)
        {
            AttractionCategory? entity = await context.AttractionCategories.FindAsync(id);

            if (entity != null)
            {
                context.AttractionCategories.Remove(entity);
                await context.SaveChangesAsync();
                return entity;
            }

            return null;
        }

        public async Task<IEnumerable<AttractionCategory>> GetAllAsync()
        {
            return await context.AttractionCategories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<AttractionCategory?> GetByIdAsync(int id)
        {
            return await context.AttractionCategories.FindAsync(id);
        }

        public async Task<AttractionCategory?> GetByNameAsync(string name)
        {
            return await context.AttractionCategories.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<AttractionCategory> GetOrCreateAsync(string name)
        {
            AttractionCategory? category = await GetByNameAsync(name);

            if (category == null)
            {
                category = new AttractionCategory { Name = name };
                await CreateAsync(category);
            }

            return category;
        }

        public async Task<List<AttractionCategory>> GetOrCreateAsync(IEnumerable<string> names)
        {
            List<AttractionCategory> categories = [];

            foreach (string name in names)
            {
                categories.Add(await GetOrCreateAsync(name));
            }

            return categories;
        }

        public async Task<AttractionCategory?> UpdateAsync(AttractionCategory entity)
        {
            AttractionCategory? existingEntity = await context.AttractionCategories.FindAsync(entity.AttractionCategoryId);

            if (existingEntity != null)
            {
                context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await context.SaveChangesAsync();
                return existingEntity;
            }

            return null;
        }
    }
}
