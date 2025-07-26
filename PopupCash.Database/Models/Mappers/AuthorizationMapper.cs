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
                    TYPE TEXT PRIMARY KEY NOT NULL,                    
                    KEY TEXT,                    
                    POMISSION_KEY TEXT,
                    ACCESS_TOKEN TEXT NOT NULL,                    
                    REFRESH_TOKEN TEXT,
                    POLICY INTEGER NOT NULL,
                    UPDATE_DATE TEXT NOT NULL
                );

                CREATE INDEX IF NOT EXISTS IX_AUTHORIZATION_TYPE ON AUTHORIZATION (TYPE)");
        }
        public Authorization? SelectLastestAuthorization(IDbConnection connection)
        {
            return connection.QuerySingleOrDefault<Authorization>($@"
                SELECT *  from AUTHORIZATION
                ORDER BY UPDATE_DATE DESC
                LIMIT 1");
        }

        public string? SelectAccessToken(IDbConnection connection, string type)
        {
            return connection.QuerySingleOrDefault<string>($@"
                SELECT ACCESS_TOKEN from AUTHORIZATION
                WHERE TYPE=@type", new { type });
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
                    (TYPE, KEY, POMISSION_KEY, ACCESS_TOKEN, REFRESH_TOKEN, POLICY, UPDATE_DATE)
                VALUES 
                    (@Type, @Key, @PomissionKey, @AccessToken, @RefreshToken, @Policy, DATETIME('NOW', 'LOCALTIME'))", authorization, transaction);
        }


        public int UpdateAuthorization(IDbConnection connection, IDbTransaction transaction, Authorization authorization)
        {
            return connection.Execute($@"
                UPDATE AUTHORIZATION
                    SET KEY = @Key,
                        POMISSION_KEY = @PomissionKey,
                        ACCESS_TOKEN = @AccessToken,
                        REFRESH_TOKEN = @RefreshToken,
                        POLICY = @Policy,
                        UPDATE_DATE = DATETIME('NOW', 'LOCALTIME')
                    WHERE TYPE = @Type", authorization, transaction);
        }

        public int DeleteAuthorization(IDbConnection connection, IDbTransaction transaction, string key)
        {
            return connection.Execute($@"
                DELETE FROM AUTHORIZATION
                    WHERE KEY = @key 
                    OR POMISSION_KEY = @key", new { key }, transaction);
        }

    }
}
