﻿using Microsoft.EntityFrameworkCore;

namespace SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;

public static class InMemoryProviderRelationshipsDataContext
{
    public static ProviderRelationshipsDataContext CreateInMemoryContext(string contextName = "")
    {
        contextName = string.IsNullOrEmpty(contextName) ? Guid.NewGuid().ToString() : contextName;
        var _dbContextOptions = new DbContextOptionsBuilder<ProviderRelationshipsDataContext>()
            .UseInMemoryDatabase(databaseName: contextName)
            .Options;

        return new ProviderRelationshipsDataContext(_dbContextOptions);
    }
}
