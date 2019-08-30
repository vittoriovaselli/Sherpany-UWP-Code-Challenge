using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sherpany_UWP_Code_Challange.Interfaces;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Sherpany_UWP_Code_Challange.Services
{
    public class KeyManager: IKeyManager
    {
        private readonly string _pinKey = "pin";

        private ApplicationDataContainer _localSettings;

        public KeyManager()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
        }

        public string GetEncryptionKey(bool isDemoMode)
        {
            return (string)_localSettings.Values[_pinKey];
        }

        public void SetEncryptionKey(string key)
        {
            //hash pin
            string hashedPin = Hash(key);

            _localSettings.Values.Add(_pinKey, hashedPin);
        }

        public bool DeleteEncryptionKey()
        {
            return _localSettings.Values.Remove(_pinKey);
        }

        public bool IsKeySet()
        {
            return _localSettings.Values.ContainsKey(_pinKey);            
        }

        private string Hash(string message)
        {
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(message, BinaryStringEncoding.Utf8);

            HashAlgorithmProvider algoritmProvider = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);

            IBuffer bufferHash = algoritmProvider.HashData(buffer);

            String stringHashBase64 = CryptographicBuffer.EncodeToBase64String(bufferHash);

            return stringHashBase64;
        }
    }
}
