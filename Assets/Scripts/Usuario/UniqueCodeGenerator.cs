using System.Security.Cryptography;
using System.Text;

public class UniqueCodeGenerator
{
    public static string GenerateCode(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Computa el hash del input
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Convierte el hash a una cadena hexadecimal
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            // Toma los primeros 8 caracteres del hash para el código único
            return builder.ToString().Substring(0, 8).ToUpper();
        }
    }
}