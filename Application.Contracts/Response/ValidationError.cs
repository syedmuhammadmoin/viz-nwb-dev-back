﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Response
{
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string error { get; }


        public ValidationError(string field,  string message)
        {
            Field = field != string.Empty ? field : null;
            error = message;
        }
    }
}
