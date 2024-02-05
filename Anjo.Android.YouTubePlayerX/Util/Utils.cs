using System.Runtime.CompilerServices;
using Android.Content;
using Android.Net;
using Android.OS;
using Exception = System.Exception;
using Trace = System.Diagnostics.Trace;

namespace Anjo.Android.YouTubePlayerX.Util
{
    public static class Utils
    {
        internal static void DisplayReportResultTrack(Exception exception, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                Trace.WriteLine("ReportMode YouTubePlayerAndroidX >> message: " + exception.Message + " \n  " + exception.StackTrace);
                Trace.WriteLine("ReportMode YouTubePlayerAndroidX >> member name: " + memberName);
                Trace.WriteLine("ReportMode YouTubePlayerAndroidX >> source file path: " + sourceFilePath);
                Trace.WriteLine("ReportMode YouTubePlayerAndroidX >> source line number: " + sourceLineNumber);
            }
            catch (Exception xx)
            {
                Console.WriteLine(xx);
            }
        }
         
        public static string FormatTime(int sec)
        {
            try
            {
                int minutes = sec / 60;
                int seconds = sec % 60;
                return $"{minutes:D}:{seconds:D2}";
            }
            catch (Exception e)
            {
                DisplayReportResultTrack(e);
                return string.Empty;
            }
        }

        public static bool isOnline(Context context)
        {
            try
            {
                ConnectivityManager cm = (ConnectivityManager) context.GetSystemService(Context.ConnectivityService);
                switch ((int) Build.VERSION.SdkInt)
                {
                    case <= 25:
                    {
#pragma warning disable 618
                        var activeNetwork = cm?.ActiveNetworkInfo;
#pragma warning restore 618
                        if (activeNetwork != null)
                        {
#pragma warning disable 618
                            bool isOnline = activeNetwork.IsConnected;
#pragma warning restore 618
                            return isOnline;
                        }

                        break;
                    }
                    default:
                    {
                        NetworkCapabilities capabilities = cm.GetNetworkCapabilities(cm.ActiveNetwork);
                        if (capabilities != null)
                        {
                            if (capabilities.HasTransport(TransportType.Cellular) ||
                                capabilities.HasTransport(TransportType.Wifi) ||
                                capabilities.HasTransport(TransportType.Ethernet) ||
                                capabilities.HasTransport(TransportType.Vpn))
                                return true;
                        }

                        break;
                    }
                }

                return false;
            }
            catch (Exception exception)
            {
                DisplayReportResultTrack(exception);
                return false;
            }
        } 
    } 
}