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
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsExistUser(string email);
        /// <summary>
        /// Select User 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User? SelectUser(string email);
        /// <summary>
        /// Insert User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool InsertUser(User user);
        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateUser(User user);
        /// <summary>
        /// Delte User
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool DeleteUser(string email);
    }
}
