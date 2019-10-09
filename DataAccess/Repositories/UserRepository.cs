

using AutoMapper;
using Model.Entities;
using Model.ViewModels.UserModels;

namespace DataAccess.Repositories {
    public class UserRepository: BaseRepository<User> {

        public UserRepository(WebApiDbContext dbContext): base(dbContext) {
        }

    }
}