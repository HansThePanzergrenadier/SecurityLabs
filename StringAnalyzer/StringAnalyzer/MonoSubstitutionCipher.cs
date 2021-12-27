namespace CipherBreaker
{
    public class MonoSubstitutionCipher
    {
        public static string Encrypt(string alphabet, string plainText)
        {
            return PolySubstitutionCipher.Encrypt(new[] { alphabet }, plainText);
        }

        public static string Decrypt(string alphabet, string plainText)
        {
            return PolySubstitutionCipher.Decrypt(new[] { alphabet }, plainText);
        }
    }
}