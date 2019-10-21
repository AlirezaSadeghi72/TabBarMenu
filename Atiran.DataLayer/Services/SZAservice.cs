using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atiran.DataLayer.Services
{
    public class SZAservice
    {
        public int Srow { get; set; }
        public int SID { get; set; }
        public string SGroupSarfaslName { get; set; }
        public int SGroupSarfaslID { get; set; }
        public string Shas_dar { get; set; }
        public string Swho_def { get; set; }
        public string SName { get; set; }
        public decimal Sbed { get; set; }
        public decimal Sbes { get; set; }
        public decimal SMan { get; set; }
        public string Sbed_bes { get; set; }
        public decimal SMan_Befor { get; set; }
        public string Sbed_bes_Befor { get; set; }
        public decimal SMan_All { get; set; }
        public string Sbed_bes_All { get; set; }


        public int Zrow { get; set; }
        public int ZID { get; set; }
        public int ZSarfaslID { get; set; }
        public string Zhas_dar { get; set; }
        public string ZName { get; set; }
        public decimal Zbed { get; set; }
        public decimal Zbes { get; set; }
        public decimal ZMan { get; set; }
        public string Zbed_bes { get; set; }
        public decimal ZMan_Befor { get; set; }
        public string Zbed_bes_Befor { get; set; }
        public decimal ZMan_All { get; set; }
        public string Zbed_bes_All { get; set; }
        public Nullable<bool> ZActive { get; set; }

        public int Arow { get; set; }
        public string Adate { get; set; }
        public string Auser { get; set; }
        public Nullable<int> Asanadno { get; set; }
        public int AID { get; set; }
        public int AZirSarfaslID { get; set; }
        public string Adescription { get; set; }
        public decimal Abed { get; set; }
        public decimal Abes { get; set; }
        public string Abed_bes { get; set; }
        public decimal AMan { get; set; }
        public string AManbed_bes { get; set; }
        public Nullable<int> Akind { get; set; }
        public string AkindName { get; set; }
    }
}
