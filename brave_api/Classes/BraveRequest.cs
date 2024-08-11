using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using brave_api.Classes;

namespace brave_api.Requests
{
    public class BraveRequest
    {
        public required Person PersonA { get; set; }
        public required Person PersonB { get; set; }
    }
}