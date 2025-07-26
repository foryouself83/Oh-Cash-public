using System.Data;
using System.Diagnostics;
using Dapper.FluentMap;
using PopupCash.Database.Conventions;
using PopupCash.Database.Models.Locations;
using PopupCash.Database.Models.Migrations;
using PopupCash.Database.Models.Users;

namespace PopupCash.Database.Models.Services.Impl
{
    public class DatabaseFactory : IDatabaseFactory
    {
        /// <summary>
        /// DB Password
        /// </summary>
        public string Password { get; set; }

        public DatabaseFactory()
        {
            Password = string.Empty;
            //Password = "PopupCash2024@@";

            FluentMapper.Initialize(config =>
            {
                config.AddConvention<CamelToUnderscoreConvention>()
                .ForEntitiesInAssembly(typeof(MigrationDatabase).Assembly, typeof(MigrationDatabase).Namespace)
                .ForEntitiesInAssembly(typeof(Authorization).Assembly, typeof(Authorization).Namespace)
                .ForEntitiesInAssembly(typeof(WindowPosition).Assembly, typeof(WindowPosition).Namespace);
            });
        }

        public IDbConnection CreateConnection(string path)
        {
            // 파일 여부 확인
            if (!File.Exists(path))
            {
                // 디렉토리 경로 얻기
                var directoryPath = Path.GetDirectoryName(path);

                // 디렉토리가 없는 경우 생성
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }

            Debug.Assert(!string.IsNullOrEmpty(Password));

            var connectionString = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(string.Format(@$"Data Source={path};"))
            {
                Mode = Microsoft.Data.Sqlite.SqliteOpenMode.ReadWriteCreate,
                Password = Password,
            }.ToString();
            return new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
        }
    }
}
