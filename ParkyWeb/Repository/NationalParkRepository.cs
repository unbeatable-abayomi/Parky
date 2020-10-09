using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
    {
        private readonly IHttpClientFactory _clientfactory;

		public NationalParkRepository(IHttpClientFactory clientFactory) : base(clientFactory)
		{
            _clientfactory = clientFactory;
		}
    }
}
