using System;
using System.Collections.Generic;

namespace KurssiApiNet6.Models
{
    public partial class Kurssit
    {
        public int KurssiId { get; set; }
        public string Nimi { get; set; } = null!;
        public int? Laajuus { get; set; }
    }
}
