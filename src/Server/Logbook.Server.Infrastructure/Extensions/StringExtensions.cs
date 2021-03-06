﻿using System;
using Logbook.Shared;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToBase64UrlSafe(this byte[] self)
        {
            Guard.NotNull(self, nameof(self));

            return Convert.ToBase64String(self)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }

        public static byte[] FromBase64UrlSafe(this string self)
        {
            Guard.NotNullOrWhiteSpace(self, nameof(self));

            string base64 = self
                .Replace("-", "+")
                .Replace("_", "/");

            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}