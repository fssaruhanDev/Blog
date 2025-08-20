using System;
using System.Security.Cryptography;

namespace Blog.Common.Helpers;

/// <summary>
/// Simple ULID generator that returns a Guid containing the 16 ULID bytes.
/// Note: this stores ULID bytes directly into a Guid; canonical ULID string conversion may differ.
/// </summary>
public static class UlidHelper
{
    /// <summary>
    /// Generates a new ULID and returns it as a Guid whose internal bytes are the ULID bytes.
    /// Timestamp (48 bits) is placed in the first 6 bytes (big-endian) followed by 10 bytes of randomness.
    /// </summary>
    public static Guid NewGuid()
    {
        // 48-bit timestamp (milliseconds since unix epoch)
        var timestamp = (ulong)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() & 0xFFFFFFFFFFFF);

        var bytes = new byte[16];

        // timestamp big-endian into bytes[0..5]
        bytes[0] = (byte)(timestamp >> 40);
        bytes[1] = (byte)(timestamp >> 32);
        bytes[2] = (byte)(timestamp >> 24);
        bytes[3] = (byte)(timestamp >> 16);
        bytes[4] = (byte)(timestamp >> 8);
        bytes[5] = (byte)(timestamp);

        // fill remaining 10 bytes with cryptographic RNG
        RandomNumberGenerator.Fill(bytes.AsSpan(6, 10));

        return new Guid(bytes);
    }

    /// <summary>
    /// Returns the ULID bytes for a Guid produced by this helper.
    /// </summary>
    public static byte[] ToUlidBytes(Guid guid)
    {
        return guid.ToByteArray();
    }
}
