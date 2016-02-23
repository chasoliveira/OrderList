using System;
using System.Linq;

namespace OrderList
{
    public class Program
    {
        private static RepositoryMyClass _repositoryMyClass;
        static void Main(string[] args)
        {
            _repositoryMyClass = new RepositoryMyClass();

            Option();
            Console.WriteLine("The system was finished, press any key!");
            Console.ReadKey();
        }

        private static void Option()
        {
            Console.WriteLine("Choose an Option");
            Console.WriteLine("\t1 = Print List");
            Console.WriteLine("\t2 = Add");
            Console.WriteLine("\t3 = Add at position");
            Console.WriteLine("\t4 = Add nested");
            Console.WriteLine("\t5 = Move");
            Console.WriteLine("\t6 = Remove Item");
            Console.WriteLine("\t7 = Remove All");
            Console.WriteLine("\t8 = Exit");

            var choice = 1;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("The value must be between 1 and 8!");
                NewOption();
            }
            Console.Clear();
            switch (choice)
            {
                case 1: PrintMyClass(); break;
                case 2: AddNewMyClass(); break;
                case 3: AddNewQuestionAtPosition(); break;
                case 5: MoveItem(); break;
                case 4: AddNested(); break;
                case 6: RemoveItem(); break;
                case 7: _repositoryMyClass.RemoveAll(); break;
                case 8: return;
                default:
                    break;
            }
            NewOption();
        }
        private static void NewOption()
        {
            Console.WriteLine("...Press any key to continue!");
            Console.ReadKey();
            Console.Clear();
            Option();
        }
        private static string GetName()
        {
            Console.Write("Type a name: ");
            return Console.ReadLine();
        }
        private static int GetPosition()
        {
            Console.Write("Type a position: ");
            var position = 0;
            int.TryParse(Console.ReadLine(), out position);
            return position;
        }
        private static void PrintMyClass()
        {
            Console.WriteLine("===== List as Full =====\r\n");
            var myClassList = _repositoryMyClass.All();
            myClassList.Each((q, i) =>
            {
                q.Index = i;
                Console.WriteLine(string.Format("Real Index = {0:00},{1} Internal Index= {2}, Name = {3}", i, WriteSpaces(q.ParentCount), q.Index, q.Name));
            });
            Console.WriteLine("\r\n===== List as Fit =====\r\n");
            var myClassListFit = _repositoryMyClass.AllToFit();
            myClassListFit.EachRecursive((q, i) =>
            {
                q.Index = i;
                Console.WriteLine(string.Format("Real Index = {0:00},{1} Internal Index= {2}, Name = {3}", i, WriteSpaces(q.ParentCount), q.Index, q.Name));
            });
        }
        private static void AddNewMyClass()
        {
            var name = GetName();
            AddNewMyClass(name);
        }
        private static void AddNewMyClass(string name)
        {
            _repositoryMyClass.Add(new MyClass
            {
                Index = _repositoryMyClass.All().Count,
                Name = name,
            });
        }
        private static void AddNewMyClass(string name, int position)
        {
            if (position > 0 && position < _repositoryMyClass.All().Count)
            {
                _repositoryMyClass.Add(position, new MyClass { Index = position, Name = name });
            }
        }
        private static void AddNewQuestionAtPosition()
        {
            var name = GetName();
            var position = GetPosition();
            AddNewMyClass(name, position);
        }
        private static void AddNested()
        {
            var position = GetPosition();
            var name = GetName();
            var newItem = new MyClass { Name = name };
            _repositoryMyClass.Add(position, newItem);
        }
        private static void RemoveItem() { }
        private static string WriteSpaces(int count)
        {
            var spaces = "";
            for (int i = 0; i < count; i++)
            {
                spaces += "\t";
            }
            return spaces;
        }

        private static void MoveItem()
        {
            Console.Write("Type the origin index: ");
            var oldindex = 0;
            int.TryParse(Console.ReadLine(), out oldindex);
            Console.Write("Type the destination index: ");
            var newIndex = 0;
            int.TryParse(Console.ReadLine(), out newIndex);
            Console.Write("Wich a type of moviment: 1 = root, 2 = nested: ");
            var type = 0;
            int.TryParse(Console.ReadLine(), out type);
            TypeMoviment typeMvt = type == 1 ? TypeMoviment.Root : TypeMoviment.Nested;
            _repositoryMyClass.Move(oldindex, newIndex, typeMvt);
            PrintMyClass();
        }
    }
}
