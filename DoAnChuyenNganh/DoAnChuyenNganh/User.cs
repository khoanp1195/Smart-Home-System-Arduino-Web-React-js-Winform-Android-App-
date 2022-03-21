using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh
{
    class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string Rule { get; set; }

        public User() { }

        public User(string name, int age, string gender, string address, int phone, string username, string password)
        {
            Name = name;
            Age = age;
            Gender = gender;
            Address = address;
            Phone = phone;
            Username = username;
            Password = password;
        }

        public User(string name, int age, string gender, string address, int phone, string username, string password, string image)
        {
            Name = name;
            Age = age;
            Gender = gender;
            Address = address;
            Phone = phone;
            Username = username;
            Password = password;
            Image = image;
        }

        public User(string name, int age, string gender, string address, int phone, string username, string password, string image, string rule)
        {
            Name = name;
            Age = age;
            Gender = gender;
            Address = address;
            Phone = phone;
            Username = username;
            Password = password;
            Image = image;
            Rule = rule;
        }

        public static string error = "";

        public static bool checkLogin(User user1, User user2)
        {
            if(user1 == null || user2 == null)
            {
                error = "Tài khoản hoặc mật khẩu không hợp lệ. Vui lòng kiểm tra lại";
                return false;
            }
            if(user1.Username != user2.Username)
            {
                error = "Tài khoản này không tồn tại vui lòng kiểm tra lại!";
                return false;
            }
            if (user1.Password != user2.Password)
            {
                error = "Mật khẩu không đúng vui lòng kiểm tra lại!";
                return false;
            }
            return true;
        }
    }
}
