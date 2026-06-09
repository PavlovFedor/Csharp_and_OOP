using System;
using System.IO;
using System.Security.Cryptography;

namespace Encryptor
{
    internal class Program
    {
        private static void EncryptData(string inName, string outName, byte[] tdesKey, byte[] tdesIV)
        {
            //Create the file streams to handle the input and output files.
            FileStream fin = new FileStream(inName, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(outName, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            //Create variables to help with read and write.
            byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
            long rdlen = 0;              //This is the total number of bytes written.
            long totlen = fin.Length;    //This is the total length of the input file.
            int len;                     //This is the number of bytes to be written at a time.

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(fout, tdes.CreateEncryptor(tdesKey, tdesIV), CryptoStreamMode.Write);

            Console.WriteLine("Encrypting...");

            //Read from the input file, then encrypt and write to the output file.
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                encStream.Write(bin, 0, len);
                rdlen = rdlen + len;
                Console.WriteLine("{0} bytes processed", rdlen);
            }
            Console.WriteLine("Text has encrypted to encrypted.txt");
            encStream.Close();
        }1
        static void Main(string[] args)
        {
            string inPath = "F:/lab2/source.txt";
            string outPath = "F:/lab2/encrypted.txt";
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.GenerateKey();
            tdes.GenerateIV();
            File.WriteAllBytes("F:/lab2/key.dat", tdes.Key);
            File.WriteAllBytes("F:/lab2/iv.dat", tdes.IV);
            EncryptData(inPath, outPath, tdes.Key, tdes.IV);
        }
    }
}
