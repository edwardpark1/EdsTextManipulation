using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskReplace : TextTask
    {
        public string FindText { get; }
        public string ReplaceText { get; }
        public bool IgnoreCase { get; }

        public TextTaskReplace(string findText, string replaceText, 
            bool ignoreCase, string name = "Replace")
            : base(name) 
        { 
            FindText = findText;
            ReplaceText = replaceText;
            IgnoreCase = ignoreCase;
        }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.RenderReplaceText(FindText, ReplaceText, IgnoreCase);
        }

        public override string GetTaskInfo() => base.GetTaskInfo() + 
            $"\r\nFind: {FindText}\r\nReplace with: {ReplaceText}\r\nIgnoreCase?: {IgnoreCase}";
    }
}
