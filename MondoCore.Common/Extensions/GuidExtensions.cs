using System;
using System.Collections.Generic;
using System.Text;

namespace MondoCore.Common
{
    public static class GuidExtensions
    {
        public static string ToId(this Guid guid)
        {
            return guid.ToString().ToLower();
        }
    }
}
