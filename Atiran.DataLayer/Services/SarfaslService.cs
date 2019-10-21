using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atiran.DataLayer.Services
{
    public class SarfaslService
    {
        public int row { get; set; }
        public int ID { get; set; }
        public int GroupSarfaslID { get; set; }
        public string has_dar { get; set; }
        public string who_def { get; set; }
        public string Name { get; set; }
        public decimal bed { get; set; }
        public decimal bes { get; set; }
        public decimal Man { get; set; }
        public string bed_bes { get; set; }
        public decimal Man_Befor { get; set; }
        public string bed_bes_Befor { get; set; }

        public List<ZirSarfaslService> ZirSarfasls { get; set; }

    }
}
