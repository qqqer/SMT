using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace SMT
{
    public class SocketHelper
    {

        private Socket _socketClient;

        private string IP;
        private int Port;


        public bool Connect(string remote_ip, int remote_port)
        {
            IP = remote_ip;
            Port = remote_port;
            if (_socketClient != null)
            {
                //已连接
                if (_socketClient.Connected) { return true; }
            }

            try
            {
                //创建连接
                IPEndPoint serverRemote = new IPEndPoint(IPAddress.Parse(remote_ip), remote_port);
                _socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socketClient.Connect(serverRemote);
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }


        public int Send(byte[] data)
        {
            try
            {
                if (_socketClient != null && _socketClient.Connected)
                {
                    return _socketClient.Send(data, data.Length, 0);

                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }


        public int Receive(byte[] buffer)
        {
            try
            {
                if (_socketClient != null && _socketClient.Connected)
                {
                     return _socketClient.Receive(buffer);
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }


        public void Close()
        {
            try
            {
                _socketClient.Shutdown(SocketShutdown.Both);
            }
            finally
            {
                _socketClient.Close();
            }

        }
    }
}
