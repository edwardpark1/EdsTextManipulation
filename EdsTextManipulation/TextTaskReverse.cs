using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskReverse : TextTask
    {
        public TextTaskReverse(string name = "Reverse")
        : base(name) { }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.RenderReverse();
        }
    }
}
