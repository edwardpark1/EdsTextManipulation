using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskRomanLine : TextTask
    {
        public string Separator { get; }
        public bool IsLowerCase { get; }

        public TextTaskRomanLine(bool isLowerCase, string separator, string name = "Roman Line")
           : base(name)
        {
            Separator = separator;
            IsLowerCase = isLowerCase;
        }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.LineNumberingRoman(Separator, IsLowerCase);
        }

        public override string GetTaskInfo() => base.GetTaskInfo() +
                $"\r\nSeparator: {GetSeparatorText(Separator)}\r\nLowercase?: {IsLowerCase}";
    }
}
