﻿using Microsoft.EntityFrameworkCore;
using Users.Core.Database;
using Users.Core.Database.Entities.Identity;
using Users.Core.Repositories.Interfaces;

namespace Users.Core.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly BaseDbContext _dbContext;

    public UsersRepository(BaseDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<ScamUser?> GetAsync(int id)
    {
        return _dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));
    }
}