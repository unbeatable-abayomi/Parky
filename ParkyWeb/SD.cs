﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb
{
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:44310/";
        public static string NationalParkAPIPath = APIBaseUrl+ "api/v1/nationalParks";
        public static string TrailAPIPath = APIBaseUrl+ "api/v1/trails";
    }
}
