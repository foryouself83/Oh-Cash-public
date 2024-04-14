using System.Data;

using Dapper;

using PopupCash.Database.Models.Users;

namespace PopupCash.Database.Models.Mappers
{
    public class AuthorizationMapper
    {
        public AuthorizationMapper()
        {
        }

        public int CreateAuthorizationTable(IDbConnection connection)
        {
            return connection.Execute($@"
                CREATE TABLE IF NOT EXISTS AUTHORIZATION (
                    KEY TEXT PRIMARY KEY NOT NULL,
                    TYPE TEXT NOT NULL,                    
                    ACCESS_TOKEN TEXT NOT NULL,
                    REFRESH_TOKEN TEXT,
                    UPDATE_DATE TEXT NOT NULL
                );

                CREATE INDEX IF NOT EXISTS IX_AUTHORIZATION_KEY ON AUTHORIZATION (KEY)");
        }
        public Authorization? SelectLastestAuthorization(IDbConnection connection)
        {
            return connection.QuerySingleOrDefault<Authorization>($@"
                SELECT *  from AUTHORIZATION
                ORDER BY UPDATE_DATE DESC
                LIMIT 1");
        }

        public string? SelectAccessToken(IDbConnection connection, string key)
        {
            return connection.QuerySingleOrDefault<string>($@"
                SELECT ACCESS_TOKEN from AUTHORIZATION
                WHERE KEY=@key", new { key });
        }

        public IEnumerable<string> SelectOrderByAccesToken(IDbConnection connection, int limit)
        {
            return connection.Query<string>(@$"
 	            SELECT 
	                ACCESS_TOKEN
                FROM 
	                AUTHORIZATION
                ORDER BY UPDATE_DATE DESC
                LIMIT @limit", new { limit });
        }

        public int InsertAuthorization(IDbConnection connection, IDbTransaction transaction, Authorization authorization)
        {
            return connection.Execute($@"
                INSERT INTO AUTHORIZATION 
                    (KEY, TYPE, ACCESS_TOKEN, REFRESH_TOKEN, UPDATE_DATE)
                VALUES 
                    (@Key, @Type, @AccessToken, @RefreshToken, DATETIME('NOW', 'LOCALTIME'))", authorization, transaction);
        }


        public int UpdateAuthorization(IDbConnection connection, IDbTransaction transaction, Authorization authorization)
        {
            return connection.Execute($@"
                UPDATE AUTHORIZATION
                    SET TYPE = @Type,
                        ACCESS_TOKEN = @AccessToken,
                        REFRESH_TOKEN = @RefreshToken,
                        UPDATE_DATE = DATETIME('NOW', 'LOCALTIME')
                    WHERE KEY = @Key", authorization, transaction);
        }

        public int DeleteAuthorization(IDbConnection connection, IDbTransaction transaction, string type)
        {
            return connection.Execute($@"
                DELETE FROM AUTHORIZATION
                    WHERE KEY = @Key", new { type }, transaction);
        }

    }
}
