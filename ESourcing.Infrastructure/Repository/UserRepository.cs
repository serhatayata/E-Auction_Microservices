using ESourcing.Core.Entities;
using ESourcing.Core.Repositories;
using ESourcing.Infrastructure.Data;
using ESourcing.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESourcing.Infrastructure.Repository
{
    public class UserRepository:Repository<AppUser>,IUserRepository
    {
        private readonly WebAppContext _context;

        public UserRepository(WebAppContext context):base(context)
        {
            _context = context;
        }

    }
}
