using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskLeftTrim : TextTask
    {
        public char TrimChar { get; }

        public TextTaskLeftTrim(char trimChar, string name = "Left Trim")
            : base(name) 
        {
            TrimChar = trimChar;
        }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.TrimLeft(TrimChar);
        }

        public override string GetTaskInfo() => base.GetTaskInfo() + 
            $"\r\nTrim Char: {Text.GetSpecialName(TrimChar)}";
    }
}
