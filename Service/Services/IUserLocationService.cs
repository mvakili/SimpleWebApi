using Model.ViewModels.Location;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IUserLocationService
    {
        Task SetUserLocationAsync(LocationViewModel input, ClaimsIdentity identity);
        LocationViewModel GetUserLocationModel(ClaimsIdentity identity);
        IEnumerable<UserLocationViewModel> GetUsersAround(double x, double y, double radius);
        IEnumerable<UserLocationViewModel> GetUsersAroundMe(ClaimsIdentity identity, double radius);


    }
}
