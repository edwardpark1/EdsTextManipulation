using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskLetterLine : TextTask
    {
        public string Separator { get; }
        public bool IsLowerCase { get; }

        public TextTaskLetterLine(bool isLowerCase, string separator, string name = "Letter Line")
           : base(name)
        {
            Separator = separator;
            IsLowerCase = isLowerCase;
        }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.LineNumberingLetter(Separator, IsLowerCase);
        }

        public override string GetTaskInfo() => base.GetTaskInfo() + 
            $"\r\nSeparator: {GetSeparatorText(Separator)}\r\nLowercase?: {IsLowerCase}";
    }
}
