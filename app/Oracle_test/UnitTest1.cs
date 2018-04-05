using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oracle_test
{
    using Dapper;
    using Oracle.ManagedDataAccess.Client;

    [TestClass]
    public class UnitTest1
    {
        // https://docs.oracle.com/cd/B28359_01/win.111/b28375/featConnecting.htm

        string connectionString = 
            @"User Id=system;Password=Oracle_01;Data Source=
(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)
     (HOST=192.168.2.10)(PORT=1521))(CONNECT_DATA=
     (SERVICE_NAME=orcl.sysoperations.co.th)))";

        [TestMethod]
        public void test_connection()
        {
            using (var db = new OracleConnection(connectionString))
            {
                db.Open();

            }
            
        }

        [TestMethod]
        public void query_employees()
        {
            var q_allemp = @"SELECT * FROM hr.employees";
            using (var db = new OracleConnection(connectionString))
            {
                var all_employees = db.Query<dynamic>(q_allemp);

                foreach (var emp in all_employees)
                {
                    Console.WriteLine(emp);
                }
            }
        }
    }
}
