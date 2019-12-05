using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Apis.PayJunction
{
    public enum PayJunctionServer
    {
        Development,

        Production,
    }

    public enum AVSType
    {
        Address,

        Zip,

        Address_and_Zip,

        Address_or_Zip,

        Bypass,

        Off
    }
}
