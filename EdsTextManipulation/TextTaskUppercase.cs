using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskUppercase : TextTask
    {
        public TextTaskUppercase(string name = "Uppercase")
            : base(name) { }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.RenderToUpper();
        }
    }
}
