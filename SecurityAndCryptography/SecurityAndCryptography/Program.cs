using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

namespace SecurityAndCryptography
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            // Limiting who can call your code.
            PrincipalPermissionDeclarativeDemo();
            //PrincipalPermissionImperativeDemo();

            // Limiting what your code can do.
            //FileIOPermissionDemo(@"D:\TestFiles\testfile.txt", "test");

            //HashingDemo();

            //ProtectedMemoryDemo();

            //ProtectedDataDemo();

            //SymmetricEncryptionDemo();

            //AsymmetricEncryptionDemo();

            //DigitalSigningDemo();

            //ExamPrep();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Name = @"mattlt1\matta", Role = "Administrators")]
        private static void PrincipalPermissionDeclarativeDemo()
        {
            Console.WriteLine("User has permission to call this method.");
        }

        private static void PrincipalPermissionImperativeDemo()
        {
            PrincipalPermission principalPerm = new PrincipalPermission(@"mattlt1\matta", "Administrators", true);
            principalPerm.Demand();

            Console.WriteLine("User has permission to call this method.");
        }

        //[FileIOPermission(SecurityAction.PermitOnly, Read = @"D:\TestFiles")]
        //[FileIOPermission(SecurityAction.PermitOnly, Write = @"D:\TestFiles")]
        private static void FileIOPermissionDemo(string path, string contents)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(contents);
            }
        }

        private static void HashingDemo()
        {
            // Hash and "save" the password
            string myPassword = "P@ssw0rd!";
            byte[] data = Encoding.UTF8.GetBytes(myPassword);
            byte[] hash = SHA256.Create().ComputeHash(data);
            Console.WriteLine(Encoding.UTF8.GetString(hash));

            // Confirm the user entered the same password.
            byte[] data2 = Encoding.UTF8.GetBytes(myPassword);
            byte[] hash2 = SHA256.Create().ComputeHash(data);
            Console.WriteLine(Encoding.UTF8.GetString(hash2));

            Console.WriteLine(hash.SequenceEqual(hash2));
        }

        private static void ProtectedMemoryDemo()
        {
            byte[] message = Encoding.UTF8.GetBytes("Some very sensitive data here!!!");

            Console.WriteLine("Original data: " + Encoding.UTF8.GetString(message));
            Console.WriteLine("Encrypting...");

            // Encrypt the data in memory.
            ProtectedMemory.Protect(message, MemoryProtectionScope.SameLogon);

            Console.WriteLine("Encrypted data: " + Encoding.UTF8.GetString(message));
            Console.WriteLine("Decrypting...");

            // Decrypt the data in memory.
            ProtectedMemory.Unprotect(message, MemoryProtectionScope.SameLogon);

            Console.WriteLine("Decrypted data: " + Encoding.UTF8.GetString(message));
        }

        private static void ProtectedDataDemo()
        {
            // Create the original data to be encrypted
            byte[] message = Encoding.UTF8.GetBytes("Some very sensitive persistent data here!!!");

            // Create some random entropy.
            byte[] entropy = CreateRandomEntropy();

            Console.WriteLine();
            Console.WriteLine("Original data: " + Encoding.UTF8.GetString(message));
            Console.WriteLine("Encrypting and writing to disk...");

            byte[] encryptedData = ProtectedData.Protect(message, entropy, DataProtectionScope.CurrentUser);

            // Create a file.
            int bytesWritten = 0;
            using (FileStream stream = new FileStream("Data.dat", FileMode.OpenOrCreate))
            {
                stream.Write(encryptedData, 0, encryptedData.Length);
                bytesWritten = encryptedData.Length;
            }

            Console.WriteLine("Reading data from disk and decrypting...");

            // Open the file.
            using (FileStream stream = new FileStream("Data.dat", FileMode.Open))
            {
                // Read from the stream and decrypt the data.
                byte[] inBuffer = new byte[bytesWritten];

                stream.Read(inBuffer, 0, bytesWritten);
                byte[] decryptData = ProtectedData.Unprotect(inBuffer, entropy, DataProtectionScope.CurrentUser);

                Console.WriteLine("Decrypted data: " + Encoding.UTF8.GetString(decryptData));
            }
        }

        public static byte[] CreateRandomEntropy()
        {
            // Create a byte array to hold the random value.
            byte[] entropy = new byte[16];

            // Create a new instance of the RNGCryptoServiceProvider.
            // Fill the array with a random value.
            new RNGCryptoServiceProvider().GetBytes(entropy);

            // Return the array.
            return entropy;
        }

        private static void SymmetricEncryptionDemo()
        {
            string plainText = "My super secret string";
            byte[] encryptedValue;
            string decryptedValue;

            byte[] key;
            byte[] iv;

            // Encrypt the value using symmetric encryption.
            using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
            {
                //provider.GenerateKey();
                key = csp.Key;
                Console.WriteLine("Key: {0}", Encoding.UTF8.GetString(key));

                //provider.GenerateIV();
                iv = csp.IV;
                Console.WriteLine("IV: {0}", Encoding.UTF8.GetString(iv));

                ICryptoTransform encryptor = csp.CreateEncryptor(key, iv);

                // Create the streams used for encryption.
                using (MemoryStream stream = new MemoryStream())
                using (CryptoStream crypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(crypt))
                    {
                        writer.Write(plainText);
                    }

                    encryptedValue = stream.ToArray();
                }
            }

            // Decrypt the value using symmetric encryption.
            using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
            {
                ICryptoTransform decryptor = csp.CreateDecryptor(key, iv);

                // Create the streams for decryption.
                using (MemoryStream stream = new MemoryStream(encryptedValue))
                using (CryptoStream crypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Read))
                using (StreamReader reader = new StreamReader(crypt))
                {
                    decryptedValue = reader.ReadToEnd();
                }

                //Display the original data and the decrypted data.
                Console.WriteLine("Plain text: {0}", plainText);
                Console.WriteLine("Encrypted value: {0}", Encoding.UTF8.GetString(encryptedValue));
                Console.WriteLine("Decrypted value: {0}", decryptedValue);
            }
        }

        private static void AsymmetricEncryptionDemo()
        {
            string plainText = "My super secret string";
            string privateKey;
            string publicKey;
            byte[] encryptedValue;
            string decryptedValue;

            // Get a public/private key pair.
            using (RSACryptoServiceProvider csp = new RSACryptoServiceProvider())
            {
                publicKey = csp.ToXmlString(false);
                privateKey = csp.ToXmlString(true);
            }

            Console.WriteLine();
            Console.WriteLine("Public key: {0}{1}", Environment.NewLine, publicKey);

            Console.WriteLine();
            Console.WriteLine("Private key: {0}{1}", Environment.NewLine, privateKey);

            // Encrypt the value using asymmetric encryption using the public key.
            using (RSACryptoServiceProvider csp = new RSACryptoServiceProvider())
            {
                csp.FromXmlString(publicKey);

                encryptedValue = csp.Encrypt(Encoding.UTF8.GetBytes(plainText), true);
            }

            // Decrypt the value using asymmetric encryption using the private key.
            using (RSACryptoServiceProvider csp = new RSACryptoServiceProvider())
            {
                csp.FromXmlString(privateKey);

                decryptedValue = Encoding.UTF8.GetString(csp.Decrypt(encryptedValue, true));
            }

            Console.WriteLine();
            Console.WriteLine("Plain text: {0}", plainText);
            //Console.WriteLine("Encrypted value: {0}", Encoding.UTF8.GetString(encryptedValue));
            Console.WriteLine("Decrypted value: {0}", decryptedValue);
        }

        private static void DigitalSigningDemo()
        {
            string plainText = "My super important message.";
            string sha256Id = CryptoConfig.MapNameToOID("SHA256");
            string privateKey;
            string publicKey;
            byte[] signature;
            byte[] bogusSignature = new byte[] { 0, 1, 2, 3 };
            bool isValid;

            // Get a public/private key pair.
            using (RSACryptoServiceProvider csp = new RSACryptoServiceProvider())
            {
                publicKey = csp.ToXmlString(false);
                privateKey = csp.ToXmlString(true);
            }

            Console.WriteLine();
            Console.WriteLine("Public key: {0}{1}", Environment.NewLine, publicKey);

            Console.WriteLine();
            Console.WriteLine("Private key: {0}{1}", Environment.NewLine, privateKey);

            // Sign the data using the private key and SHA256.
            using (RSACryptoServiceProvider csp = new RSACryptoServiceProvider())
            {
                csp.FromXmlString(privateKey);

                signature = csp.SignData(Encoding.UTF8.GetBytes(plainText), sha256Id);
            }

            // Verify the signature using the public key and SHA256.
            using (RSACryptoServiceProvider csp = new RSACryptoServiceProvider())
            {
                csp.FromXmlString(publicKey);

                //signature = bogusSignature;
                isValid = csp.VerifyData(Encoding.UTF8.GetBytes(plainText), sha256Id, signature);
            }

            Console.WriteLine();
            Console.WriteLine("Plain text: {0}", plainText);
            //Console.WriteLine("Signature: {0}", Encoding.UTF8.GetString(signature));
            Console.WriteLine("IsValid? {0}", isValid);
        }

        interface IFile
        {
            void Open();
        }

        interface IDatabase
        {
            void Open();
        }

        private class UseResources : IFile, IDatabase
        {
            void IDatabase.Open()
            {
                Console.WriteLine("Database opened.");
            }

            void IFile.Open()
            {
                Console.WriteLine("File opened.");
            }
        }

        public static void ExamPrep()
        {
            var manager = new UseResources();
            ((IFile)manager).Open();
            ((IDatabase)manager).Open();
        }
    }
}
