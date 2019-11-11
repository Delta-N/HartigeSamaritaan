using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoosterPlanner.Data.Context;

namespace RoosterPlanner.Data
{
    [TestClass]
    public abstract class DatabaseContext
    {
        #region Fields
        protected static ConnectionStringsConfig connectionStringsConfig = null;
        #endregion

        public static void Init(TestContext context)
        {
            DirectoryInfo dir = new DirectoryInfo(context.DeploymentDirectory);
            while (!dir.Name.EndsWith(".Test") && dir.Parent != null)
            {
                dir = dir.Parent;
            }
            connectionStringsConfig = TestHelper.GetConnectionStringConfiguration(dir.FullName);
        }

        public RoosterPlannerContext GetRoosterPlannerContext(ConnectionStringsConfig connectionStringsConfig)
        {
            DbContextOptionsBuilder<RoosterPlannerContext> dbContextOptions = new DbContextOptionsBuilder<RoosterPlannerContext>()
                .UseSqlServer(connectionStringsConfig.RoosterPlannerDatabase)
                .EnableDetailedErrors(true);
            return new RoosterPlannerContext(dbContextOptions.Options);
        }
    }
}
