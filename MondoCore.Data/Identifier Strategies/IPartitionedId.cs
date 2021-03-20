using System;
using System.Collections.Generic;
using System.Text;

namespace MondoCore.Data
{
    public interface IPartitionedId
    {
        string Id           { get; }
        string PartitionKey { get; }
    }
}
