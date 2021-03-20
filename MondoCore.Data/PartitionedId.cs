using System;
using System.Collections.Generic;
using System.Text;

namespace MondoCore.Data
{
    public class PartitionedId : IPartitionedId
    {
        public string Id           { get; set; }
        public string PartitionKey { get; set; }
    }
}
