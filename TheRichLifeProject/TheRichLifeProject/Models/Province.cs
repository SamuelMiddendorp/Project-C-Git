using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheRichLifeProject.Models
{
    public enum Province
    {
        [Display(Name = "North Holland")]
        NorthHolland,
        [Display(Name = "South Holland")]
        SouthHolland,
        Zeeland,
        Limburg,
        [Display(Name = "North Brabant")]
        NorthBrabant,
        Friesland,
        Gelderland,
        Utrecht,
        Drenthe,
        Overijssel,
        Flevoland,
        Groningen
    }
}
