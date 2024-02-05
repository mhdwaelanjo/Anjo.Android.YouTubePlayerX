using Android.Content;
using Android.Net;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using AndroidX.Lifecycle;
using Anjo.Android.YouTubePlayerX.Player.playerUtils;
using Anjo.Android.YouTubePlayerX.Ui;
using Anjo.Android.YouTubePlayerX.Util;
using static Android.Net.ConnectivityManager;

namespace Anjo.Android.YouTubePlayerX.Player
{
    public class YouTubePlayerView : FrameLayout, ILifecycleObserver
    { 
        private WebViewYouTubePlayer YouTubePlayer;
        public DefaultPlayerUiController DefaultPlayerUiController; 

        private PlaybackResumer PlaybackResumer;  
        private FullScreenHelper FullScreenHelper; 
        private ICallable AsyncInitialization;
         
        protected YouTubePlayerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public YouTubePlayerView(Context context) : base(context)
        {
            Init(context);
        }

        public YouTubePlayerView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }

        public YouTubePlayerView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context);
        }

        public YouTubePlayerView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(context);
        }

        private void Init(Context context)
        {
            try
            {
                YouTubePlayer = new WebViewYouTubePlayer(context);
                AddView(YouTubePlayer, new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));

                DefaultPlayerUiController = new DefaultPlayerUiController(this, YouTubePlayer);

                PlaybackResumer = new PlaybackResumer();
                FullScreenHelper = new FullScreenHelper();

                FullScreenHelper.AddFullScreenListener(DefaultPlayerUiController);
                AddYouTubePlayerListeners(YouTubePlayer);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            try
            {
                // if height == wrap content make the view 16:9
                if (LayoutParameters.Height == ViewGroup.LayoutParams.WrapContent)
                {
                    int sixteenNineHeight = MeasureSpec.MakeMeasureSpec(MeasureSpec.GetSize(widthMeasureSpec) * 9 / 16, MeasureSpecMode.Exactly);
                    base.OnMeasure(widthMeasureSpec, sixteenNineHeight);
                }
                else
                {
                    base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        /// <summary>
        /// Initialize the player </summary>
        /// <param name="youTubePlayerInitListener"> lister for player init events </param>
        /// <param name="handleNetworkEvents"> if <b>true</b> a broadcast receiver will be registered.<br/>If <b>false</b> you should handle network events with your own broadcast receiver. See <seealso cref="YouTubePlayerView.OnNetworkAvailable()"/> and <seealso cref="YouTubePlayerView.OnNetworkUnavailable()"/> </param>
        public void Initialize(IYouTubePlayerInitListener youTubePlayerInitListener)
        {
            try
            {
                AsyncInitialization = new CallableAnonymousInnerClass(this, youTubePlayerInitListener);
                AsyncInitialization.Call();
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            } 
        }
         
        private class MyConnectivityManagerNetworkCallback : NetworkCallback
        {
            private readonly YouTubePlayerView PlayerView;

            public MyConnectivityManagerNetworkCallback(YouTubePlayerView view)
            {
                PlayerView = view;
            }

            public override void OnAvailable(Network network)
            {
                try
                {
                    base.OnAvailable(network);
                    PlayerView.Context.SendBroadcast(GetConnectivityIntent(false));
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                }
            }

            public override void OnLost(Network network)
            {
                try
                {
                    base.OnLost(network);
                    PlayerView.Context.SendBroadcast(GetConnectivityIntent(true));
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                }

            }

            private Intent GetConnectivityIntent(bool noConnection)
            {
                try
                {
                    Intent intent = new Intent();

                    intent.SetAction(PlayerView.Context.PackageName + ".CONNECTIVITY_CHANGE");
                    intent.PutExtra(ExtraNoConnectivity, noConnection);

                    return intent;
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                    return null;
                }

            }
        }

        private class CallableAnonymousInnerClass : ICallable
        {
            private readonly YouTubePlayerView OuterInstance;

            private readonly IYouTubePlayerInitListener YouTubePlayerInitListener;

            public CallableAnonymousInnerClass(YouTubePlayerView outerInstance, IYouTubePlayerInitListener youTubePlayerInitListener)
            {
                try
                {
                    OuterInstance = outerInstance;
                    YouTubePlayerInitListener = youTubePlayerInitListener;
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                }

            }

            public void Call()
            {
                try
                {
                    OuterInstance.YouTubePlayer.Initialize(new YouTubePlayerInitListenerAnonymousInnerClass(this));
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                }

            }

            private class YouTubePlayerInitListenerAnonymousInnerClass : IYouTubePlayerInitListener
            {
                private readonly CallableAnonymousInnerClass OuterInstance;

                public YouTubePlayerInitListenerAnonymousInnerClass(CallableAnonymousInnerClass outerInstance)
                {
                    OuterInstance = outerInstance;
                }

                public void OnInitSuccess(IYouTubePlayer youTubePlayer)
                {
                    try
                    {
                        OuterInstance.YouTubePlayerInitListener.OnInitSuccess(youTubePlayer);
                    }
                    catch (Exception e)
                    {
                        Utils.DisplayReportResultTrack(e);
                    }

                }
            }
        }

        /// <summary>
        /// Calls <seealso cref="WebView"/> on the player. And unregisters the broadcast receiver (for network events), if registered.
        /// Call this method before destroying the host Fragment/Activity, or register this View as an observer of its host lifcecycle
        /// </summary>
        [Lifecycle.Event.OnDestroy]
        public void Release()
        { 
            try
            {
                YouTubePlayer.Destroy();
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

       // [Lifecycle.Event.OnStop]
        internal void OnStop()
        {
            try
            {
                YouTubePlayer.Pause();
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void OnNetworkAvailable()
        {
            try
            {
                if (AsyncInitialization != null)
                {
                    AsyncInitialization.Call();
                }
                else
                {
                    PlaybackResumer.Resume(YouTubePlayer);
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void OnNetworkUnavailable()
        {
        }


        public IPlayerUiController PlayerUiController
        {
            get
            {
                if (DefaultPlayerUiController == null)
                {
                    throw new Exception("You have inflated a custom player UI. You must manage it with your own controller.");
                }

                return DefaultPlayerUiController;
            }
        }

        /// <summary>
        /// Replaces the default UI of the player with a custom UI.<br/>
        /// You will have to control the custom UI in your application,
        /// the default controller obtained through <seealso cref="get_PlayerUiController"/> won't be available anymore. </summary>
        /// <param name="customPlayerUiLayoutId"> the ID of the layout defining the custom UI. </param>
        /// <returns> The inflated View </returns>

        public View InflateCustomPlayerUi(int customPlayerUiLayoutId)
        {
            try
            {
                RemoveViews(1, ChildCount - 1);

                if (DefaultPlayerUiController != null)
                {
                    YouTubePlayer.RemoveListener(DefaultPlayerUiController);
                    FullScreenHelper.RemoveFullScreenListener(DefaultPlayerUiController);
                }

                DefaultPlayerUiController = null;

                return Inflate(Context, customPlayerUiLayoutId, this);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return null;
            }

        }

        public void EnterFullScreen()
        {
            try
            {
                FullScreenHelper.EnterFullScreen(this);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void ExitFullScreen()
        {
            try
            {
                FullScreenHelper.ExitFullScreen(this);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public bool FullScreen => FullScreenHelper.FullScreen;

        public void ToggleFullScreen()
        {
            try
            {
                FullScreenHelper.ToggleFullScreen(this);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }


        public bool AddFullScreenListener(IYouTubePlayerFullScreenListener fullScreenListener)
        {
            try
            {
                return FullScreenHelper.AddFullScreenListener(fullScreenListener);
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
                return FullScreenHelper.RemoveFullScreenListener(fullScreenListener);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return false;
            }

        }


        private void AddYouTubePlayerListeners(IYouTubePlayer youTubePlayer)
        {
            try
            {
                if (DefaultPlayerUiController != null)
                {
                    youTubePlayer.AddListener(DefaultPlayerUiController);
                }

                youTubePlayer.AddListener(PlaybackResumer);
                youTubePlayer.AddListener(new AbstractYouTubePlayerListenerAnonymousInnerClass(this));
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        private class AbstractYouTubePlayerListenerAnonymousInnerClass : AbstractYouTubePlayerListener
        {
            private readonly YouTubePlayerView OuterInstance;

            public AbstractYouTubePlayerListenerAnonymousInnerClass(YouTubePlayerView outerInstance)
            {
                OuterInstance = outerInstance;
            }

            public new void OnReady()
            {
                try
                {
                    OuterInstance.AsyncInitialization = null;
                    //youTubePlayer.removeListener(this);
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                }

            }
        }

    }

}