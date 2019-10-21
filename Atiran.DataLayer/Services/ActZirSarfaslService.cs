using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atiran.DataLayer.Services
{
    public class ActZirSarfaslService
    {
        public int row { get; set; }
        public string date { get; set; }
        public string user { get; set; }
        public Nullable<int> sanadno { get; set; }
        public int ID { get; set; }
        public int ZirSarfaslID { get; set; }
        public string description { get; set; }
        public decimal bed { get; set; }
        public decimal bes { get; set; }
        public string bed_bes { get; set; }
        public Nullable<int> kind { get; set; }
        public decimal Man { get; set; }
        public string kindName { get; set; }

    }
}
