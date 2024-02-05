namespace Anjo.Android.YouTubePlayerX.Player
{
    public interface IYouTubePlayerListener
    {
        /// <summary>
        /// Called when the player is ready to play videos. You should start interacting with the player only after it is ready.
        /// </summary>
        void OnReady();

        /// <summary>
        /// Use this method to track the state of the playback. Check <seealso cref="PlayerConstants.PlayerState"/> to see all the possible states. </summary>
        /// <param name="state"> a state from <seealso cref="PlayerConstants.PlayerState"/> </param>
        void OnStateChange(int state);

        /// <summary>
        /// Use this method to be notified when the quality of the playback changes. Check <seealso cref="PlayerConstants.PlaybackQuality"/> to see all the possible values. </summary>
        /// <param name="playbackQuality"> a state from <seealso cref="PlayerConstants.PlaybackQuality"/> </param>
        void OnPlaybackQualityChange(string playbackQuality);

        /// <summary>
        /// Use this method to be notified when the speed of the playback changes. Check <seealso cref="PlayerConstants.PlaybackRate"/> to see all the possible values. </summary>
        /// <param name="playbackRate"> a state from <seealso cref="PlayerConstants.PlaybackRate"/> </param>
        void OnPlaybackRateChange(string playbackRate);

        /// <summary>
        /// Use this method to be notified when an error occurs in the player. Check <seealso cref="PlayerConstants.PlayerError"/> to see all the possible values. </summary>
        /// <param name="error"> a state from <seealso cref="PlayerConstants.PlayerError"/> </param>
        void OnError(int error);

        void OnApiChange();

        /// <summary>
        /// This methods is called periodically by the player, the argument is the number of seconds that have been played. </summary>
        /// <param name="second"> current second of the playback </param>
        void OnCurrentSecond(int second);

        /// <summary>
        /// Use this method to know the duration in seconds of the currently playing video. <br/><br/>
        /// Note that getDuration() will return 0 until the video's metadata is loaded, which normally happens just after the video starts playing. </summary>
        /// <param name="duration"> total duration of the video </param>
        void OnVideoDuration(int duration);

        /// <summary>
        /// This methods is called periodically by the player, the argument is the percentage of the video that has been buffered. </summary>
        /// <param name="loadedFraction"> a number between 0 and 1 that represents the percentage of the video that has been buffered. </param>
        void OnVideoLoadedFraction(int loadedFraction);

        /// <summary>
        /// Use this method to know the id of the video being played. </summary>
        /// <param name="videoId"> the id of the video being played </param>
        void OnVideoId(string videoId);
    }

}