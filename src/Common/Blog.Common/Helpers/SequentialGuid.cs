using System;

namespace Blog.Common.Helpers;

/// <summary>
/// Generates COMB-like sequential GUIDs which are more index-friendly when used as clustered keys.
/// This implementation appends a timestamp to the GUID's last 6 bytes.
/// Not cryptographically secure; suitable for DB primary keys to reduce index fragmentation.
/// </summary>
public static class SequentialGuid
{
    public static Guid NewGuid()
    {
        var guidBytes = Guid.NewGuid().ToByteArray();

        // Use UTC now ticks
        var now = DateTime.UtcNow;
        // Get days and milliseconds since a base (like SQL Server COMB approach)
        var days = (short)(now - new DateTime(1900, 1, 1)).TotalDays;
        var msecs = (int)(now.TimeOfDay.TotalMilliseconds / 3.333333); // scale to fit

        var daysBytes = BitConverter.GetBytes(days);
        var msecsBytes = BitConverter.GetBytes(msecs);

        // replace last 6 bytes with timestamp bytes
        // Layout chosen to mimic common COMB implementations
        guidBytes[10] = daysBytes[0];
        guidBytes[11] = daysBytes[1];
        guidBytes[12] = msecsBytes[0];
        guidBytes[13] = msecsBytes[1];
        guidBytes[14] = msecsBytes[2];
        guidBytes[15] = msecsBytes[3];

        return new Guid(guidBytes);
    }
}
