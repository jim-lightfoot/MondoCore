using System;
using System.Collections.Generic;
using System.Text;

namespace MondoCore.Data
{
    public interface IIdentifierStrategy<TID>
    {
       (string Id, string PartitionKey) GetId(TID id);
    }
}
