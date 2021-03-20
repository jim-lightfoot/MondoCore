using System;
using System.Collections.Generic;
using System.Text;

namespace MondoCore.Data
{
    public class FixedIdentifierStrategy<TID> : IIdentifierStrategy<TID>
    {
        private readonly string _partitionKey;

        public FixedIdentifierStrategy(string partitionKey)
        {
            _partitionKey = partitionKey;
        }

        public (string Id, string PartitionKey) GetId(TID id)
        {
            return (id.ToString(), _partitionKey);
        }
    }
}
