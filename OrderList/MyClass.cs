using System.Collections.Generic;

namespace OrderList
{
    public class MyClass
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public MyClass Parent { get; set; }
        public virtual int ParentCount { get { return GetParentCount(); } }
        public virtual bool HasParent { get { return Parent != null; } }

        public IList<MyClass> Nesteds { get; set; }
        public MyClass()
        {
            Nesteds = new List<MyClass>();
        }
        private int GetParentCount()
        {
            var count = 0;
            GetParentCount(this, ref count);
            return count;
        }

        private void GetParentCount(MyClass question, ref int count)
        {
            if (question == null || !question.HasParent) return;
            count++;
            GetParentCount(question.Parent, ref count);
        }
    }

    public enum TypeMoviment
    {
        Root,
        Nested
    }
}
