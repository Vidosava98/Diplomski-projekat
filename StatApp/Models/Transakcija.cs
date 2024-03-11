using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace fixit.Models
{
    public class Transakcija
    {

        [Key]
        public int Transakcija_id { get; set; }
        public int Potrosnja { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Jmbg { get; set; }
        public string Proizvod { get; set; }
    }
}