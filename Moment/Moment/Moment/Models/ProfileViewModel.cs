using System.Collections.Generic;
using Moment.Models;

namespace Moment.ViewModels
{
    public class ProfileViewModel
    {
        public User CurrentUser { get; set; }

        // Takip edilebilecek kullanıcılar gibi başka property'ler de olabilir:
        public List<User> FollowableUsers { get; set; }

        // Yeni eklediğimiz property:
        public List<User> Following { get; set; }
    }
}
