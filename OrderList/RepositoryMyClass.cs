using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderList
{
    public class RepositoryMyClass
    {
        private IList<MyClass> _myClassList;
        public RepositoryMyClass()
        {
            _myClassList = new List<MyClass>();
            SetList();
        }
        public IList<MyClass> All()
        {
            var items = _myClassList.ToFullList();
            return items;
        }

        public IList<MyClass> AllToFit()
        {
            return All().ToFitList();
        }
        private void Updateindex()
        {
            All().Each((m, i) => m.Index = i);
        }
        public void Add(MyClass item)
        {
            _myClassList.Add(item);
            Updateindex();
        }
        public void Add(int position, MyClass item)
        {
            var getItem = ElementAt(position);
            item.Parent = getItem;
            getItem.Nesteds.Add(item);
            _myClassList.EachRecursive((m, i) => m.Index = i);
        }
        public void Remove(MyClass item)
        {
            All().Remove(item);
            Updateindex();
        }

        public void RemoveAll()
        {
            _myClassList = new List<MyClass>();
        }
        internal MyClass ElementAt(int position)
        {
            return All().ElementAt(position);
        }

        internal void Move(int oldindex, int newIndex, TypeMoviment typeMov)
        {
            if (oldindex == newIndex) return;
            var itemOnIndex = ElementAt(newIndex);
            var oldItem = ElementAt(oldindex);
            if (typeMov == TypeMoviment.Root)
            {
                if (oldItem.HasParent)
                {
                    oldItem.Parent.Nesteds.Remove(oldItem);
                    oldItem.Parent = null;
                }
                else
                {
                    _myClassList.Remove(oldItem);
                    _myClassList.Insert(itemOnIndex.Index, oldItem);
                }
            }
            if (typeMov == TypeMoviment.Nested)
            {
                if (!oldItem.HasParent)
                    _myClassList.Remove(oldItem);
                else
                    oldItem.Parent.Nesteds.Remove(oldItem);

                oldItem.Parent = itemOnIndex;
                itemOnIndex.Nesteds.Add(oldItem);
            }
            _myClassList.EachRecursive((m, i) => m.Index = i);
        }
        private void SetList()
        {
            var item0 = new MyClass
            {
                Index = 0,
                Name = "Item 00"
            };

            item0.Nesteds = new List<MyClass> {
                new MyClass { Index =0, Name = "Nested 00", Parent = item0},
                new MyClass { Index =1, Name = "Nested 01", Parent = item0},
                new MyClass { Index =2, Name = "Nested 02", Parent = item0},
            };
            var item1 = new MyClass
            {
                Index = 1,
                Name = "Item 01"
            };
            var item00 = new MyClass { Index = 0, Name = "Nested 00", Parent = item1 };

            item00.Nesteds = new List<MyClass> {
                new MyClass { Index =0, Name = "Second Nested 00", Parent = item00},
                new MyClass { Index =1, Name = "Second Nested 01", Parent = item00},
             };
            item1.Nesteds = new List<MyClass> {
                item00,
                new MyClass { Index =1, Name = "Nested 01", Parent = item1},
                new MyClass { Index =2, Name = "Nested 02", Parent = item1},
            };
            var item2 = new MyClass
            {
                Index = 2,
                Name = "Item 02"
            };
            item2.Nesteds = new List<MyClass> {
                new MyClass { Index =0, Name = "Nested 00", Parent = item2},
                new MyClass { Index =1, Name = "Nested 01", Parent = item2},
                new MyClass { Index =2, Name = "Nested 02", Parent = item2},
            };
            _myClassList.Add(item0);
            _myClassList.Add(item1);
            _myClassList.Add(item2);
        }

    }
}
