using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class NationalParkRepo : GenericRepository<NationalParkWeb>, INationalParkRepo
    {
        public NationalParkRepo(HttpClient client) : base(client)
        {
        }
    }
}
