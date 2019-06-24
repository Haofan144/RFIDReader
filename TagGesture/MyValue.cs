//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TagGesture
//{
//    class MyValue
//    {
//        private int myValue = 0;
//        public int count = 0;
//        public int Value
//        {
//            get
//            {
//                return myValue;
//            }
//            set
//            {
//                if (value != myValue)
//                {
//                    //intrigger event
//                    WhenMyValueChange();
//                }
//                myValue = value;
//            }
//        }
//        //private void WhenMyValueChange()
//        //{
//        //    Console.WriteLine("Change detected");
//        //}

//        public delegate void MyValueChanged(int Object);
//        public event MyValueChanged OnMyValueChanged;

//        public void Test()
//        {
//            count = 0;
//            OnMyValueChanged += new MyValueChanged(Dosomething);
//        }

//        void Dosomething(int num)
//        {
//            Console.WriteLine("I get" + num);
//        }

//        private void WhenMyValueChange()
//        {
//            if (OnMyValueChanged != null)
//            {
//                OnMyValueChanged(count);
//            }
//        }

//    }
//}
