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

        public bool IsExistUser(string accessToekn)
        {
            return Execute((con) =>
            {
                return _mapper.IsExistUser(con, accessToekn) == 1;
            });
        }

        public UserData? SelectUser(string accessToekn)
        {
            return Execute((con) =>
            {
                return _mapper.SelectUser(con, accessToekn);
            });
        }

        public bool InsertOrUpdateAuthorization(UserData user)
        {
            return ExecuteTrans((con, transaction) =>
            {
                if (_mapper.SelectUser(con, user.AccessToken) is null)
                {
                    return _mapper.InsertUser(con, transaction, user) > 0;
                }
                else
                {
                    return _mapper.UpdateUser(con, transaction, user) > 0;
                }
            });
        }
        public bool InsertUser(UserData user)
        {
            return ExecuteTrans((con, transaction) =>
            {
                return _mapper.InsertUser(con, transaction, user) > 0;
            });
        }

        public bool UpdateUser(UserData user)
        {
            return ExecuteTrans((con, transaction) =>
            {
                return _mapper.UpdateUser(con, transaction, user) > 0;
            });
        }

        public bool DeleteUser(string accessToekn)
        {
            return ExecuteTrans((con, transaction) =>
            {
                return _mapper.DeleteUser(con, transaction, accessToekn) > 0;
            });
        }
    }
}
