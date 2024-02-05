using System.Text;
using Android.Annotation;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Webkit;
using Anjo.Android.YouTubePlayerX.Util;
using Object = Java.Lang.Object;

namespace Anjo.Android.YouTubePlayerX.Player
{
    /// <summary>
    /// WebView implementing the actual YouTube Player
    /// </summary>
    internal class WebViewYouTubePlayer : WebView, IYouTubePlayer, YouTubePlayerBridge.IYouTubePlayerBridgeCallbacks, IValueCallback
    {
        private ISet<IYouTubePlayerListener> YouTubePlayerListeners;
        private Handler MainThreadHandler;
        private IYouTubePlayerInitListener YouTubePlayerInitListener;

        protected WebViewYouTubePlayer(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public WebViewYouTubePlayer(Context context) : base(context)
        {
            Init();
        }

        public WebViewYouTubePlayer(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public WebViewYouTubePlayer(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        [Obsolete]
        public WebViewYouTubePlayer(Context context, IAttributeSet attrs, int defStyleAttr, bool privateBrowsing) : base(context, attrs, defStyleAttr, privateBrowsing)
        {
            Init();
        }

        public WebViewYouTubePlayer(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        private void Init()
        {
            try
            {
                MainThreadHandler = new Handler(Looper.MainLooper);
                YouTubePlayerListeners = new HashSet<IYouTubePlayerListener>();
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        protected internal void Initialize(IYouTubePlayerInitListener initListener)
        {
            try
            {
                YouTubePlayerInitListener = initListener;

                InitWebView();
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        public void OnYouTubeIframeApiReady()
        {
            try
            {
                YouTubePlayerInitListener.OnInitSuccess(this);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        public void LoadVideo(string videoId, double startSeconds)
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    if (Build.VERSION.SdkInt >= (BuildVersionCodes)19)
                    {
                        EvaluateJavascript("javascript:loadVideo('" + videoId + "', " + startSeconds + ");", this);
                    }
                    else
                    {
                        LoadUrl("javascript:loadVideo('" + videoId + "', " + startSeconds + ");");
                    } 
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            } 
        }


        public void CueVideo(string videoId, double startSeconds)
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    if (Build.VERSION.SdkInt >= (BuildVersionCodes)19)
                    {
                        EvaluateJavascript("javascript:cueVideo('" + videoId + "', " + startSeconds + ");", this);
                    }
                    else
                    {
                        LoadUrl("javascript:cueVideo('" + videoId + "', " + startSeconds + ");");
                    } 
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void Play()
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    if (Build.VERSION.SdkInt >= (BuildVersionCodes)19)
                    {
                        EvaluateJavascript("javascript:playVideo();", this);
                    }
                    else
                    {
                        LoadUrl("javascript:playVideo();");
                    } 
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void Pause()
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    if (Build.VERSION.SdkInt >= (BuildVersionCodes)19)
                    {
                        EvaluateJavascript("javascript:pauseVideo();", this);
                    }
                    else
                    {
                        LoadUrl("javascript:pauseVideo();");
                    } 
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            } 
        }
         
        public void Stop()
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    if (Build.VERSION.SdkInt >= (BuildVersionCodes)19)
                    {
                        EvaluateJavascript("javascript:stopVideo();", this);
                    }
                    else
                    {
                        LoadUrl("javascript:stopVideo();");
                    } 
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            } 
        }

        public void Mute()
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    if (Build.VERSION.SdkInt >= (BuildVersionCodes)19)
                    {
                        EvaluateJavascript("javascript:mute();", this);
                    }
                    else
                    {
                        LoadUrl("javascript:mute();");
                    }
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            } 
        }

        public void UnMute()
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    if (Build.VERSION.SdkInt >= (BuildVersionCodes)19)
                    {
                        EvaluateJavascript("javascript:unMute();", this);
                    }
                    else
                    {
                        LoadUrl("javascript:unMute();");
                    }
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            } 
        }

        public int Volume
        {
            set
            {
                try
                {
                    if (value < 0 || value > 100)
                    {
                        throw new ArgumentException("Volume must be between 0 and 100");
                    }
                     
                    MainThreadHandler.Post(() =>
                    {
                        if (Build.VERSION.SdkInt >= (BuildVersionCodes)19)
                        {
                            EvaluateJavascript("javascript:setVolume(" + value + ");", this);
                        }
                        else
                        {
                            LoadUrl("javascript:setVolume(" + value + ");");
                        } 
                    });
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                }
            }
        }
          
