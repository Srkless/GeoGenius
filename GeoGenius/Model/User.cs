using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoGenius.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Score { get; set; }

        public User(string username, string password, int score)
        {
            Username = username;
            Password = password;
            Score = score;
        }
    }
}
