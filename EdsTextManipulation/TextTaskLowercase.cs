using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskLowercase : TextTask
    {
        public TextTaskLowercase(string name = "Lowercase")
            : base( name) { }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.RenderToLower();
        }
    }
}
