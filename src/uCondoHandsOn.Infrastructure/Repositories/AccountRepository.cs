using Microsoft.EntityFrameworkCore;
using uCondoHandsOn.Infra.Context;
using uCondoHandsOn.Domain.Entities;
using uCondoHandsOn.Domain.Enums;
using uCondoHandsOn.Domain.Interfaces;


namespace uCondoHandsOn.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        private readonly ApplicationDbContext _context;
        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

    
        public async ValueTask<IEnumerable<Account>> GetAsync(string search, AccountType? type, bool? allowEntries)
        {
            var query = _context.Accounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.Code.Contains(search) || x.Name.Contains(search));
                
            if (type.HasValue)
                query = query.Where(x => x.Type == type.Value);

            if (allowEntries.HasValue)
                query = query.Where(x => x.AllowEntries == allowEntries.Value);

            return await query
                .AsNoTracking()
                .ToListAsync();
        }


        public async ValueTask<Account> GetAsync(string code)
        {
            return await _context.Accounts
                .Include(x => x.Children)
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async ValueTask<Account> InsertAsync(Account entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(string code)
        {
            var entity = await GetAsync(code);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
        }

        public async ValueTask<bool> IsDuplicatedAsync(string code)
        {
            return await _context.Accounts.AnyAsync(x => x.Code == code);
        }

        
    }
}