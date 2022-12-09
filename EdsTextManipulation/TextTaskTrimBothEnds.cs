using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskTrimBothEnds : TextTask
    {
        public char TrimChar { get; }

        public TextTaskTrimBothEnds(char trimChar, string name = "Trim Both Ends")
            : base(name)
        {
            TrimChar = trimChar;
        }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.TrimBothSides(TrimChar);
        }

        public override string GetTaskInfo() => base.GetTaskInfo() + 
            $"\r\nTrim char: {Text.GetSpecialName(TrimChar)}";
    }
}
