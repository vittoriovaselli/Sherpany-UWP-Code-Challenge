using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherpany_UWP_Code_Challange.Model
{
    public class ConfirmActionParameters
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string CancelButtonText { get; set; }
        public string ActionButtonText { get; set; }
        public bool RedCancelButton { get; set; }
    }
}
