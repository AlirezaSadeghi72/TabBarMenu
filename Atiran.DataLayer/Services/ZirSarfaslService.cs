namespace Atiran.DataLayer.Services
{
    public class ZirSarfaslService
    {
        public int row { get; set; }
        public int ID { get; set; }
        public int SarfaslID { get; set; }
        public string Name { get; set; }
        public string has_dar { get; set; }
        public decimal bed { get; set; }
        public decimal bes { get; set; }
        public decimal Man { get; set; }
        public string bed_bes { get; set; }
        public decimal Man_Befor { get; set; }
        public string bed_bes_Befor { get; set; }

        public bool? Active { get; set; }
    }
}