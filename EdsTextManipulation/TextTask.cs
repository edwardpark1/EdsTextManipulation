using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public abstract class TextTask
    {
        public string Name { get; }
        
        public string TaskInfo
        {
            get => GetTaskInfo();
        }

        public abstract void ExecuteTask(PreviewText prevText);

        public TextTask(string name = "TextTask")
        {
            Name = name;
        }

        public virtual string GetTaskInfo() => $"Task: {Name}";

        protected string GetSeparatorText(string separators)
        {
            var query = separators.ToCharArray()
                .Select(y => new { Word = Text.GetSpecialName(y) })
                .Select(z => $"[{z.Word}]");

            return string.Join("", query);
        }
    }
}