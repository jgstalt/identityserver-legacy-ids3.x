﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Legacy.Token.ErrorHandling
{
    public class WrongIssuerException : TokenVerificationException
    {
        public WrongIssuerException()
            : base("wrong issuer")
        {

        }
    }
}
