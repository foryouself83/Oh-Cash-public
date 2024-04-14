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
                    EMAIL TEXT PRIMARY KEY NOT NULL,                    
                    NAME TEXT NOT NULL,
                    PROFILE_IMAGE_URL TEXT,
                    CASH TEXT NOT NULL,
                    MISSON_POINT TEXT NOT NULL,
                    GRADE TEXT NOT NULL,
                    UPDATE_DATE TEXT NOT NULL
                );

                CREATE INDEX IF NOT EXISTS IX_USER_INFO_EMAIL ON USER_INFO (EMAIL)");
        }

        public User? SelectUser(IDbConnection connection, string email)
        {
            return connection.QuerySingleOrDefault<User>($@"
                SELECT * from USER_INFO
                WHERE EMAIL=@email", new { email });
        }

        public int IsExistUser(IDbConnection connection, string email)
        {
            return connection.QuerySingleOrDefault<int>($@"
                SELECT EXISTS (SELECT 1 FROM USER_INFO WHERE EMAIL = @email)", new { email });
        }

        internal int InsertUser(IDbConnection connection, IDbTransaction transaction, User user)
        {
            return connection.Execute($@"
                INSERT INTO USER_INFO 
                    (EMAIL, NAME, PROFILE_IMAGE_URL, CASH, MISSON_POINT, GRADE, UPDATE_DATE)
                VALUES 
                    (@Email, @Name, @ProfileImageUrl, @Cash, @MissionPoint, @Grade, DATETIME('NOW', 'LOCALTIME'))", user, transaction);
        }

        internal int UpdateUser(IDbConnection connection, IDbTransaction transaction, User user)
        {
            return connection.Execute($@"
                UPDATE USER_INFO 
                SET 
                    NAME = @Name,
                    PROFILE_IMAGE_URL = @ProfileImageUrl,
                    CASH = @Cash,
                    MISSON_POINT = @MissionPoint
                    GRADE = @Grade,
                    UPDATE_DATE = DATETIME('NOW', 'LOCALTIME')
                WHERE 
                    EMAIL = @Email", user, transaction);
        }

        internal int DeleteUser(IDbConnection connection, IDbTransaction transaction, string email)
        {
            return connection.Execute($@"
                DELETE FROM USER_INFO
                    WHERE EMAIL = @email", new { email }, transaction);
        }
    }
}
