using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
public static class CustomSocketFactory
{
    public static System.Net.Sockets.TcpClient GetSocket(System.Net.Sockets.AddressFamily addressFamily)
    {
        System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient(System.Net.Sockets.AddressFamily.InterNetwork);
        tcpClient.NoDelay = true;
        return tcpClient;
    }
}
 