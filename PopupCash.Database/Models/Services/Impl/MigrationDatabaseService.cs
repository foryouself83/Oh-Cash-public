using PopupCash.Database.Models.Mappers;
using PopupCash.Database.Models.Migrations;

namespace PopupCash.Database.Models.Services.Impl
{
    public class MigrationDatabaseService : DbServiceBase, IMigrationDatabaseService
    {
        private readonly MigraionDatabaseMapper _mapper;

        public MigrationDatabaseService(IDatabaseFactory factory, MigraionDatabaseMapper mapper) : base(factory)
        {
            _mapper = mapper;
        }

        public bool CreateTable()
        {
            return Execute((con) =>
            {
                return _mapper.CreateTable(con) > 0;
            });
        }

        public float SelectLastVersion()
        {
            return Execute((con) =>
            {
                return _mapper.SelectLastVersion(con);
            });
        }

        public bool InsertDatabaseVersion(MigrationDatabase migration)
        {
            return ExecuteTrans((con, tran) =>
            {
                return _mapper.InsertDatabaseVersion(con, tran, migration) > 0;
            });
        }
        public bool ExecuteQuery(IEnumerable<string> queries)
        {
            var result = ExecuteTrans((con, tran) =>
            {
                foreach (var query in queries)
                {
                    _mapper.Execute(con, tran, query);
                }
                return true;
            });
            return true;
        }
    }
}
