namespace Atiran.DataLayer.Services
{
    public class ActiveUser
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_lname { get; set; }
        public string user_fname { get; set; }
        public byte[] user_pic { get; set; }
        public bool? IsLoggedIn { get; set; }
        public bool? active { get; set; }
    }
}