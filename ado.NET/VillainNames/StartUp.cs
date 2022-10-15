using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace VillainNames
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            //string[] minionInfo = Console.ReadLine()
                                         //.Split(": ", StringSplitOptions.RemoveEmptyEntries)[1]
                                         //.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                         //.ToArray();
            //string villainName = Console.ReadLine().Split(": ", StringSplitOptions.RemoveEmptyEntries)[1];
            int minionId = int.Parse(Console.ReadLine());

            using SqlConnection sqlConnection =
                new SqlConnection(Config.ConnectionString);
            sqlConnection.Open();

            //string result = GetVillainNamesWithMinionsCount(sqlConnection);
            //string result = GetVillainWithMinions(sqlConnection, villainId);
            //string result = AddNewMinion(sqlConnection, minionInfo, villainName);
            //string result = DeleteVillain(sqlConnection, villainId);
            string result = IncreaseMinionAge(sqlConnection, minionId);

            Console.WriteLine(result);

            sqlConnection.Close();
        }

        private static string IncreaseMinionAge(SqlConnection sqlConnection, int minionId)
        {
            StringBuilder output = new StringBuilder();

            string inceaseAgeQuery = @"EXECUTE [dbo].[usp_GetOlder] @MinionId";
            SqlCommand increaseAgeCommand = new SqlCommand(inceaseAgeQuery, sqlConnection);
            increaseAgeCommand.Parameters.AddWithValue("@MinionId", minionId);
            increaseAgeCommand.ExecuteNonQuery();

            string minionInfoQuery = @"SELECT [Name], [Age]
                                         FROM [Minions]
                                        WHERE [Id] = @MinionId";
            SqlCommand minionInfoCommand = new SqlCommand(minionInfoQuery, sqlConnection);
            minionInfoCommand.Parameters.AddWithValue("@MinionId", minionId);

            using SqlDataReader infoReader = minionInfoCommand.ExecuteReader();
            while (infoReader.Read())
            {
                output.AppendLine($"{infoReader["Name"]} - {infoReader["Age"]} years old");
            }

            return output.ToString().TrimEnd();
        }
        private static string DeleteVillain(SqlConnection sqlConnection, int villainId)
        {
            StringBuilder output = new StringBuilder();

            string villainNameQuery = $@"SELECT [Name] 
                                           FROM [Villains] 
                                          WHERE [Id] = @VillainId";

            SqlCommand villainNameCommand = 
                new SqlCommand(villainNameQuery, sqlConnection);

            villainNameCommand.Parameters.AddWithValue("@VillainId", villainId);

            string villainName = (string)villainNameCommand.ExecuteScalar();
            if (villainName == null)
            {
                return "No such villain was found.";
            }

            SqlTransaction sqlTransaction = 
                sqlConnection.BeginTransaction();

            try
            {
                string releaseMinionsQuery =
                @"DELETE FROM [MinionsVillains] WHERE [VillainId] = @VillainId";

                SqlCommand releaseMinionsCommand =
                    new SqlCommand(releaseMinionsQuery, sqlConnection, sqlTransaction);
                releaseMinionsCommand.Parameters.AddWithValue("@VillainId", villainId);
                int minionsReleased = releaseMinionsCommand.ExecuteNonQuery();

                string deleteVillainQuery =
                    @"DELETE FROM [Villains] WHERE [Id] = @VillainId";

                SqlCommand deleteVillainCommand =
                    new SqlCommand(deleteVillainQuery, sqlConnection, sqlTransaction);
                deleteVillainCommand.Parameters.AddWithValue("@VillainId", villainId);

                int villainsDeleted = deleteVillainCommand.ExecuteNonQuery();

                if (villainsDeleted != 1)
                {
                    sqlTransaction.Rollback();
                }
                output.AppendLine($"{villainName} was deleted.");
                output.AppendLine($"{minionsReleased} minions were released.");
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                return e.ToString();
            }

            sqlTransaction.Commit();
            return output.ToString().TrimEnd();
        }
        private static string AddNewMinion(SqlConnection sqlConnection, string[] minionInfo, string villainName)
        {
            StringBuilder output = new StringBuilder();

            string minionName = minionInfo[0];
            int minionAge = int.Parse(minionInfo[1]);
            string townName = minionInfo[2];

            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

            try
            {
                int townId = GetTownId(townName, output, sqlConnection, sqlTransaction);
                int villainId = GetVillainId(sqlConnection, sqlTransaction, output, villainName);
                int minionId = AddMinion(sqlConnection, sqlTransaction, minionName, minionAge, townId);

                string addMinionToVillainQuery = $@"INSERT INTO [MinionsVillains]([MinionId], [VillainId])
                                                          VALUES(@minionId, @villainId)";
                SqlCommand addMinionToVillainCommand =
                    new SqlCommand(addMinionToVillainQuery, sqlConnection, sqlTransaction);
                addMinionToVillainCommand.Parameters.AddWithValue("@minionId", minionId);
                addMinionToVillainCommand.Parameters.AddWithValue("@villainId", villainId);

                addMinionToVillainCommand.ExecuteNonQuery();
                output.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");

                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                return e.ToString();
            }
            return output.ToString().TrimEnd();
        }
        private static string GetVillainNamesWithMinionsCount(SqlConnection sqlConnection)
        {
            StringBuilder output = new StringBuilder();
            string query = @"SELECT [m].[Name], [m].[Age]
                               FROM [Villains]
                                 AS [v]
                          LEFT JOIN [MinionsVillains]
                                 AS [mv]
                                 ON [mv].[VillainId] = [v].[Id]
                          LEFT JOIN [Minions]
                                 AS [m]
                                 ON [m].[Id] = [mv].[MinionId]
                              WHERE [mv].[VillainId] = @VillainId
                           ORDER BY [m].[Name]";

            SqlCommand command = new SqlCommand(query, sqlConnection);

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                output.Append($"{reader["Name"]} - {reader["MinionsCount"]}");
            }

            return output.ToString().TrimEnd();
        }

        private static string GetVillainWithMinions(SqlConnection sqlConnection, int villainId)
        {
            StringBuilder output = new StringBuilder();

            string villainNameQuery = @$"SELECT [Name]
                                           FROM [Villains]
                                          WHERE [Id] = @VillainId";

            SqlCommand getVillainNameCommand = new SqlCommand(villainNameQuery, sqlConnection);
            getVillainNameCommand.Parameters.AddWithValue("@VillainId", villainId);

            string villainName = (string)getVillainNameCommand.ExecuteScalar();
            if (villainName == null)
            {
                return $"No villain with ID {villainId} exists in the database.";
            }

            output.AppendLine($"Villain: {villainName}");

            string selectMinionsQuery = $@"SELECT [m].[Name], [m].[Age]
                                             FROM [MinionsVillains]
                                               AS [mv]
                                        LEFT JOIN [Minions]
                                               AS [m]
                                               ON [m].[Id] = [mv].[MinionId]
                                            WHERE [mv].[VillainId] = @VillainId
                                         ORDER BY [m].[Name]";

            SqlCommand orderedMinionsCommand = new SqlCommand(selectMinionsQuery, sqlConnection);
            orderedMinionsCommand.Parameters.AddWithValue("@VillainId", villainId);

            using SqlDataReader minionsReader = orderedMinionsCommand.ExecuteReader();

            if (!minionsReader.HasRows)
            {
                output.AppendLine($"no minions");
            }
            else
            {
                int rowNumber = 1;
                while (minionsReader.Read())
                {
                    output.AppendLine($"{rowNumber++}. {minionsReader["Name"]} {minionsReader["Age"]}");
                }
            }
            return output.ToString().TrimEnd();
        }
        private static int GetTownId(string townName, StringBuilder output, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            string townIdQuery = $@"SELECT [Id]
                                          FROM [Towns]
                                         WHERE [Name] = @townName";
            SqlCommand townIdCommand = new SqlCommand(townIdQuery, sqlConnection, sqlTransaction);
            townIdCommand.Parameters.AddWithValue("@townName", townName);

            object townIdObj = townIdCommand.ExecuteScalar();

            if (townIdObj == null)
            {
                string addTownQuery = @"INSERT INTO [Towns]([Name])
                                             VALUES (@townName)";
                SqlCommand addTownCommand = new SqlCommand(addTownQuery, sqlConnection, sqlTransaction);
                addTownCommand.Parameters.AddWithValue("@townName", townName);
                addTownCommand.ExecuteNonQuery();
                output.AppendLine($"Town {townName} was added to the database.");
                townIdObj = townIdCommand.ExecuteScalar();
            }

            return (int)townIdObj;
        }
        private static int GetVillainId(SqlConnection sqlConnection, SqlTransaction sqlTransaction, StringBuilder output, string villainName)
        {
            string villainIdQuery = $@"SELECT [Id] FROM Villains
                                           WHERE [Name] = @villainName";
            SqlCommand villainIdCommand = new SqlCommand(villainIdQuery, sqlConnection, sqlTransaction);
            villainIdCommand.Parameters.AddWithValue("@villainName", villainName);
            object villainIdObj = villainIdCommand.ExecuteScalar();

            if (villainIdObj == null)
            {
                string evilnessFactorQuery = $@"SELECT [Id]
                                                      FROM [EvilnessFactors]
                                                     WHERE [Name] = 'Evil'";
                SqlCommand evilnessFactorCommand =
                    new SqlCommand(evilnessFactorQuery, sqlConnection, sqlTransaction);
                int evilnessFactorId = (int)evilnessFactorCommand.ExecuteScalar();

                string insertVillainQuery = $@"INSERT INTO [Villains] ([Name], [EvilnessFactorId])
                                                         VALUES(@villainName, @evilnessFactorId)";
                SqlCommand insertVillainCommand =
                    new SqlCommand(insertVillainQuery, sqlConnection, sqlTransaction);

                insertVillainCommand.Parameters.AddWithValue("@villainName", villainName);
                insertVillainCommand.Parameters.AddWithValue("@evilnessFactorId", evilnessFactorId);

                insertVillainCommand.ExecuteNonQuery();
                output.AppendLine($"Villain {villainName} was added to the database.");
                villainIdObj = villainIdCommand.ExecuteScalar();
            }

            return (int)villainIdObj;
        }

        private static int AddMinion(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string minionName, int minionAge, int townId)
        {
            string addMinionQuery = $@"INSERT INTO [Minions]([Name], [Age], [TownId])
                                                 VALUES(@minionName, @minionAge, @townId)";
            SqlCommand addMinionCommand =
                new SqlCommand(addMinionQuery, sqlConnection, sqlTransaction);
            addMinionCommand.Parameters.AddWithValue("@minionName", minionName);
            addMinionCommand.Parameters.AddWithValue("@minionAge", minionAge);
            addMinionCommand.Parameters.AddWithValue("@townId", townId);
            addMinionCommand.ExecuteNonQuery();

            string addedMinionIdQuery = $@"SELECT [Id] FROM [Minions]
                                       WHERE [Name] = @minionName AND 
                                             [Age] = @minionAge AND 
                                             [TownId] = @TownId";
            SqlCommand getMinionIdCommand =
                new SqlCommand(addedMinionIdQuery, sqlConnection, sqlTransaction);
            getMinionIdCommand.Parameters.AddWithValue("minionName", minionName);
            getMinionIdCommand.Parameters.AddWithValue("minionAge", minionAge);
            getMinionIdCommand.Parameters.AddWithValue("TownId", townId);

            int minionId = (int)getMinionIdCommand.ExecuteScalar();

            return minionId;
        }
    }
}
