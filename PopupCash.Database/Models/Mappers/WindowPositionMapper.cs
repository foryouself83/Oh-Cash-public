using System.Data;
using Dapper;
using PopupCash.Database.Models.Locations;

namespace PopupCash.Database.Models.Mappers
{
    public class WindowPositionMapper
    {
        public int CreateTable(IDbConnection connection)
        {
            return connection.Execute($@"
                CREATE TABLE IF NOT EXISTS WINDOW_POSITION (
                    ID TEXT PRIMARY KEY NOT NULL,
                    LEFT TEXT NOT NULL,
                    TOP TEXT NOT NULL,
                    UPDATE_DATE TEXT NOT NULL
                );

                CREATE INDEX IF NOT EXISTS IX_WINDOW_POSITION_ID ON WINDOW_POSITION (ID)");
        }

        public WindowPosition? SelectWindowPostion(IDbConnection connection, string windowId)
        {
            return connection.QuerySingleOrDefault<WindowPosition>($@"
                SELECT LEFT, TOP from WINDOW_POSITION
                WHERE ID=@windowId", new { windowId });
        }

        public int InsertWindowPostion(IDbConnection connection, IDbTransaction transaction, WindowPosition position)
        {
            return connection.Execute($@"
                INSERT INTO WINDOW_POSITION 
                    (ID, LEFT, TOP, UPDATE_DATE)
                VALUES 
                    (@Id, @Left, @Top, DATETIME('NOW', 'LOCALTIME'))", position, transaction);
        }

        public int UpdateWindowPostion(IDbConnection connection, IDbTransaction transaction, WindowPosition position)
        {
            return connection.Execute($@"
                UPDATE WINDOW_POSITION
                    SET LEFT = @Left,
                        TOP = @Top,
                        UPDATE_DATE = DATETIME('NOW', 'LOCALTIME')
                    WHERE ID = @Id", position, transaction);
        }

        public int DeleteWindowPostion(IDbConnection connection, IDbTransaction transaction, string windowId)
        {
            return connection.Execute($@"
                DELETE FROM WINDOW_POSITION
                    WHERE ID = @windowId", new { windowId }, transaction);
        }
    }
}