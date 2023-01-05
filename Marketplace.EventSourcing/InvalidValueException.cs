﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.EventSourcing
{
    public class InvalidValueException : Exception
    {
        public InvalidValueException(Type type, string message)
            : base($"Value of {type.Name} {message}") { }
    }
}
