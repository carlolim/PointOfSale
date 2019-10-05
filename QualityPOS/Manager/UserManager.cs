using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QualityPOS.Objects;
using QualityPOS.Repository;
using QualityPOS.Objects.DTO;

namespace QualityPOS.Manager
{
    public class UserManager
    {
        RepositoryNgPinas _repositoryNgPinas = new RepositoryNgPinas();

        public async Task<User> Login(User user)
        {
            var param = new { Username = user.Username, Password = user.Password };
            user = await _repositoryNgPinas.QuerySingleAsync<User>($@"SELECT TOP 1 * FROM [User] WHERE Username = @Username AND [Password]=@Password", param);
            if (user != null)
            {
                var sql = $@"UPDATE [User] SET LastLogin = GETDATE() WHERE UserID = @UserID";
                await _repositoryNgPinas.ExecuteAsync(sql, new { UserID = user.UserID });
            }
            return user;
        }

        public async Task<Result> Add(UserDTO user)
        {
            Result result = new Result();
            result.IsSuccess = true;

            #region validation
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                result.IsSuccess = false;
                result.Message = "First name is required";
            }
            else if (string.IsNullOrWhiteSpace(user.LastName))
            {
                result.IsSuccess = false;
                result.Message = "Last name is required";
            }
            else if (string.IsNullOrWhiteSpace(user.Username))
            {
                result.IsSuccess = false;
                result.Message = "Username is required";
            }
            else if (string.IsNullOrWhiteSpace(user.Password))
            {
                result.IsSuccess = false;
                result.Message = "Password is required";
            }
            else if (string.IsNullOrWhiteSpace(user.ReTypePassword))
            {
                result.IsSuccess = false;
                result.Message = "Please re-type password";
            }
            else if (string.IsNullOrWhiteSpace(user.UserRoleStr))
            {
                result.IsSuccess = false;
                result.Message = "Please select user role";
            }
            else if (user.Password != user.ReTypePassword)
            {
                result.IsSuccess = false;
                result.Message = "Passwords do not match";
            }


            if (result.IsSuccess)
            {
                var existingUsername = await GetByUsername(user.Username);
                if (existingUsername != null)
                {
                    result.IsSuccess = false;
                    result.Message = $"Username: { user.Username } not available";
                }
            }
            #endregion

            if (result.IsSuccess)
            {
                var userRole = await GetUserRoleByName(user.UserRoleStr);
                if (userRole != null)
                {
                    user.UserRoleID = userRole.UserRoleID;
                }

                var tobeAdded = new User()
                {
                    DateCreated = DateTime.Now,
                    FirstName = user.FirstName,
                    UserRoleID = user.UserRoleID,
                    IsDeleted = false,
                    LastName = user.LastName,
                    Password = user.Password,
                    Username = user.Username
                };

                result = await _repositoryNgPinas.Insert("User", tobeAdded, "UserID");
            }

            return result;
        }


        public async Task<Result> Update(UserDTO user)
        {
            Result result = new Result();
            result.IsSuccess = true;

            #region validation
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                result.IsSuccess = false;
                result.Message = "First name is required";
            }
            else if (string.IsNullOrWhiteSpace(user.LastName))
            {
                result.IsSuccess = false;
                result.Message = "Last name is required";
            }
            else if (string.IsNullOrWhiteSpace(user.Username))
            {
                result.IsSuccess = false;
                result.Message = "Username is required";
            }
            else if (string.IsNullOrWhiteSpace(user.UserRoleStr))
            {
                result.IsSuccess = false;
                result.Message = "Please select user role";
            }


            if (result.IsSuccess)
            {
                var existingUsername = await GetByUsername(user.Username);
                if (existingUsername != null)
                {
                    if(existingUsername.UserID != user.UserID)
                    {
                        result.IsSuccess = false;
                        result.Message = $"Username: { user.Username } not available";
                    }
                }
            }

            if (result.IsSuccess)
            {
                if(!string.IsNullOrWhiteSpace(user.Password) || !string.IsNullOrWhiteSpace(user.ReTypePassword))
                {
                    if(user.Password != user.ReTypePassword)
                    {
                        result.IsSuccess = false;
                        result.Message = "Passwords do not match";
                    }
                }
            }
            #endregion

