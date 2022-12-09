using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class TextTaskPascalcase : TextTask
    {
        public TextTaskPascalcase(string name = "Pascal casing")
            : base(name) { }

        public override void ExecuteTask(PreviewText prevText)
        {
            prevText.RenderPascalCase();
        }
    }
}
