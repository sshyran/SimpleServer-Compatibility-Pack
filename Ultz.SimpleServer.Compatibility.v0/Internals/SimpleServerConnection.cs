using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Ultz.SimpleServer.Internals;

namespace SimpleServer.Internals
{
    public class SimpleServerConnection : IDisposable, IConnection
    {
        internal IConnection _client;
        internal SimpleServerConnection(IConnection connection)
        {
            _client = connection;
            Stream = connection.Stream;
            Id = connection.Id;
        }

        public bool Connected => _client.Connected;
        EndPoint IConnection.LocalEndPoint => LocalEndPoint;

        EndPoint IConnection.RemoteEndPoint => RemoteEndPoint;

        public int Id { get; }
        public IPEndPoint LocalEndPoint => (IPEndPoint) _client.LocalEndPoint;
        public IPEndPoint RemoteEndPoint => (IPEndPoint) _client.RemoteEndPoint;
        public void Close()
        {
            Dispose();
        }

        public Stream Stream { get; internal set; }

        public void Dispose()
        {
            _client.Close();
        }

        public TcpClient AsTcpClient()
        {
            return (TcpClient)typeof(TcpConnection).GetProperty("Base")?.GetValue((_client as TcpConnection));
        }

        public Socket AsSocket()
        {
            return AsTcpClient().Client;
        }
    }
}