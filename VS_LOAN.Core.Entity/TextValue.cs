using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    [Serializable]
    public class TextValue
    {
        public TextValue(string value, string text)
        {
            Text = text;
            Value = value;
        }

        public string Text { get; set; }

        public string Value { get; set; }
    }
}
