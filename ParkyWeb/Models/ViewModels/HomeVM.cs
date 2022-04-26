using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<NationalParkWeb> NationalParkList { get; set; }
        public IEnumerable<TrailWeb> TrailList { get; set; }
    }
}
