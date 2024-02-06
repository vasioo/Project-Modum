using Microsoft.EntityFrameworkCore;
using Modum.DataAccess;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.ProductStructure;
using NUnit.Framework;

namespace Modum.Tests.Mocks
{
    public static class DatabaseMock
    {
        public static DataContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase("ModumDatabase" + DateTime.Now.Ticks.ToString())
                    .Options;

                var context = new DataContext(dbContextOptions, seedDb: false);
                return context;
            }
        }
    }
}
