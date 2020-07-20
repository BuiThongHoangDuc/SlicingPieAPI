using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SlicingPieAdmin.Helper
{
    public class SlicingPieApi
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://slicingpiepj.azurewebsites.net/");
            return client;
        }
    }
}
