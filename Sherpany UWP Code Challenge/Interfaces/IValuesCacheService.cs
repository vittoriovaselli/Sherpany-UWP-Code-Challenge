using Sherpany_UWP_Code_Challange.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherpany_UWP_Code_Challange.Interfaces
{
    public interface IValuesCacheService
    {
        Task<IEnumerable<SherpanyValueModel>> GetData();
        void SetData(IEnumerable<SherpanyValueModel> values);
        Task<bool> IsListStored();
    }
}
