using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskPrefix : TextTask
    {
        public string Prefix { get; }

        public TextTaskPrefix(string prefix, string name = "Prefix")
            : base(name) 
        {
            Prefix = prefix;
        }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.PrefixSuffixText(Prefix, prevText.PrefixText);
        }

        public override string GetTaskInfo() => base.GetTaskInfo() + 
            $"\r\nPrefix: {Prefix}";
    }
}
