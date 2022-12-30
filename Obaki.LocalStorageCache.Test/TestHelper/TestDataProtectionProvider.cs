using Microsoft.AspNetCore.DataProtection;
using System.Text;
using System.Threading.Tasks;

namespace Obaki.LocalStorageCache.Test.TestHelper
{
    //Reused same test class from https://github.com/dotnet/aspnetcore/blob/main/src/Components/Server/test/ProtectedBrowserStorageTest.cs
    public class TestDataProtectionProvider : IDataProtectionProvider
    {
        public List<string> ProtectorsCreated { get; } = new List<string>();

        public static string Protect(string purpose, string plaintext)
            => new TestDataProtector(purpose).Protect(plaintext);

        public static string Unprotect(string purpose, string protectedValue)
            => new TestDataProtector(purpose).Unprotect(protectedValue);

        public IDataProtector CreateProtector(string purpose)
        {
            ProtectorsCreated.Add(purpose);
            return new TestDataProtector(purpose);
        }

        class TestDataProtector : IDataProtector
        {
            private readonly string _purpose;

            public TestDataProtector(string purpose)
            {
                _purpose = purpose;
            }

            public IDataProtector CreateProtector(string purpose)
            {
                throw new NotImplementedException();
            }

            public byte[] Protect(byte[] plaintext)
            {
                // The test cases will only involve passing data that was originally converted from strings
                var plaintextString = Encoding.UTF8.GetString(plaintext);
                var fakeProtectedString = $"{ProtectionPrefix(_purpose)}{plaintextString}";
                return Encoding.UTF8.GetBytes(fakeProtectedString);
            }

            public byte[] Unprotect(byte[] protectedData)
            {
                // The test cases will only involve passing data that was originally converted from strings
                var protectedString = Encoding.UTF8.GetString(protectedData);

                var expectedPrefix = ProtectionPrefix(_purpose);
                if (!protectedString.StartsWith(expectedPrefix, StringComparison.Ordinal))
                {
                    throw new ArgumentException($"The value is not protected with the expected purpose '{_purpose}'. Value supplied: '{protectedString}'", nameof(protectedData));
                }

                var unprotectedString = protectedString.Substring(expectedPrefix.Length);
                return Encoding.UTF8.GetBytes(unprotectedString);
            }

            private static string ProtectionPrefix(string purpose)
                => $"PROTECTED:{purpose}:";
        }
    }
}
