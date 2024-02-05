namespace Anjo.Android.YouTubePlayerX.Player
{
    /// <summary>
    /// Extend this class if you want to implement only some of the methods of <seealso cref="IYouTubePlayerListener"/>
    /// </summary>
    public abstract class AbstractYouTubePlayerListener : IYouTubePlayerListener
    {
        public void OnReady()
        {

        }

        public void OnStateChange(int state)
        {

        }

        public void OnPlaybackQualityChange(string playbackQuality)
        {

        }

        public void OnPlaybackRateChange(string playbackRate)
        {

        }

        public void OnError(int error)
        {

        }

        public void OnApiChange()
        {

        }

        public void OnCurrentSecond(int second)
        {

        }

        public void OnVideoDuration(int duration)
        {

        }

        public void OnVideoLoadedFraction(int loadedFraction)
        {

        }

        public void OnVideoId(string videoId)
        {

        }
    }

}