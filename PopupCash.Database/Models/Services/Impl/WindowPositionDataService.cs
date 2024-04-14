using PopupCash.Database.Models.Locations;
using PopupCash.Database.Models.Mappers;

namespace PopupCash.Database.Models.Services.Impl
{
    public class WindowPositionDataService : DbServiceBase, IWindowPositionDataService
    {
        private readonly WindowPositionMapper _mapper;
        public WindowPositionDataService(IDatabaseFactory factory, WindowPositionMapper mapper) : base(factory)
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

        public WindowPosition? SelectWindowPostion(string windowId)
        {
            return Execute((con) =>
            {
                _mapper.CreateTable(con);
                return _mapper.SelectWindowPostion(con, windowId);
            });
        }

        public bool InsertWindowPostion(WindowPosition position)
        {
            return ExecuteTrans((con, transaction) =>
            {
                _mapper.CreateTable(con);
                return _mapper.InsertWindowPostion(con, transaction, position) > 0;
            });
        }

        public bool UpdateWindowPostion(WindowPosition position)
        {
            return ExecuteTrans((con, transaction) =>
            {
                _mapper.CreateTable(con);
                return _mapper.UpdateWindowPostion(con, transaction, position) > 0;
            });
        }

        public bool DeleteWindowPostion(string windowId)
        {
            return ExecuteTrans((con, transaction) =>
            {
                _mapper.CreateTable(con);
                return _mapper.DeleteWindowPostion(con, transaction, windowId) > 0;
            });
        }
    }
}
