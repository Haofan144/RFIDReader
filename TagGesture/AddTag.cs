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
        private int myValue = 0;
        public string inputstr;
        public string Output;
        public int MyValue
        {
            get { return myValue; }
            set
            {
                //if myValue<threshold
                if (myValue < 5)
                {
                    myValue = value;
                    Console.WriteLine(myValue);
                    //Trigger event 
                    Output = WhenMyValueChange();
                }


            }
        }


        //Define a delegate 
        public delegate string MyValueChanged();
        //Define an event 
        public event MyValueChanged OnMyValueChanged;

        string DoSomgThing()
        {
            //Console.WriteLine("tag state changes"+myValue);
            string output = "hello" + myValue;
            return output;
        }

        private string WhenMyValueChange()
        {
            //if (OnMyValueChanged != null)
            //{
            OnMyValueChanged += new MyValueChanged(DoSomgThing);
            string output = OnMyValueChanged();
            return output;
            // }
        }

    }
}
