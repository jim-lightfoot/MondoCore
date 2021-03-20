using System;
using System.Collections.Generic;
using System.Text;

namespace MondoCore.Data
{
    public class DelimitedIdentifierStrategy<TID> : IIdentifierStrategy<TID>
    {
        private readonly string _separator;

        public DelimitedIdentifierStrategy(string separator = ";")
        {
            _separator = separator;
        }

        public (string Id, string PartitionKey) GetId(TID id)
        {
            var parts = id.ToString().Split(_separator);

            return (parts[0], parts[1]);
        }

        public string ToPartitionedId(string id, string partitionKey)
        {
            return id + _separator + partitionKey;
        }
    }
}
