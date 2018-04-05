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

        string connection2String =
            @"User Id=system;Password=Oracle_01;Data Source=
(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)
     (HOST=192.168.2.11)(PORT=1521))(CONNECT_DATA=
     (SERVICE_NAME=orcl_stb.sysstb)))";

        [TestMethod]
        public void test_connection()
        {
            using (var db = new OracleConnection(connectionString))
            {
                db.Open();

            }
            
        }

        [TestMethod]
        public void prm_query_employees()
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

        [TestMethod]
        public void check_department()
        {
            var q_allemp = @"SELECT COUNT(*) FROM hr.departments";
            using (var orcl_db = new OracleConnection(connectionString))
            using (var orcl_stb_db = new OracleConnection(connection2String))
            {
                var count = orcl_db.ExecuteScalar<long>(q_allemp);
                var count2 = orcl_stb_db.ExecuteScalar<long>(q_allemp);

                Assert.AreEqual(count, count2);
            }
        }

        [TestMethod]
        public void insert_department()
        {
            string insert = @"
INSERT INTO hr.departments (DEPARTMENT_ID, DEPARTMENT_NAME, MANAGER_ID, LOCATION_ID) 
VALUES (:DEPARTMENT_ID, :DEPARTMENT_NAME, :MANAGER_ID, :LOCATION_ID)";
            using (var db = new OracleConnection(connectionString))
            {
                db.Execute(insert, new
                {
                    DEPARTMENT_ID = 999,
                    DEPARTMENT_NAME = "Test",
                    MANAGER_ID = 121,
                    LOCATION_ID = 1400
                }
                );
            }
        }

        [TestMethod]
        public void delete_department()
        {
            using (var db = new OracleConnection(connectionString))
            {
                db.Execute(@"DELETE hr.departments WHERE DEPARTMENT_ID = :DEPARTMENT_ID", 
                    new
                {
                    DEPARTMENT_ID = 999
                }
                );
            }
        }

        [TestMethod]
        public void query_standby_employees()
        {
            var q_allemp = @"SELECT * FROM hr.employees";
            using (var db = new OracleConnection(connection2String))
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
