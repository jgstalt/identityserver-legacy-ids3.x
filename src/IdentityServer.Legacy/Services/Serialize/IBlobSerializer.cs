﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Legacy.Services.Serialize
{
    public interface IBlobSerializer
    {
        string SerializeObject(object obj);
        T DeserializeObject<T>(string text);
    }
}
