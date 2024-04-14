using System.Data;

namespace PopupCash.Database.Models.Services.Impl
{
    public abstract class DbServiceBase
    {
        protected readonly IDatabaseFactory _dbFactory;
        /// <summary>
        /// DB File Path
        /// </summary>
        public readonly string _filePath;

        public DbServiceBase(IDatabaseFactory factory)
        {
            this._dbFactory = factory;
            _filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"PopupCash\PopupCash.DB");
        }

        protected TResult Execute<TResult>(Func<IDbConnection, TResult> func)
        {
            using var connection = _dbFactory.CreateConnection(_filePath);
            try
            {
                connection.Open();

                TResult result = func(connection);

                return result;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null) { connection.Close(); }
            }
        }


        protected TResult ExecuteTrans<TResult>(Func<IDbConnection, IDbTransaction, TResult> func)
        {
            using var connection = _dbFactory.CreateConnection(_filePath);
            connection.Open();

            using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                TResult result = func(connection, transaction);
                transaction.Commit();

                return result;
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
                if (connection != null) { connection.Close(); }

            }
        }

        protected Task<TResult> ExecuteAsync<TResult>(Func<IDbConnection, Task<TResult>> funcAsync)
        {
            using var connection = _dbFactory.CreateConnection(_filePath);
            try
            {
                connection.Open();
                var result = funcAsync(connection);

                return result;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null) { connection.Close(); }
            }
        }

        protected async Task<TResult> ExecuteTranAsync<TResult>(Func<IDbConnection, IDbTransaction, Task<TResult>> funcAsync)
        {
            using var connection = _dbFactory.CreateConnection(_filePath);
            using var transaction = connection.BeginTransaction();
            try
            {
                var result = funcAsync(connection, transaction);
                transaction.Commit();

                return await result;
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
                if (connection != null) { connection.Close(); }
            }
        }

        protected Task<TResult> ExecuteTrans<TResult>(Func<IDbConnection, IDbTransaction, Task<TResult>> func)
        {
            using var connection = _dbFactory.CreateConnection(_filePath);
            connection.Open();

            using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                var result = func(connection, transaction);
                transaction.Commit();

                return result;
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
                if (connection != null) { connection.Close(); }

            }
        }
    }
}
