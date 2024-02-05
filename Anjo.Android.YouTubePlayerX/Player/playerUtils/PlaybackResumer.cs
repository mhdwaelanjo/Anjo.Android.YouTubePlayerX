using Android.Annotation;
using Anjo.Android.YouTubePlayerX.Util;

namespace Anjo.Android.YouTubePlayerX.Player.playerUtils
{
    /// <summary>
    /// Class responsible for resuming the playback state in case of network problems.
    /// eg: player is playing -> network goes out -> player stops -> network comes back -> player resumes playback automatically.
    /// </summary>
    public class PlaybackResumer : AbstractYouTubePlayerListener
    {

        private static readonly int NoError = int.MinValue;

        private bool IsPlaying = false;
        private int Error = NoError;

        private string CurrentVideoId;
        public int CurrentSecond { private set; get; }

        public void Resume(IYouTubePlayer youTubePlayer)
        {
            try
            {
                if (IsPlaying && Error == PlayerConstants.PlayerError.Html5Player)
                {
                    youTubePlayer.LoadVideo(CurrentVideoId, CurrentSecond);
                }
                else if (!IsPlaying && Error == PlayerConstants.PlayerError.Html5Player)
                {
                    youTubePlayer.CueVideo(CurrentVideoId, CurrentSecond);
                }

                Error = NoError;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        [SuppressLint(Value = new []{ "SwitchIntDef" })]
        public new void OnStateChange(int state)
        {
            try
            {
                if (state == PlayerConstants.PlayerState.Ended)
                {
                    IsPlaying = false;
                }
                else if (state == PlayerConstants.PlayerState.Paused)
                {
                    IsPlaying = false;
                }
                else if (state == PlayerConstants.PlayerState.Playing)
                {
                    IsPlaying = true;
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        public new void OnError(int error)
        {
            try
            {

                if (error == PlayerConstants.PlayerError.Html5Player)
                {
                    Error = error;
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        public new void OnCurrentSecond(int second)
        {
            try
            { 
                CurrentSecond = second;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        public new void OnVideoId(string videoId)
        {
            try
            {

                CurrentVideoId = videoId;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }
    }
}