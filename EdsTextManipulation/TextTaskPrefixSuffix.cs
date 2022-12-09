using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskPrefixSuffix : TextTask
    {
        public string InsertText { get; }

        public TextTaskPrefixSuffix(string insertText, string name = "Prefix&Suffix")
            : base(name)
        {
            InsertText = insertText;
        }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.PrefixSuffixText(InsertText, prevText.InsertTextBothSides);
        }

        public override string GetTaskInfo() => base.GetTaskInfo() + 
            $"\r\nInsert text: {InsertText}";
    }
}
