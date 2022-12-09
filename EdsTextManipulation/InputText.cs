using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class InputText : Text
    {
        public PreviewText Render;

        public InputText(string input)
            :base(input)
        {
            ResetPreviewText(input);
        }

        public void ResetPreviewText(string input)
        {
            Render = new PreviewText(input);
        }
    }
}
