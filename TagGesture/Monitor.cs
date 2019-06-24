using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagGesture
{
    class Monitor
    {
        public void ChangeDetected()
        {
            Console.WriteLine("a tag changed");
        }
       
    }
}
