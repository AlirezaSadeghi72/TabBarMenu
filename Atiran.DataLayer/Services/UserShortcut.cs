using System.Drawing;

namespace Atiran.DataLayer.Services
{
    public class UserShortcut
    {
        //------------- Menu

        public Bitmap Ico { get; set; }
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
        public bool? v1 { get; set; }
        public bool? v2 { get; set; }
        public bool? v3 { get; set; }
        public bool? v4 { get; set; }
        public bool? v5 { get; set; }
        public bool? v6 { get; set; }
        public bool? v7 { get; set; }
        public bool? v8 { get; set; }
        public bool? v9 { get; set; }
        public bool? v10 { get; set; }
    }
}