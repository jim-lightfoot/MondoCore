using System;
using System.Collections.Generic;
using System.Text;

namespace MondoCore.Data
{
    public static class IDatabaseExtensions
    {
        public static IReadRepository<TID, TValue> GetRepositoryReader<TID, TValue>(this IDatabase db, string repoName, string partitionKey) where TValue : IIdentifiable<TID>
        {
            return db.GetRepositoryReader<TID, TValue>(repoName, new FixedIdentifierStrategy<TID>(partitionKey));
        }

        public static IWriteRepository<TID, TValue> GetRepositoryWriter<TID, TValue>(this IDatabase db, string repoName, string partitionKey) where TValue : IIdentifiable<TID>
        {
            return db.GetRepositoryWriter<TID, TValue>(repoName, new FixedIdentifierStrategy<TID>(partitionKey));
        }
    }
}
