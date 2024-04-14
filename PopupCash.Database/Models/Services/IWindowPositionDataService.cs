using PopupCash.Database.Models.Locations;

namespace PopupCash.Database.Models.Services
{
    public interface IWindowPositionDataService
    {
        /// <summary>
        /// Create Table
        /// </summary>
        /// <returns></returns>
        public bool CreateTable();

        public WindowPosition? SelectWindowPostion(string windowId);

        public bool InsertWindowPostion(WindowPosition position);

        public bool UpdateWindowPostion(WindowPosition position);

        public bool DeleteWindowPostion(string windowId);
    }
}
