using System;
using System.Collections.Generic;
using System.Text;

namespace MondoCore.Data
{
    public static class StringExtensions
    {
        public static IPartitionedId ToPartitionedId(this string id, string partitionKey = null)
        {
            return new PartitionedId {  Id = id, PartitionKey = partitionKey };
        }
    }
}
