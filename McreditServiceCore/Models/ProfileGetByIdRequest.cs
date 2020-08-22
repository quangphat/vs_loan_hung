using System;
using System.Collections.Generic;
using System.Text;

namespace McreditServiceCore.Models
{
    public class ProfileGetByIdRequest : MCreditRequestModelBase
    {
        public string Id { get; set; }
    }
}
