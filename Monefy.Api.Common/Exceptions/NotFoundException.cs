﻿using System;

namespace Monefy.Api.Common
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
