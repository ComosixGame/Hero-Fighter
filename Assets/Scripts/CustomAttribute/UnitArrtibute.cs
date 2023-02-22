using UnityEngine;

namespace MyCustomAttribute
{
    //custom atrr read only (sử dụng để ẩn chỉnh sửa trên inspector)
    //cách sử dụng [ReadOnly]
    public class ReadOnlyAttribute : PropertyAttribute { }

    //custom attr lable (chỉnh sửa tiêu đề của field)
    //cách sử dụng [Label("tiêu đề")]
    public class LabelAttribute : PropertyAttribute
    {
        public readonly string Label;
        public LabelAttribute(string label)
        {
            Label = label;
        }
    }
}
