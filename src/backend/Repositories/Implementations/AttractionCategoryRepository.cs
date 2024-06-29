using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations
{
    public class AttractionCategoryRepository(ApplicationDbContext context) : IAttractionCategoryRepository
    {
        public async Task<IEnumerable<AttractionCategory>> GetAllAsync()
        {
            return await context.AttractionCategories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<AttractionCategory?> GetByNameAsync(string name)
        {
            return await context.AttractionCategories.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<AttractionCategory> GetOrCreateAsync(string name)
        {
            var category = await GetByNameAsync(name);

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

            foreach (var name in names)
            {
                categories.Add(await GetOrCreateAsync(name));
            }

            return categories;
        }

        private async Task<AttractionCategory> CreateAsync(AttractionCategory entity)
        {
            await context.AttractionCategories.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
