using System.Net;

namespace SimpleServer.Internals
{
    public class SimpleServerEndpoint
    {
        public IPAddress Scope { get; set; }
        public int Port { get; set; }

        #region Equality Operators

        public bool Equals(SimpleServerEndpoint other)
        {
            return Equals(Scope, other.Scope) && Port == other.Port;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Scope != null ? Scope.GetHashCode() : 0) * 397) ^ Port;
            }
        }

        public static bool operator ==(SimpleServerEndpoint endpoint1, SimpleServerEndpoint endpoint2)
        {
            return endpoint1 != null && endpoint1.Equals(endpoint2);
        }

        public static bool operator !=(SimpleServerEndpoint endpoint1, SimpleServerEndpoint endpoint2)
        {
            return !(endpoint1 == endpoint2);
        }

        #endregion
    }
}