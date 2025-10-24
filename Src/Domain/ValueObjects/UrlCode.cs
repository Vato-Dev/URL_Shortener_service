using System.Security.Cryptography;
using System.Text;

namespace Domain.ValueObjects;

public sealed record UrlCode
{
    public string Value { get;}
    private UrlCode(string value)
    {
        Value = value;
    }
    public static UrlCode Create(string value) //i am not sure but throwing exceptions there maybe is not a good idea.
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Code cannot be empty.");

        if (value.Length < 3 || value.Length > 50)
            throw new ArgumentException("Code must be between 4 and 10 characters.");

        if (!value.All(char.IsLetterOrDigit))
            throw new ArgumentException("Code must contain only letters and digits.");

        return new UrlCode(value);
    }

    public static UrlCode FromDb(string value)
    {
        return new UrlCode(value); 
    }
    public static UrlCode GenerateCode()
    {
        
        const string alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        const int length = 6;
        var bytes =   RandomNumberGenerator.GetBytes(8); // better that Guid.NewGuid().ToByteArray(); and much better that //Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
        StringBuilder sb = new StringBuilder();
        ulong num = BitConverter.ToUInt32(bytes, 0);
        for (int i = 0; i < length; i++)
        {
            ulong temp = num % 62;
            sb.Append(alphabet[(int)temp]);
            num/=62;
        }
        return new UrlCode(sb.ToString());
    }
    public static implicit operator string(UrlCode urlCode) => urlCode.Value;
    public static implicit operator UrlCode(string value) => FromDb(value);

}