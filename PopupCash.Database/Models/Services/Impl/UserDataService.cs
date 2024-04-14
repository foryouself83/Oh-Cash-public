using PopupCash.Database.Models.Mappers;
using PopupCash.Database.Models.Users;

namespace PopupCash.Database.Models.Services.Impl
{
    public class UserDataService : DbServiceBase, IUserDataService
    {
        private readonly UserDataMapper _mapper;
        public UserDataService(IDatabaseFactory factory, UserDataMapper mapper) : base(factory)
        {
            _mapper = mapper;
        }

        public bool CreateTable()
        {
            return Execute((con) =>
            {
                return _mapper.CreateUserTable(con) > 0;
            });
        }

        public bool IsExistUser(string email)
        {
            return Execute((con) =>
            {
                _mapper.CreateUserTable(con);
                return _mapper.IsExistUser(con, email) == 1;
            });
        }

        public User? SelectUser(string email)
        {
            return Execute((con) =>
            {
                _mapper.CreateUserTable(con);
                return _mapper.SelectUser(con, email);
            });
        }
        public bool InsertUser(User user)
        {
            return ExecuteTrans((con, transaction) =>
            {
                _mapper.CreateUserTable(con);
                return _mapper.InsertUser(con, transaction, user) > 0;
            });
        }

        public bool UpdateUser(User user)
        {
            return ExecuteTrans((con, transaction) =>
            {
                _mapper.CreateUserTable(con);
                return _mapper.UpdateUser(con, transaction, user) > 0;
            });
        }

        public bool DeleteUser(string email)
        {
            return ExecuteTrans((con, transaction) =>
            {
                _mapper.CreateUserTable(con);
                return _mapper.DeleteUser(con, transaction, email) > 0;
            });
        }
    }
}
