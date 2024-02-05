using Android.OS;
using Android.Text;
using Android.Webkit;
using Anjo.Android.YouTubePlayerX.Util;
using Java.Interop;

namespace Anjo.Android.YouTubePlayerX.Player
{
    /// <summary>
    /// Bridge used to communicate from Javascript to Java.
    /// </summary>
    public class YouTubePlayerBridge : Java.Lang.Object
    {

        // these constant values correspond to the values in the Javascript player

        private const string StateUnstarted = "UNSTARTED";
        private const string StateEnded = "ENDED";
        private const string StatePlaying = "PLAYING";
        private const string StatePaused = "PAUSED";
        private const string StateBuffering = "BUFFERING";
        private const string StateCued = "CUED";

        private const string QualitySmall = "small";
        private const string QualityMedium = "medium";
        private const string QualityLarge = "large";
        private const string QualityHd720 = "hd720";
        private const string QualityHd1080 = "hd1080";
        private const string QualityHighRes = "highres";
        private const string QualityDefault = "default";

        private const string Rate025 = "0.25";
        private const string Rate05 = "0.5";
        private const string Rate1 = "1";
        private const string Rate15 = "1.5";
        private const string Rate2 = "2";

        private const string ErrorInvalidParameterInRequest = "2";
        private const string ErrorHtml5Player = "5";
        private const string ErrorVideoNotFound = "100";
        private const string ErrorVideoNotPlayableInEmbeddedPlayer1 = "101";
        private const string ErrorVideoNotPlayableInEmbeddedPlayer2 = "150";


        private readonly IYouTubePlayerBridgeCallbacks YouTubePlayer;

        private readonly Handler MainThreadHandler;

        public interface IYouTubePlayerBridgeCallbacks
        {
            void OnYouTubeIframeApiReady();
            ICollection<IYouTubePlayerListener> Listeners { get; }
        }


