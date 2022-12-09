using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskRightTrim : TextTask
    {
        public char TrimChar { get; }

        public TextTaskRightTrim(char trimChar, string name = "Right Trim")
            : base(name)
        {
            TrimChar = trimChar;
        }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.TrimRight(TrimChar);
        }

        public override string GetTaskInfo() => base.GetTaskInfo() + 
            $"\r\nTrim char: {TrimChar}";
    }
}
