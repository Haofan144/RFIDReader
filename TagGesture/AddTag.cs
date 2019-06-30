using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagGesture
{
    class AddTag
    {

        private int IncomingTagNumber;
        public int Count
        {
            get
            {
                return IncomingTagNumber;
            }
            set
            {
                if (value < 10)//当值发生改变时
                {
                    // IncomingTagNumber = value;
                    WhenMyValueChange();
                }
                IncomingTagNumber = value;
            }
        }


        //private void WhenMyValueChange()
        //{
        //    Console.WriteLine("hello");
        //}

        //定义一个委托
        public delegate void MyValueChanged();
        //与委托相关联的事件
        public event MyValueChanged OnMyValueChanged;

        public void Test()
        {
            //IncomingTagNumber = 0;
            OnMyValueChanged += new MyValueChanged(DoSomgThing);
        }

        void DoSomgThing()
        {
            //do
            Console.WriteLine("tag state changes");
        }

        private void WhenMyValueChange()
        {
            if (OnMyValueChanged != null)
            {
                OnMyValueChanged();
            }
        }




        //public void WhenMyValueChange()
        //{
        //    if (OnMyValueChanged != null)

        //    {

        //        OnMyValueChanged(this, null);

        //    }
        //}

        //public void afterMyValueChange()
        //{
        //    //dosomething
        //    Console.WriteLine("hello");
        //}

        ////定义一个委托
        //public delegate void MyValueChanged(object sender, EventArgs e);
        ////与委托相关联的事件
        //public event MyValueChanged OnMyValueChanged;

        ////将afterMyValueChanged的委托绑定到事件上
        //public void Test()
        //{
        //    OnMyValueChanged += new MyValueChanged(afterMyValueChanged);
        //}





        //public int IncomingTagNumber;
        //public List<int> TagList;
        ////public  List<int> TagList = new List<int>();
        //public int threshold; 

        ////Initialization
        //public AddTag(int IncomingTagNumber, int threshold,Monitor monitor)
        //{
        //    this.IncomingTagNumber = IncomingTagNumber;
        //    //this.TagList = TagList;
        //    this.threshold = threshold;

        //}

        //public delegate void ValueChange();
        //public event ValueChange WhenValueChange;

        //public void ChangeDetection()
        //{
        //    if (IncomingTagNumber <= threshold)
        //    {
        //        Console.WriteLine("State Change");
        //    }

        //}

    }
}
