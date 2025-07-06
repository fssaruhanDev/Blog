using System;
using System.Security.Cryptography;
using System.Text;

namespace Jotem.Common.Infrastructure;

public class PasswordEncryptor
{
	public static string Encrypt(string password)
	{
		using var md5 = MD5.Create();
		byte[] getBytes = Encoding.ASCII.GetBytes(password);
		byte[] hashByte = md5.ComputeHash(getBytes);

		return Convert.ToHexString(hashByte);
	}
}

