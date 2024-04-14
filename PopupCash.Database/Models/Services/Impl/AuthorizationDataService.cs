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
                _mapper.CreateAuthorizationTable(con);
                return _mapper.SelectLastestAuthorization(con);
            });
        }
        public IEnumerable<string> SelectOrderByAccesToken(int limit)
        {
            return Execute((con) =>
            {
                _mapper.CreateAuthorizationTable(con);
                return _mapper.SelectOrderByAccesToken(con, limit);
            });
        }
        public bool InsertAuthorization(Authorization authorization)
        {
            return ExecuteTrans((con, transaction) =>
            {
                _mapper.CreateAuthorizationTable(con);
                return _mapper.InsertAuthorization(con, transaction, authorization) > 0;
            });
        }

        public bool UpdateAuthorization(Authorization authorization)
        {
            return ExecuteTrans((con, transaction) =>
            {
                _mapper.CreateAuthorizationTable(con);

                //// ID가 다른 경우 이전 ID를 불러와서 덮어쓴다.
                //if (_mapper.SelectLastestAuthorization(con) is Authorization oldAuth)
                //    authorization.Id = oldAuth.Id;

                return _mapper.UpdateAuthorization(con, transaction, authorization) > 0;
            });
        }

        public bool DeleteAuthorization(string id)
        {
            return ExecuteTrans((con, transaction) =>
            {
                _mapper.CreateAuthorizationTable(con);
                return _mapper.DeleteAuthorization(con, transaction, id) > 0;
            });
        }
    }
}
