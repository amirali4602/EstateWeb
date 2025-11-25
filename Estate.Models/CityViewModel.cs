using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estate.Models
{

    public class CityViewModel
    {
        public City SelectedCity { get; set; }
        public IEnumerable<City> CityList { get; set; }
    }

}
