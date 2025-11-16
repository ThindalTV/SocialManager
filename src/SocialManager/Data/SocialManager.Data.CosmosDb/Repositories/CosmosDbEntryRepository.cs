using Microsoft.EntityFrameworkCore;
using SocialManager.Data.Repositories;
using SocialManager.Data.Types;

namespace SocialManager.Data.CosmosDb.Repositories;

public class CosmosDbEntryRepository : CosmosRepository<Entry>, IEntryRepository
{
    public CosmosDbEntryRepository(SocialManagerDbContext context) : base(context) { }
}
