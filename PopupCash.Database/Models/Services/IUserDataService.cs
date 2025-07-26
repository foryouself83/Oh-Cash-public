using PopupCash.Database.Models.Users;

namespace PopupCash.Database.Models.Services
{
    public interface IUserDataService
    {
        /// <summary>
        /// Create User Table
        /// </summary>
        /// <returns></returns>
        public bool CreateTable();
        /// <summary>
        /// User 정보가 존재 여부 확인
        /// </summary>
        /// <param name="accessToekn"></param>
        /// <returns></returns>
        public bool IsExistUser(string accessToekn);
        /// <summary>
        /// Select User 
        /// </summary>
        /// <param name="accessToekn"></param>
        /// <returns></returns>
        public UserData? SelectUser(string accessToekn);


        /// <summary>
        /// Insert / Update Authorization
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        public bool InsertOrUpdateAuthorization(UserData user);

        /// <summary>
        /// Insert User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool InsertUser(UserData user);
        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateUser(UserData user);
        /// <summary>
        /// Delte User
        /// </summary>
        /// <param name="accessToekn"></param>
        /// <returns></returns>
        public bool DeleteUser(string accessToekn);
    }
}
