using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using MondoCore.Common;
using MondoCore.Data;
using MondoCore.Repository.TestHelper;

namespace MondoCore.Data.UnitTests
{
    [TestClass]
    [TestCategory("Unit Tests")]
    public class MemoryRepository_guidTests : RepositoryTestBase<Guid>
    {
           public MemoryRepository_guidTests() :

           base(new MemoryRepositoryTests.MemoryDatabase(),
                "cars",
                ()=> Guid.NewGuid())
        {
        }
    }
}
