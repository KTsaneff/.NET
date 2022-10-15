using System;
using System.Data.SqlClient;
using System.Security;

namespace ADO.NET
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            string jobTitleEnt = Console.ReadLine();
            using SqlConnection sqlConnection = new SqlConnection(Config.ConnString);

            sqlConnection.Open();
            string employeeCountQuery = @"SELECT COUNT(*) 
                                              AS [EmployeeCount] 
                                            FROM [Employees]";

            //SqlTransaction transaction = conn.BeginTransaction();
            SqlCommand emplouyeeCountCommand = 
                new SqlCommand(employeeCountQuery, sqlConnection);

            int employeesCount = 
                (int)emplouyeeCountCommand.ExecuteScalar();

            Console.WriteLine($"Employees available: {employeesCount}");

            string employeeInfoQuery = @"SELECT [FirstName], 
                                                [LastName], 
                                                [JobTitle] 
                                           FROM [Employees]
                                          WHERE [JobTitle] = @jobTitle";

            SqlCommand employeeInfoCommand = 
                new SqlCommand(employeeInfoQuery, sqlConnection);
            employeeInfoCommand.Parameters.AddWithValue("@jobTitle", jobTitleEnt);
            using SqlDataReader employeeInfoReader = 
                employeeInfoCommand.ExecuteReader();

            int rowNum = 1;
            while (employeeInfoReader.Read())
            {
                string firstName = (string)employeeInfoReader["FirstName"];
                string lastName = (string)employeeInfoReader["LastName"];
                string jobTitle = (string)employeeInfoReader["JobTitle"];

                Console.WriteLine($"#{rowNum++}. {firstName} {lastName} - {jobTitle}");
            }

            employeeInfoReader.Close();
            sqlConnection.Close();

            //SecureString password = new SecureString();
            //char[] chars = { 'J', 'E', 'A', 'l', 'o', 'u', 's', 'y', '0', '1' };
            //foreach (var symbol in chars)
            //{
            //    password.AppendChar(symbol);
            //}
            //SqlCredential credential = new SqlCredential("sa", password);

        }
    }
}
