using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskRemoveEmptyLines : TextTask
    {
        public TextTaskRemoveEmptyLines(string name = "Remove Empty Lines")
        : base(name) { }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.RemoveEmptyLines();
        }
    }
}
