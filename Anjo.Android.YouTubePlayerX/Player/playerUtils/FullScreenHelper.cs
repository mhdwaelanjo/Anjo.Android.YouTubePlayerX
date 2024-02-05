using Android.Views;
using Anjo.Android.YouTubePlayerX.Util;

namespace Anjo.Android.YouTubePlayerX.Player.playerUtils
{
    public class FullScreenHelper
    {

        private bool IsFullScreen;
        private readonly ISet<IYouTubePlayerFullScreenListener> FullScreenListeners;

        public FullScreenHelper()
        {
            try
            {
                IsFullScreen = false;
                FullScreenListeners = new HashSet<IYouTubePlayerFullScreenListener>();
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }
         
        public void EnterFullScreen(View view)
        {
            try
            {
                if (IsFullScreen)
                {
                    return;
                }

                //FrameLayout.LayoutParams viewParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                //viewParams.Width = ViewGroup.LayoutParams.MatchParent;
                //viewParams.Height = ViewGroup.LayoutParams.MatchParent;
                //view.LayoutParameters = viewParams;
                  
                IsFullScreen = true;

                foreach (IYouTubePlayerFullScreenListener fullScreenListener in FullScreenListeners)
                {
                    fullScreenListener.OnYouTubePlayerEnterFullScreen();
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }


        public void ExitFullScreen(View view)
        {
            try
            {
                if (!IsFullScreen)
                {
                    return;
                }

                //FrameLayout.LayoutParams viewParams = (FrameLayout.LayoutParams)view.LayoutParameters;
                //viewParams.Height = ViewGroup.LayoutParams.WrapContent;
                //viewParams.Width = ViewGroup.LayoutParams.MatchParent;
                //view.LayoutParameters = viewParams;

                IsFullScreen = false;

                foreach (IYouTubePlayerFullScreenListener fullScreenListener in FullScreenListeners)
                {
                    fullScreenListener.OnYouTubePlayerExitFullScreen();
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }  
        }
         
        public void ToggleFullScreen(View view)
        {
            try
            {
                if (IsFullScreen)
                {
                    ExitFullScreen(view);
                }
                else
                {
                    EnterFullScreen(view);
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public bool FullScreen => IsFullScreen;


        public bool AddFullScreenListener(IYouTubePlayerFullScreenListener fullScreenListener)
        {
            try
            {
                return FullScreenListeners.Add(fullScreenListener);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return false;
            }
        }


        public bool RemoveFullScreenListener(IYouTubePlayerFullScreenListener fullScreenListener)
        {
            try
            {
                return FullScreenListeners.Remove(fullScreenListener);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return false;
            }
        }
    }

}