using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sherpany_UWP_Code_Challange.Interfaces;

namespace Sherpany_UWP_Code_Challange.Services
{
    public class KeyManager: IKeyManager
    {
        public string GetEncryptionKey(bool isDemoMode)
        {
            throw new NotImplementedException();
        }

        public void SetEncryptionKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool DeleteEncryptionKey()
        {
            throw new NotImplementedException();
        }

        public bool IsKeySet()
        {
            throw new NotImplementedException();
        }
    }
}
