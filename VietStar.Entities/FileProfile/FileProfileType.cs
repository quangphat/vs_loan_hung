using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.FileProfile
{
    public class FileProfileType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IsRequire { get; set; }
        public int ProfileId { get; set; }
        public int ProfileTypeId { get; set; }
        public int FileKey { get; set; }
        public string FilePath { get; set; }
        public string Folder { get; set; }
        public string FileName { get; set; }
    }
}
