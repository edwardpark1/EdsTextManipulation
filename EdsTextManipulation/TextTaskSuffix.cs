using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskSuffix : TextTask
    {
        public string Suffix { get; }

        public TextTaskSuffix(string suffix, string name = "Suffix")
            : base(name)
        {
            Suffix = suffix;
        }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.PrefixSuffixText(Suffix, prevText.SuffixText);
        }

        public override string GetTaskInfo() => base.GetTaskInfo() + 
            $"\r\nSuffix: {Suffix}";
    }
}
