using Android.Content;

namespace Anjo.Android.YouTubePlayerX.Util
{
    public class NetworkReceiver : BroadcastReceiver
    {

        public interface INetworkListener
        {
            void OnNetworkAvailable();
            void OnNetworkUnavailable();
        }

        private readonly INetworkListener NetworkListener;

        public NetworkReceiver(INetworkListener networkListener)
        {
            NetworkListener = networkListener;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (Utils.isOnline(context))
            {
                NetworkListener.OnNetworkAvailable();
            }
            else
            {
                NetworkListener.OnNetworkUnavailable();
            }
        }
    }

}