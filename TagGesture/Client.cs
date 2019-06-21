using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TagGesture
{
    class Client
    {
        public string txtstr;
        public string serverstr;
        Socket socket;
        private string hostIp = "127.0.0.1";
        private int portId = 1234;
        const int buff_size = 1024;
        public byte[] readbuff = new byte[buff_size];
        public int num;

        public void Start()
        {
            Connection();
        }

        //update data in real time 
        public void Update()
        {
            //txtstr.text = serverstr;
            txtstr = serverstr;
        }


        public void Connection()
        {
            //txtstr.text = "";
            txtstr = "";

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //string host = hostInput.text;
            string host = hostIp;

            //int port = int.Parse(portInput.text);
            int port = portId;

            socket.Connect(host, port);

            Console.WriteLine("client address" + socket.LocalEndPoint);

            //connecttext.text = socket.LocalEndPoint.ToString();

            socket.BeginReceive(readbuff, 0, buff_size, SocketFlags.None, ReceiveCb, null);

            //Send();


        }

        private void ReceiveCb(IAsyncResult ar)
        {
            try
            {
                int count = socket.EndReceive(ar);

                string str = System.Text.Encoding.Default.GetString(readbuff, 0, count);

                if (serverstr.Length > 300) serverstr = "";
                serverstr += str + "\n";

                socket.BeginReceive(readbuff, 0, buff_size, SocketFlags.None, ReceiveCb, null);


            }
            catch (Exception e)
            {
                //txtstr.text = "connect is broken" + e.Message;
                txtstr = "connect is broken" + e.Message;

                socket.Close();
            }
        }


        public void ReadInput(int num)
        {
            this.num = num;
        }
        public void Send()
        {
            //string str = TextInput.text;
            string str = " I'm a reader"+num;

            byte[] bytes = System.Text.Encoding.Default.GetBytes(str);


            try
            {
                socket.Send(bytes);
            }
            catch
            {

            }
        }





    }
}
