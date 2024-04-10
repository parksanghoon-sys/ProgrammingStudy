using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace WpCommunityToolkitMVVM.Models
{
    public class User
    {
        public string? Id { get; set; }
        public string? Message { get; set; }

        public User createUser(string Id , string? Message)
        {
            User user = new User
            {
                Id = Id,
                Message = Message
            };
            return user;
        }
            

    }
}
