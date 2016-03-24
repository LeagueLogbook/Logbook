using System;

namespace Logbook.Shared
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SecureAttribute : Attribute
    {
         
    }
}