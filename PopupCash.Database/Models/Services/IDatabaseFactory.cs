using System.Data;

namespace PopupCash.Database.Models.Services
{
    public interface IDatabaseFactory
    {
        public string Password { get; set; }

        //public Task<IDbConnection> CreateConnection(string path);
        public IDbConnection CreateConnection(string path);
    }
}
