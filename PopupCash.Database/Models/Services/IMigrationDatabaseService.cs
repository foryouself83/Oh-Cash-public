using PopupCash.Database.Models.Migrations;

namespace PopupCash.Database.Models.Services
{
    public interface IMigrationDatabaseService
    {
        public bool CreateTable();
        public float SelectLastVersion();
        public bool InsertDatabaseVersion(MigrationDatabase migration);
        public bool ExecuteQuery(IEnumerable<string> queries);
    }

}
