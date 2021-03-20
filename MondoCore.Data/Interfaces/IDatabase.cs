using System;
using System.Collections.Generic;
using System.Text;

namespace MondoCore.Data
{
    /// <summary>
    /// Interface for a database
    /// </summary>
    public interface IDatabase
    {
        IReadRepository<TID, TValue> GetRepositoryReader<TID, TValue>(string repoName, IIdentifierStrategy<TID> strategy = null) where TValue : IIdentifiable<TID>;
        IWriteRepository<TID, TValue> GetRepositoryWriter<TID, TValue>(string repoName, IIdentifierStrategy<TID> strategy = null) where TValue : IIdentifiable<TID>;
    }
}
