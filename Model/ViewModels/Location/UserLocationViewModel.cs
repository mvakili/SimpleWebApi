using Model.ViewModels.UserModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels.Location
{
    public class UserLocationViewModel
    {
        public UserViewModel User { get; set; }
        public LocationViewModel Location { get; set; }
    }
}
