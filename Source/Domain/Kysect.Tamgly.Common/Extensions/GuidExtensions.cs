﻿namespace Kysect.Tamgly.Common;

public static class GuidExtensions
{
    public static string ToShortString(this Guid value)
    {
        return value.ToString().Substring(0, 5);
    }
}