            if (result.IsSuccess)
            {
                var userRole = await GetUserRoleByName(user.UserRoleStr);
                if (userRole != null)
                {
                    user.UserRoleID = userRole.UserRoleID;
                }


                var tobeUpdated = new User()
                {
                    FirstName = user.FirstName,
                    UserRoleID = user.UserRoleID,
                    IsDeleted = false,
                    LastName = user.LastName,
                    Password = user.Password,
                    Username = user.Username,
                    DateModified = user.DateModified,
                    DateCreated = user.DateCreated,
                    LastLogin = user.LastLogin,
                    UserModifiedID = user.UserModifiedID,
                    UserID = user.UserID
                };

                var _originalUserData = GetByID(user.UserID);

                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    tobeUpdated.Password = _originalUserData.Password;
                }

                result = await _repositoryNgPinas.Update("User", tobeUpdated, new List<string>() { "UserID" }, new List<object>() { user.UserID }, "UserID");
            }

            return result;
        }

        public User GetByID(int id)
        {
            return _repositoryNgPinas.Select<User>("User", "UserID", id);
        }

        public async Task<List<UserDTO>> GetAll()
        {
            List<UserDTO> result = new List<UserDTO>();

            var sql = $@"SELECT * FROM [User] WHERE IsDeleted =0";
            result = await _repositoryNgPinas.QueryMultipleAsync<UserDTO>(sql);
            var roleSql = $@"SELECT * FROM UserRole WHERE Isdeleted = 0";
            var roles = await _repositoryNgPinas.QueryMultipleAsync<UserRole>(roleSql);

            foreach (var r in result)
            {
                r.UserRole = roles.FirstOrDefault(m => m.UserRoleID == r.UserRoleID);
            }

            return result;
        }
        public async Task<List<UserDTO>> GetAllByRole(UserRoleEnum role)
        {
            List<UserDTO> result = new List<UserDTO>();

            var sql = $@"SELECT * FROM [User] WHERE IsDeleted =0 AND UserRoleID = { (int)role }";
            result = await _repositoryNgPinas.QueryMultipleAsync<UserDTO>(sql);
            var roleSql = $@"SELECT * FROM UserRole WHERE Isdeleted = 0";
            var roles = await _repositoryNgPinas.QueryMultipleAsync<UserRole>(roleSql);

            foreach (var r in result)
            {
                r.UserRole = roles.FirstOrDefault(m => m.UserRoleID == r.UserRoleID);
            }

            return result;
        }

        public async Task<Result> Delete(int userID)
        {
            Result result = new Result();
            var sql = $@"UPDATE [User] SET IsDeleted = 1 WHERE UserID = @UserID";
            var r = await _repositoryNgPinas.ExecuteAsync(sql, new { UserID = userID });
            result.IsSuccess = r > 0;

            if (result.IsSuccess)
            {
                result.Message = "User Deleted!";
            }
            return result;
        }

        public async Task<List<UserRole>> GetUserRoles()
        {
            var sql = $@"SELECT * FROM UserRole WHERE IsDeleted = 0 ORDER BY RoleName";
            return await _repositoryNgPinas.QueryMultipleAsync<UserRole>(sql);
        }

        public async Task<UserRole> GetUserRoleByName(string rolename)
        {
            var sql = $@"SELECT * FROM UserRole WHERE RoleName = @RoleName";
            return await _repositoryNgPinas.QuerySingleAsync<UserRole>(sql, new { RoleName = rolename });
        }

        public async Task<UserRole> GetUserRoleByID(int id)
        {
            var sql = $@"SELECT * FROM UserRole WHERE UserRoleID = @ID";
            return await _repositoryNgPinas.QuerySingleAsync<UserRole>(sql, new { ID = id });
        }

        public async Task<User> GetByUsername(string userName)
        {
            var sql = $@"SELECT TOP 1 * FROM [User] WHERE Username = @Username";
            return await _repositoryNgPinas.QuerySingleAsync<User>(sql, new { Username = userName });
        }
    }
}
