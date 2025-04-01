using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Application.Common.BaseClass
{
    public class BaseDto
    {
        public DtoStatus DtoStatus { get; set; }
        public string? RequestTimeZoneId { get; set; }
    }

    public enum DtoStatus
    {
        None = 0,
        Add = 1,
        Update = 2,
        Delete =3,
    }
}
