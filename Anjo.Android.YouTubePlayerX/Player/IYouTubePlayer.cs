namespace Anjo.Android.YouTubePlayerX.Player
{
    public interface IYouTubePlayer
    {
        /// <summary>
        /// Loads and automatically plays the specified video. </summary>
        /// <param name="videoId"> id of the video </param>
        /// <param name="startSeconds"> the time from which the video should start playing </param>
        public void LoadVideo(string videoId, double startSeconds);

        /// <summary>
        /// Loads the specified video's thumbnail and prepares the player to play the video. Does not automatically play the video. </summary>
        /// <param name="videoId"> id of the video </param>
        /// <param name="startSeconds"> the time from which the video should start playing </param>
        public void CueVideo(string videoId, double startSeconds);

        public void Play();
        public void Pause(); 
        public void Stop(); 
         
        /// <summary>
        /// Integer between 0 and 100
        /// </summary>
        public int Volume { set; }
         
        public void Mute();
        public void UnMute();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"> The absolute time in seconds to seek to </param>  
        public void SeekTo(double time);

        /// <summary>
        /// This function indicates whether the video player should continuously play a playlist or if it should stop playing after the last video in the playlist ends. The default behavior is that playlists do not loop
        /// This setting will persist even if you load or cue a different playlist, which means that if you load a playlist, call the setLoop function with a value of true, and then load a second playlist, the second playlist will also loop
        /// </summary>
        /// <param name="loopPlaylists">If the parameter value is true, then the video player will continuously play playlists. After playing the last video in a playlist, the video player will go back to the beginning of the playlist and play it again.
        /// If the parameter value is false, then playbacks will end after the video player plays the last video in a playlist.</param>
        public void SetLoop(bool loopPlaylists);

        /// <summary>
        /// This function indicates whether a playlist's videos should be shuffled so that they play back in an order different from the one that the playlist creator designated. If you shuffle a playlist after it has already started playing, the list will be reordered while the video that is playing continues to play. The next video that plays will then be selected based on the reordered list.
        /// This setting will not persist if you load or cue a different playlist, which means that if you load a playlist, call the setShuffle function, and then load a second playlist, the second playlist will not be shuffled.
        /// </summary>
        /// <param name="shufflePlaylist">If the parameter value is true, then YouTube will shuffle the playlist order. If you instruct the function to shuffle a playlist that has already been shuffled, YouTube will shuffle the order again
        /// If the parameter value is false, then YouTube will change the playlist order back to its original order.</param>
        public void SetShuffle(bool shufflePlaylist);


        public bool AddListener(IYouTubePlayerListener listener);

        public bool RemoveListener(IYouTubePlayerListener listener);
    }

}