        public void SeekTo(double time)
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    if (Build.VERSION.SdkInt >= (BuildVersionCodes)19)
                    {
                        EvaluateJavascript("javascript:seekTo(" + time + ");", this);
                    }
                    else
                    {
                        LoadUrl("javascript:seekTo(" + time + ");");
                    } 
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void SetLoop(bool loopPlaylists)
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    if (Build.VERSION.SdkInt >= (BuildVersionCodes)19)
                    {
                        EvaluateJavascript("javascript:setLoop(" + loopPlaylists + ");", this);
                    }
                    else
                    {
                        LoadUrl("javascript:setLoop(" + loopPlaylists + ");");
                    } 
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            } 
        }

        public void SetShuffle(bool shufflePlaylist)
        {
            try
            {
                MainThreadHandler.Post(() =>
                {
                    if (Build.VERSION.SdkInt >= (BuildVersionCodes)19)
                    {
                        EvaluateJavascript("javascript:setShuffle(" + shufflePlaylist + ");", this);
                    }
                    else
                    {
                        LoadUrl("javascript:setShuffle(" + shufflePlaylist + ");");
                    } 
                });
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            } 
        }

        public override void Destroy()
        {
            try
            {
                YouTubePlayerListeners.Clear();
                MainThreadHandler.RemoveCallbacksAndMessages(null);

                MainThreadHandler.Post(() =>
                {
                    if (Build.VERSION.SdkInt >= (BuildVersionCodes)19)
                    {
                        EvaluateJavascript("javascript:onDestroy();", this);
                    }
                    else
                    {
                        LoadUrl("javascript:onDestroy();");
                    }
                });

                base.Destroy();

            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            } 
        }


        public ICollection<IYouTubePlayerListener> Listeners => YouTubePlayerListeners;


        public bool AddListener(IYouTubePlayerListener listener)
        {
            try
            {
                if (listener == null)
                {
                    Log.Error("YouTubePlayer", "null YouTubePlayerListener not allowed.");
                    return false;
                }

                return YouTubePlayerListeners.Add(listener);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return false;
            }
        }


        public bool RemoveListener(IYouTubePlayerListener listener)
        {
            try
            {
                return YouTubePlayerListeners.Remove(listener);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return false;
            }
        }

        [SuppressLint(Value = new []{ "SetJavaScriptEnabled" })]
        private void InitWebView()
        {
            try
            {
                WebSettings settings = Settings;
                settings.JavaScriptEnabled = true;
                settings.CacheMode = CacheModes.NoCache;
                settings.MediaPlaybackRequiresUserGesture = false;
                 
                settings.LoadsImagesAutomatically = true;
                
                settings.JavaScriptCanOpenWindowsAutomatically = true;
                settings.SetLayoutAlgorithm(WebSettings.LayoutAlgorithm.TextAutosizing);
                settings.DomStorageEnabled = true;
                settings.AllowFileAccess = true;
                settings.DefaultTextEncodingName = "utf-8";

                settings.UseWideViewPort = true;
                settings.LoadWithOverviewMode = true;

                settings.SetSupportZoom(false);
                settings.BuiltInZoomControls = false;
                settings.DisplayZoomControls = false;
                 
                AddJavascriptInterface(new YouTubePlayerBridge(this), "YouTubePlayerBridge");
                LoadDataWithBaseURL("https://www.youtube.com", ReadYouTubePlayerHtmlFromFile(), "text/html", "utf-8", null);

                // if the video's thumbnail is not in memory, show a black screen
                SetWebChromeClient(new WebChromeClientAnonymousInnerClass(this)); 
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }
 
        private class WebChromeClientAnonymousInnerClass : WebChromeClient
        {
            private readonly WebViewYouTubePlayer OuterInstance;

            public WebChromeClientAnonymousInnerClass(WebViewYouTubePlayer outerInstance)
            {
                OuterInstance = outerInstance;
            }
          
            public override Bitmap DefaultVideoPoster
            {
                get
                {
                    try
                    {
                        Bitmap result = base.DefaultVideoPoster;

                        if (result == null)
                        {
                            Bitmap icon = Bitmap.CreateBitmap(50, 50, Bitmap.Config.Argb8888);
                            return icon; 
                        }

                        return result;
                    }
                    catch (Exception e)
                    {
                        Utils.DisplayReportResultTrack(e);
                        return null;
                    }
                }
            } 
        }

        private string ReadYouTubePlayerHtmlFromFile()
        {
            try
            {
                Stream inputStream = Resources.OpenRawResource(Resource.Raw.youtube_player);

                StreamReader inputStreamReader = new StreamReader(inputStream, Encoding.UTF8);

                StreamReader bufferedReader = new StreamReader(inputStreamReader.BaseStream);

                string read;
                StringBuilder sb = new StringBuilder();

                while (!ReferenceEquals(read = bufferedReader.ReadLine(), null))
                {
                    sb.Append(read).Append("\n");
                }

                inputStream.Close();

                return sb.ToString();
            }
            catch (Exception)
            {
                throw new Exception("Can't parse HTML file containing the player.");
            }
        }

        public void OnReceiveValue(Object value)
        {
             
        }
    }
}