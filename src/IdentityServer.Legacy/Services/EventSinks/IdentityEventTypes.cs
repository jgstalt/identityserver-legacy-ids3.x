﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Legacy.Services.EventSinks
{
    public enum IdentityEventTypes
    {
        //
        // Summary:
        //     Success event
        Success = 1,
        //
        // Summary:
        //     Failure event
        Failure = 2,
        //
        // Summary:
        //     Information event
        Information = 3,
        //
        // Summary:
        //     Error event
        Error = 4
    }
}
