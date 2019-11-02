using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atiran.DataLayer.Services
{
    public class ActiveUser
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_lname { get; set; }
        public string user_fname { get; set; }
        public byte[] user_pic { get; set; }
        public Nullable<bool> IsLoggedIn { get; set; }
        public Nullable<bool> active { get; set; }
    }
}
