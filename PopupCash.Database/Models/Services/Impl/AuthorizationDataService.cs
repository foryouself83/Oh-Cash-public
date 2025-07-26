using PopupCash.Database.Models.Mappers;
using PopupCash.Database.Models.Users;

namespace PopupCash.Database.Models.Services.Impl
{
    public class AuthorizationDataService : DbServiceBase, IAuthorizationDataService
    {
        private readonly AuthorizationMapper _mapper;

        public AuthorizationDataService(IDatabaseFactory factory, AuthorizationMapper mapper) : base(factory)
        {
            _mapper = mapper;
        }

        public bool CreateTable()
        {
            return Execute((con) =>
            {
                return _mapper.CreateAuthorizationTable(con) > 0;
            });
        }

        public Authorization? SelectLastestAuthorization()
        {
            return Execute((con) =>
            {
                return _mapper.SelectLastestAuthorization(con);
            });
        }
        public IEnumerable<string> SelectOrderByAccesToken(int limit)
        {
            return Execute((con) =>
            {
                return _mapper.SelectOrderByAccesToken(con, limit);
            });
        }
        public bool InsertOrUpdateAuthorization(Authorization authorization)
        {
            return ExecuteTrans((con, transaction) =>
            {
                if (_mapper.SelectAccessToken(con, authorization.Type) is string)
                {
                    return _mapper.UpdateAuthorization(con, transaction, authorization) > 0;
                }
                else
                {
                    return _mapper.InsertAuthorization(con, transaction, authorization) > 0;
                }
            });
        }
        public bool InsertAuthorization(Authorization authorization)
        {
            return ExecuteTrans((con, transaction) =>
            {
                return _mapper.InsertAuthorization(con, transaction, authorization) > 0;
            });
        }

        public bool UpdateAuthorization(Authorization authorization)
        {
            return ExecuteTrans((con, transaction) =>
            {
                return _mapper.UpdateAuthorization(con, transaction, authorization) > 0;
            });
        }

        public bool DeleteAuthorization(string key)
        {
            return ExecuteTrans((con, transaction) =>
            {
                return _mapper.DeleteAuthorization(con, transaction, key) > 0;
            });
        }
    }
}
