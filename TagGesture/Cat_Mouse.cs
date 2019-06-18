using System;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Cat cat = new Cat("Blue", "Tom");
            Mouse mouse1 = new Mouse("Yellow", "Jerry");
            cat.catCome += mouse1.RunAway;//注册
            Mouse mouse2 = new Mouse("Yellow", "Micky");
            cat.catCome += mouse2.RunAway;//注册
            cat.CatComing();
            Console.ReadKey();
        }
    }



    class Cat
    {

        private string color;
        private string name;
        public void Cat(string color, string name)
        {
            this.name = name;
            this.color = color;
        }
        public void CatComing()
        {
            Console.WriteLine("cat" + name + "is coming");
            if (catCome != null)
                catCome();
        }
        public Action catCome;
    }

    class Mouse
    {
        private string name;
        private string color;
        public void Mouse(string color, string name)
        {
            this.name = name;
            this.color = color;
        }
        public void RunAway()
        {
            Console.WriteLine("mouse" + name + color);
        }

    }




}