        public YouTubePlayerBridge(IYouTubePlayerBridgeCallbacks youTubePlayer)
        {
            try
            {
                YouTubePlayer = youTubePlayer;
                MainThreadHandler = new Handler(Looper.MainLooper);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        [JavascriptInterface, Export("SendYouTubeIframeApiReady")]
        public void SendYouTubeIframeApiReady()
        {
            try
            {
                YouTubePlayer.OnYouTubeIframeApiReady();
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        [JavascriptInterface, Export("SendReady")]
        public void SendReady()
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    foreach (IYouTubePlayerListener listener in YouTubePlayer.Listeners)
                    {
                        listener.OnReady();
                    }
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        [JavascriptInterface, Export("SendStateChange")]
        public void SendStateChange(string state)
        {
            try
            {
                int playerState = ParsePlayerState(state);

                MainThreadHandler.Post(() =>
                {
                    foreach (IYouTubePlayerListener listener in YouTubePlayer.Listeners)
                    {
                        listener.OnStateChange(playerState);
                    }
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        [JavascriptInterface, Export("SendPlaybackQualityChange")]
        public void SendPlaybackQualityChange(string quality)
        {
            try
            {
                string playbackQuality = ParsePlaybackQuality(quality);

                MainThreadHandler.Post(() =>
                {
                    foreach (IYouTubePlayerListener listener in YouTubePlayer.Listeners)
                    {
                        listener.OnPlaybackQualityChange(playbackQuality);
                    }
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        [JavascriptInterface, Export("SendPlaybackRateChange")]
        public void SendPlaybackRateChange(string rate)
        {

            try
            {
                string playbackRate = ParsePlaybackRate(rate);

                MainThreadHandler.Post(() =>
                {
                    foreach (IYouTubePlayerListener listener in YouTubePlayer.Listeners)
                    {
                        listener.OnPlaybackRateChange(playbackRate);
                    }
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        [JavascriptInterface, Export("SendError")]
        public void SendError(string error)
        {
            try
            {
                int playerError = ParsePlayerError(error);

                MainThreadHandler.Post(() =>
                {
                    foreach (IYouTubePlayerListener listener in YouTubePlayer.Listeners)
                    {
                        listener.OnError(playerError);
                    }
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        [JavascriptInterface, Export("SendApiChange")]
        public void SendApiChange()
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    foreach (IYouTubePlayerListener listener in YouTubePlayer.Listeners)
                    {
                        listener.OnApiChange();
                    }
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        [JavascriptInterface, Export("SendVideoCurrentTime")]
        public void SendVideoCurrentTime(string seconds)
        {
            try
            {
                int currentTimeSeconds;
                try
                {
                    bool success = int.TryParse(seconds, out var number);
                    if (success)
                    {
                        currentTimeSeconds = number;
                    }
                    else
                    {
                        seconds = seconds?.Split('.')?.FirstOrDefault() ?? "0";
                        currentTimeSeconds = Convert.ToInt32(seconds);
                    } 
                }
                catch (FormatException e)
                {
                    Utils.DisplayReportResultTrack(e);
                    return;
                }

                MainThreadHandler.Post(() =>
                {
                    foreach (IYouTubePlayerListener listener in YouTubePlayer.Listeners)
                    {
                        listener.OnCurrentSecond(currentTimeSeconds);
                    }
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }


        [JavascriptInterface, Export("SendVideoDuration")]
        public void SendVideoDuration(string seconds)
        {
            try
            {
                int videoDuration;
                try
                {
                    string finalSeconds = TextUtils.IsEmpty(seconds) ? "0" : seconds;

                    bool success = int.TryParse(finalSeconds, out var number);
                    if (success)
                    {
                        videoDuration = number;
                    }
                    else
                    {
                        finalSeconds = finalSeconds?.Split('.')?.FirstOrDefault() ?? "0";
                        videoDuration = Convert.ToInt32(finalSeconds);
                    } 
                }
                catch (FormatException e)
                {
                    Utils.DisplayReportResultTrack(e);
                    return;
                }

                MainThreadHandler.Post(() =>
                {
                    foreach (IYouTubePlayerListener listener in YouTubePlayer.Listeners)
                    {
                        listener.OnVideoDuration(videoDuration);
                    }
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        [JavascriptInterface, Export("SendVideoLoadedFraction")]
        public void SendVideoLoadedFraction(string fraction)
        {
            try
            {
                int loadedFraction;
                try
                {
                    bool success = int.TryParse(fraction, out var number);
                    if (success)
                    {
                        loadedFraction = number;
                    }
                    else
                    {
                        fraction = fraction?.Split('.')?.FirstOrDefault() ?? "0";
                        loadedFraction = Convert.ToInt32(fraction);
                    } 
                }
                catch (FormatException e)
                {
                    Utils.DisplayReportResultTrack(e); 
                    return;
                }

                MainThreadHandler.Post(() =>
                {
                    foreach (IYouTubePlayerListener listener in YouTubePlayer.Listeners)
                    {
                        listener.OnVideoLoadedFraction(loadedFraction);
                    }
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }


        [JavascriptInterface, Export("SendVideoId")]
        public void SendVideoId(string videoId)
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    foreach (IYouTubePlayerListener listener in YouTubePlayer.Listeners)
                    {
                        listener.OnVideoId(videoId);
                    }
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }


        private int ParsePlayerState(string state)
        {
            try
            {
                int playerState;

                if (state.Equals(StateUnstarted, StringComparison.OrdinalIgnoreCase))
                {
                    playerState = PlayerConstants.PlayerState.Unstarted;
                }
                else if (state.Equals(StateEnded, StringComparison.OrdinalIgnoreCase))
                {
                    playerState = PlayerConstants.PlayerState.Ended;
                }
                else if (state.Equals(StatePlaying, StringComparison.OrdinalIgnoreCase))
                {
                    playerState = PlayerConstants.PlayerState.Playing;
                }
                else if (state.Equals(StatePaused, StringComparison.OrdinalIgnoreCase))
                {
                    playerState = PlayerConstants.PlayerState.Paused;
                }
                else if (state.Equals(StateBuffering, StringComparison.OrdinalIgnoreCase))
                {
                    playerState = PlayerConstants.PlayerState.Buffering;
                }
                else if (state.Equals(StateCued, StringComparison.OrdinalIgnoreCase))
                {
                    playerState = PlayerConstants.PlayerState.VideoCued;
                }
                else
                {
                    playerState = PlayerConstants.PlayerState.Unknown;
                }

                return playerState;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return 0;
            }
        }



        private string ParsePlaybackQuality(string quality)
        {
            try
            {
                string playbackQuality;

                if (quality.Equals(QualitySmall, StringComparison.OrdinalIgnoreCase))
                {
                    playbackQuality = PlayerConstants.PlaybackQuality.Small;
                }
                else if (quality.Equals(QualityMedium, StringComparison.OrdinalIgnoreCase))
                {
                    playbackQuality = PlayerConstants.PlaybackQuality.Medium;
                }
                else if (quality.Equals(QualityLarge, StringComparison.OrdinalIgnoreCase))
                {
                    playbackQuality = PlayerConstants.PlaybackQuality.Large;
                }
                else if (quality.Equals(QualityHd720, StringComparison.OrdinalIgnoreCase))
                {
                    playbackQuality = PlayerConstants.PlaybackQuality.Hd720;
                }
                else if (quality.Equals(QualityHd1080, StringComparison.OrdinalIgnoreCase))
                {
                    playbackQuality = PlayerConstants.PlaybackQuality.Hd1080;
                }
                else if (quality.Equals(QualityHighRes, StringComparison.OrdinalIgnoreCase))
                {
                    playbackQuality = PlayerConstants.PlaybackQuality.HighRes;
                }
                else if (quality.Equals(QualityDefault, StringComparison.OrdinalIgnoreCase))
                {
                    playbackQuality = PlayerConstants.PlaybackQuality.Default;
                }
                else
                {
                    playbackQuality = PlayerConstants.PlaybackQuality.Unknown;
                }

                return playbackQuality;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return null;
            }
        }


        private string ParsePlaybackRate(string rate)
        {
            try
            {
                string playbackRate;

                if (rate.Equals(Rate025, StringComparison.OrdinalIgnoreCase))
                {
                    playbackRate = PlayerConstants.PlaybackRate.Rate025;
                }
                else if (rate.Equals(Rate05, StringComparison.OrdinalIgnoreCase))
                {
                    playbackRate = PlayerConstants.PlaybackRate.Rate05;
                }
                else if (rate.Equals(Rate1, StringComparison.OrdinalIgnoreCase))
                {
                    playbackRate = PlayerConstants.PlaybackRate.Rate1;
                }
                else if (rate.Equals(Rate15, StringComparison.OrdinalIgnoreCase))
                {
                    playbackRate = PlayerConstants.PlaybackRate.Rate15;
                }
                else if (rate.Equals(Rate2, StringComparison.OrdinalIgnoreCase))
                {
                    playbackRate = PlayerConstants.PlaybackRate.Rate2;
                }
                else
                {
                    playbackRate = PlayerConstants.PlaybackRate.Unknown;
                }

                return playbackRate;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return null;
            }
        }
         
        private int ParsePlayerError(string error)
        { 
            try
            {

                if (error == "2") Console.WriteLine("[LOG] Error: The request contains an invalid parameter value.");
                if (error == "5") Console.WriteLine("[LOG] Error: content cannot be played in an HTML5 player.");
                if (error == "100") Console.WriteLine("[LOG] Error: Video doesn't exists (or has been removed)!");
                if (error == "101") Console.WriteLine("[LOG] Error: Not allowed to be played in an embedded player.");
                if (error == "150") Console.WriteLine("[LOG] Error: Not allowed to be played in an embedded player.");

                int playerError;
                if (error.Equals(ErrorInvalidParameterInRequest, StringComparison.OrdinalIgnoreCase))
                {
                    playerError = PlayerConstants.PlayerError.InvalidParameterInRequest;
                }
                else if (error.Equals(ErrorHtml5Player, StringComparison.OrdinalIgnoreCase))
                {
                    playerError = PlayerConstants.PlayerError.Html5Player;
                }
                else if (error.Equals(ErrorVideoNotFound, StringComparison.OrdinalIgnoreCase))
                {
                    playerError = PlayerConstants.PlayerError.VideoNotFound;
                }
                else if (error.Equals(ErrorVideoNotPlayableInEmbeddedPlayer1, StringComparison.OrdinalIgnoreCase))
                {
                    playerError = PlayerConstants.PlayerError.VideoNotPlayableInEmbeddedPlayer;
                }
                else if (error.Equals(ErrorVideoNotPlayableInEmbeddedPlayer2, StringComparison.OrdinalIgnoreCase))
                {
                    playerError = PlayerConstants.PlayerError.VideoNotPlayableInEmbeddedPlayer;
                }
                else
                {
                    playerError = PlayerConstants.PlayerError.Unknown;
                }

                return playerError;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return 0;
            }

        }
    }

}