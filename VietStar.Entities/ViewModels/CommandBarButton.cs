using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.ViewModels
{
    public class CommandBarButton
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
    }
    public class CommandBar
    {
        public List<CommandBarButton> Buttons { get; set; }
    }
}
