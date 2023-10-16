using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;

public static class Password
{
    public static string HashPassword(string password, string user)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = $"{GenerateSaltWithString(user)}{password}";
            var bytes = System.Text.Encoding.UTF8.GetBytes(saltedPassword);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    public static string GenerateSaltWithString(string inputString)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(inputString);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
