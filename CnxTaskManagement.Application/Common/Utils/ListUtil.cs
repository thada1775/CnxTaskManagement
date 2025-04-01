using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Application.Common.Utils
{
    public static class ListUtil
    {
        public static bool IsEmptyList<T>(ICollection<T> lst) => lst == null || lst.Count() == 0;
    }
}
