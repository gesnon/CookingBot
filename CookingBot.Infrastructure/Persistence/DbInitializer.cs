using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingBot.Infrastructure.Persistence
{
    public class DbInitializer
    {
        private readonly CookingBotContext _context;

        public DbInitializer(CookingBotContext context)
        {
            _context = context;
        }

        //public async Task InitializeAsync()
        //{
        //    await _context.Database.MigrateAsync();
        //}
    }
}
