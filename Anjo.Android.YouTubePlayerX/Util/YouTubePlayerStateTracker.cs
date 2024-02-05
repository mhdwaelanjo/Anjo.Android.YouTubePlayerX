using Anjo.Android.YouTubePlayerX.Player;

namespace Anjo.Android.YouTubePlayerX.Util
{
    /// <summary>
    /// Utility class responsible for tracking the state of YouTubePlayer.
    /// This is a YouTubePlayerListener, therefore is responsibility of the user to add and remove it as a listener on the YouTubePlayer object.
    /// </summary>
    public class YouTubePlayerStateTracker : AbstractYouTubePlayerListener
    {

        private int currentState = PlayerConstants.PlayerState.Unknown;
        private int currentSecond;
        private int videoDuration;
        private string videoId;

        public new void OnStateChange(int state)
        {
            currentState = state;
        }

        public new void OnCurrentSecond(int second)
        {
            currentSecond = second;
        }

        public new void OnVideoDuration(int duration)
        {
            videoDuration = duration;
        }

        public new void OnVideoId(string videoId)
        {
            this.videoId = videoId;
        }

        /// <returns> the player state. A value from <seealso cref="PlayerConstants.PlayerState"/> </returns>
        public int CurrentState => currentState;

        public int CurrentSecond => currentSecond;

        public int VideoDuration => videoDuration;

        public string VideoId => videoId;
    }

}