using Sherpany_UWP_Code_Challange.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Sherpany_UWP_Code_Challange.Services
{
    class ValuesCacheService
    {
        private readonly string fileName = "values_cache_file";

        private StorageFolder _localFolder;

        private UwpEncryptionManager _encryptionManager;

        public ValuesCacheService()
        {
            _localFolder = ApplicationData.Current.LocalFolder;
            _encryptionManager = new UwpEncryptionManager(new KeyManager());
        }

        public async Task<IEnumerable<SherpanyValueModel>> GetData()
        {
            //get data from local storage         
            var file = await _localFolder.GetFileAsync(fileName);

            var encryptedData = await ReadBytesFromFile(file);
            //decrypt
            var serializedData = await _encryptionManager.DecryptV2(encryptedData, true);
            //deserialize
            var values = Deserialize(serializedData);
            //return data
            return values;
        }

        public async void SetData(IEnumerable<SherpanyValueModel> values)
        {
            //serialize
            var serializedData = Serialize(values);
            //encrypt
            var encryptedData = await _encryptionManager.EncryptV2(serializedData, true);
            //save
            var file = await _localFolder.TryGetItemAsync(fileName) as StorageFile;

            if (file == null)
            {
                file = await _localFolder.CreateFileAsync(fileName);
            }

            await WriteBytesToFile(encryptedData, file);       
        }

        public async Task<bool> IsListSaved()
        {
            var file = await _localFolder.TryGetItemAsync(fileName);

            return file != null;
        }

        private static byte[] Serialize(IEnumerable<SherpanyValueModel> values)
        {
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream= new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, values);
                return memoryStream.ToArray();
            }
        }

        private IEnumerable<SherpanyValueModel> Deserialize(byte[] serializedValues)
        {
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(serializedValues, 0, serializedValues.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var values = (IEnumerable<SherpanyValueModel>)binaryFormatter.Deserialize(memoryStream);
                return values;
            }
        }

        private async Task<byte[]> ReadBytesFromFile(StorageFile file)
        {
            using (Stream stream = await file.OpenStreamForReadAsync())
            {
                using (var memoryStream = new MemoryStream())
                {

                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        private async Task WriteBytesToFile(byte[] encryptedData, StorageFile file)
        {
            using (Stream stream =  await file.OpenStreamForWriteAsync())
            {
                using( var memoryStream = new MemoryStream())
                {
                    memoryStream.Write(encryptedData, 0, encryptedData.Length);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(stream);
                }
            }
        }
    }
}
