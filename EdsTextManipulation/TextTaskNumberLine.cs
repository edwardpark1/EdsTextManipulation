using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskNumberLine : TextTask
    {
        public string Separator { get; }
        public bool StartAtZero { get; }

        public TextTaskNumberLine(bool startAtZero, string separator, string name = "Number Line")
           : base(name) 
        { 
            Separator = separator;
            StartAtZero = startAtZero;
        }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.LineNumberingNumbers(StartAtZero, Separator);
        }

        public override string GetTaskInfo() => base.GetTaskInfo() + 
            $"\r\nSeparator: {GetSeparatorText(Separator)}\r\nStartAtZero: {StartAtZero}";
    }
}
