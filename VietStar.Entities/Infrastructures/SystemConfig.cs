using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Infrastructures
{
    public class SystemConfig
    {
        public int ImportMaxRow { get; set; } = 400;
        public string ExportFolder { get; set; } = "wwwroot\\Export";
    }
}
