using System;
using System.Collections.Generic;
using System.Text;

namespace Control
{
    class User
    {
        public User(int id, string name)
        {
            this.id = id;
            this.name = name;
            status = "Offline";
        }

        public int id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
    }
}
