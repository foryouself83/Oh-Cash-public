using System.Data;

using Dapper;
using PopupCash.Database.Models.Migrations;

namespace PopupCash.Database.Models.Mappers
{
    public class MigraionDatabaseMapper
    {
        public MigraionDatabaseMapper()
        {
        }
        public int CreateTable(IDbConnection connection)
        {
            return connection.Execute($@"
                CREATE TABLE IF NOT EXISTS MIGRATION_DATABASE (
                    VERSION REAL PRIMARY KEY NOT NULL,
                    UPDATE_DATE TEXT NOT NULL
                );

                CREATE INDEX IF NOT EXISTS IX_MIGRATION_DATABASE_MIGRATION_VERSION ON MIGRATION_DATABASE (VERSION)");
        }
        public float SelectLastVersion(IDbConnection connection)
        {
            return connection.QuerySingleOrDefault<float>($@"
                SELECT *  from MIGRATION_DATABASE
                ORDER BY UPDATE_DATE DESC
                LIMIT 1");
        }
        public int InsertDatabaseVersion(IDbConnection connection, IDbTransaction transaction, MigrationDatabase migration)
        {
            return connection.Execute($@"
                INSERT INTO MIGRATION_DATABASE 
                    (VERSION, UPDATE_DATE)
                VALUES 
                    (@Version, DATETIME('NOW', 'LOCALTIME'))", migration, transaction);
        }
        public int Execute(IDbConnection connection, IDbTransaction transaction, string query)
        {
            return connection.Execute(query, transaction: transaction);
        }
    }
}
