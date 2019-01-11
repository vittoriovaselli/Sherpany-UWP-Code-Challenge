using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherpany_UWP_Code_Challange.Messages
{
    public class DialogCloseMessage
    {
        public DialogCloseMessage(bool? result)
        {
            Result = result;
        }

        public bool? Result { get; }
    }
}
