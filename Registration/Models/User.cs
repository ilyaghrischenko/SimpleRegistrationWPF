using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace Registration.Models
{
    public class User
    {
        private string _login = "NoLogin123";
        private string _password = "NoPassword123";

        public User() { }
        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }

        [JsonPropertyName("login")]
        public string Login
        {
            get { return _login; }
            set
            {
                if (value == string.Empty) throw new ArgumentException("Error: Login cannot be empty");

                uint kilk_upper = 0;
                uint kilk_digits = 0;
                foreach (var item in value)
                {
                    if (char.IsDigit(item)) ++kilk_digits;
                    if (char.IsUpper(item)) ++kilk_upper;
                }

                if (kilk_upper >= 1 && kilk_digits >= 2) _login = value;
                else throw new ArgumentException("Error: Login must contain at least one uppercase letter and two digits");
            }
        }

        [JsonPropertyName("password")]
        public string Password
        {
            get { return _password; }
            set
            {
                if (value == string.Empty) throw new ArgumentException("Error: Password cannot be empty");

                uint kilk_upper = 0;
                uint kilk_digits = 0;
                foreach (var item in value)
                {
                    if (char.IsDigit(item)) ++kilk_digits;
                    if (char.IsUpper(item)) ++kilk_upper;
                }

                if (kilk_upper >= 1 && kilk_digits >= 2) _password = value;
                else throw new ArgumentException("Error: Password must contain at least one uppercase letter and two digits");
            }
        }

        public bool IsValid(List<User> users)
        {
            foreach (var item in users)
            {
                if (item.Login == Login) return false;
            }
            return true;
        }
    }
}
