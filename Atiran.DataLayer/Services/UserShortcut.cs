using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atiran.DataLayer.Services
{
    public class UserShortcut
    {
        //------------- Menu

        public string Text { get; set; }
        public string Shortcut { get; set; }


        //-------- userFavorite
        public int RowID { get; set; }
        public int UserID { get; set; }
        public int MenuID { get; set; }

        //------------- Form

        public int FormID { get; set; }
        public string Title { get; set; }
        public string NameSpace { get; set; }
        public string Class { get; set; }
        public string Description { get; set; }
        public Nullable<bool> v1 { get; set; }
        public Nullable<bool> v2 { get; set; }
        public Nullable<bool> v3 { get; set; }
        public Nullable<bool> v4 { get; set; }
        public Nullable<bool> v5 { get; set; }
        public Nullable<bool> v6 { get; set; }
        public Nullable<bool> v7 { get; set; }
        public Nullable<bool> v8 { get; set; }
        public Nullable<bool> v9 { get; set; }
        public Nullable<bool> v10 { get; set; }
    }
}
