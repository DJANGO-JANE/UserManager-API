using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class ResponsePayload
    {
        public bool IsSuccessful { get; set; } = true;
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public List<string> ErrorMessages { get; set; } 
    }
}
