using System.Data;
using Dapper;
using PopupCash.Database.Models.Users;

namespace PopupCash.Database.Models.Mappers
{
    public class UserDataMapper
    {
        public UserDataMapper()
        {
        }

        public int CreateUserTable(IDbConnection connection)
        {
            return connection.Execute($@"
                CREATE TABLE IF NOT EXISTS USER_INFO (
                    ACCESS_TOKEN TEXT PRIMARY KEY NOT NULL,                    
                    MAC_ADDRESS TEXT NOT NULL,
                    UPDATE_DATE TEXT NOT NULL
                );

                CREATE INDEX IF NOT EXISTS IX_USER_INFO_ACCESS_TOKEN ON USER_INFO (ACCESS_TOKEN)");
        }

        public UserData? SelectUser(IDbConnection connection, string accessToken)
        {
            return connection.QuerySingleOrDefault<UserData>($@"
                SELECT * from USER_INFO
                WHERE ACCESS_TOKEN=@accessToken", new { accessToken });
        }

        public int IsExistUser(IDbConnection connection, string accessToken)
        {
            return connection.QuerySingleOrDefault<int>($@"
                SELECT EXISTS (SELECT 1 FROM USER_INFO WHERE ACCESS_TOKEN = @accessToken)", new { accessToken });
        }

        internal int InsertUser(IDbConnection connection, IDbTransaction transaction, UserData user)
        {
            return connection.Execute($@"
                INSERT INTO USER_INFO 
                    (ACCESS_TOKEN, MAC_ADDRESS, UPDATE_DATE)
                VALUES 
                    (@AccessToken, @MacAddress, DATETIME('NOW', 'LOCALTIME'))", user, transaction);
        }

        internal int UpdateUser(IDbConnection connection, IDbTransaction transaction, UserData user)
        {
            return connection.Execute($@"
                UPDATE USER_INFO 
                SET 
                    MAC_ADDRESS = @MacAddress,
                    UPDATE_DATE = DATETIME('NOW', 'LOCALTIME')
                WHERE 
                    ACCESS_TOKEN = @AccessToken", user, transaction);
        }

        internal int DeleteUser(IDbConnection connection, IDbTransaction transaction, string accessToken)
        {
            return connection.Execute($@"
                DELETE FROM USER_INFO
                    WHERE ACCESS_TOKEN = @accessToken", new { accessToken }, transaction);
        }
    }
}
