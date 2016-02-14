using System;
using Logbook.Shared;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StripEverythingAfterSeconds(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);
        }
    }
}