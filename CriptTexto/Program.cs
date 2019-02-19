using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CriptTexto
{
    class Program
    {
        static void Main(string[] args)
        {
            var ByteConverter = new UnicodeEncoding();
            var RSA = new RSACryptoServiceProvider();
            var textoPlano = ByteConverter.GetBytes("93874518");
            var textoCifrado = Criptografia.RSACifra(textoPlano, RSA.ExportParameters(false), false);

            var path = @"C:\Temp\arquivo.txt";

            FileInfo file = new FileInfo(path);
            StreamWriter sw = file.CreateText();
            sw.WriteLine(ByteConverter.GetString(textoCifrado));
            sw.Close();

            var textoDecifrado = Criptografia.RSADecifra(textoCifrado, RSA.ExportParameters(true), false);

            Console.WriteLine(ByteConverter.GetString(textoPlano));

            Console.ReadKey();
        }
    }

    public class Criptografia
    {
        static public byte[] RSACifra(byte[] byteCifrado, RSAParameters RSAInfo, bool isOAEP)
        {
            try
            {
                byte[] DadosCifrados;

                // Cria a nova instância de RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    // Importa a informação da chave RSA.
                    // Feito apenas para incluir a informação da chave pública
                    RSA.ImportParameters(RSAInfo);

                    // Cria o array de bytes e especifica o preenchimento OAEP
                    DadosCifrados = RSA.Encrypt(byteCifrado, isOAEP);
                }
                return DadosCifrados;
            }
            catch (CryptographicException e)
            {
                throw new Exception(e.Message);
            }
        }

        static public byte[] RSADecifra(byte[] byteDecifrado, RSAParameters RSAInfo, bool isOAEP)
        {
            try
            {
                byte[] DadosDecifrados;

                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    // Importa a informação da chave RSA
                    // Isso é preciso para incluir a informação da chave privada.
                    RSA.ImportParameters(RSAInfo);

                    // Decifra o array de bytes e especifica o preenchimento OAEP.
                    DadosDecifrados = RSA.Decrypt(byteDecifrado, isOAEP);
                }
                return DadosDecifrados;
            }
            catch (CryptographicException e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
