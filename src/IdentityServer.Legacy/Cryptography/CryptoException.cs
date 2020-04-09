﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Legacy.Cryptography
{
    public class CryptoException : Exception
    {
        public CryptoException()
            : base()
        {

        }

        public CryptoException(string message)
            : this(message, null)
        {

        }

        public CryptoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
        public CryptoException(Exception innerException)
            : this(String.Empty, innerException)
        {

        }
    }
}
