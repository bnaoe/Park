using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkAPI.Models;

namespace ParkAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(String username);
        User Authenticate(string username, string password);
        User Register(string username, string password);
    }
}
