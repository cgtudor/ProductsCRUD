﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsCRUD.CustomExceptionHandler
{
    public class ErrorModel
    {
        [JsonRequired]
        public int StatusCode { get; set; }
        [JsonRequired]
        public string ErrorMessage { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
