using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using AndroidX.Core.Content;
using Anjo.Android.YouTubePlayerX.Player;
using Anjo.Android.YouTubePlayerX.Ui.menu;
using Anjo.Android.YouTubePlayerX.Ui.menu.defaultMenu;
using Anjo.Android.YouTubePlayerX.Util;
using Java.Lang;
using Exception = Java.Lang.Exception;
using Object = Java.Lang.Object;
using Uri = Android.Net.Uri;

namespace Anjo.Android.YouTubePlayerX.Ui
{
    public class DefaultPlayerUiController : Object, IPlayerUiController, IYouTubePlayerListener, IYouTubePlayerFullScreenListener, View.IOnClickListener, SeekBar.IOnSeekBarChangeListener
    {

        private readonly YouTubePlayerView YouTubePlayerView;

        private readonly IYouTubePlayer YouTubePlayer;


        private IYouTubePlayerMenu YouTubePlayerMenu;

        /// <summary>
        /// View used for for intercepting clicks and for drawing a black background.
        /// Could have used controlsRoot view, but in this way I'm able to hide all the control at once by hiding controlsRoot
        /// </summary>
        private View Panel;

        // view containing the controls
        private View ControlsRoot;

        private LinearLayout ExtraViewsContainer;

        private TextView videoTitle;
        private TextView VideoCurrentTime;
        private TextView VideoDuration;
        private TextView LiveVideoIndicator;

        private ProgressBar ProgressBar;
        private ImageView MenuButton;
        private ImageView PlayPauseButton;
        private ImageView YouTubeButton;
        private ImageView FullScreenButton;

        private ImageView CustomActionLeft;
        private ImageView CustomActionRight;
        private ImageView CustomActionLeft2;
        private ImageView CustomActionRight2;

        private SeekBar SeekBar;


        private View.IOnClickListener OnFullScreenButtonListener;

        private View.IOnClickListener OnMenuButtonClickListener;

        // view state
        public bool IsPlaying { private set; get; } = false;
        public bool IsVisible { private set; get; } = true;
        private bool CanFadeControls = false;
        private string VideoIdYoutube;
        private int CurrentSecond;


        private bool ShowUiRenamed = true;

        private bool ShowPlayPauseButtonRenamed = true;

        private bool ShowBufferingProgressRenamed = true;


        public DefaultPlayerUiController(YouTubePlayerView youTubePlayerView, IYouTubePlayer youTubePlayer)
        {
            try
            {
                YouTubePlayerView = youTubePlayerView;
                YouTubePlayer = youTubePlayer;

                View defaultPlayerUi = View.Inflate(youTubePlayerView.Context, Resource.Layout.default_player_ui, youTubePlayerView);
                InitViews(defaultPlayerUi);

                YouTubePlayerMenu = new DefaultYouTubePlayerMenu(youTubePlayerView.Context);
                FadeOutRunnable = new MyRunnable(this);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        private void InitViews(View controlsView)
        {
            try
            {
                Panel = controlsView.FindViewById<View>(Resource.Id.panel);

                ControlsRoot = controlsView.FindViewById<View>(Resource.Id.controls_root);
                ExtraViewsContainer = controlsView.FindViewById<LinearLayout>(Resource.Id.extra_views_container);

                videoTitle = controlsView.FindViewById<TextView>(Resource.Id.video_title);
                VideoCurrentTime = controlsView.FindViewById<TextView>(Resource.Id.video_current_time);
                VideoDuration = controlsView.FindViewById<TextView>(Resource.Id.video_duration);
                LiveVideoIndicator = controlsView.FindViewById<TextView>(Resource.Id.live_video_indicator);

                ProgressBar = controlsView.FindViewById<ProgressBar>(Resource.Id.progress);
                MenuButton = controlsView.FindViewById<ImageView>(Resource.Id.menu_button);
                PlayPauseButton = controlsView.FindViewById<ImageView>(Resource.Id.play_pause_button);
                YouTubeButton = controlsView.FindViewById<ImageView>(Resource.Id.youtube_button);
                FullScreenButton = controlsView.FindViewById<ImageView>(Resource.Id.fullscreen_button);

                CustomActionLeft = controlsView.FindViewById<ImageView>(Resource.Id.custom_action_left_button);
                CustomActionRight = controlsView.FindViewById<ImageView>(Resource.Id.custom_action_right_button);
                
                CustomActionLeft2 = controlsView.FindViewById<ImageView>(Resource.Id.custom_action_left_button2);
                CustomActionRight2 = controlsView.FindViewById<ImageView>(Resource.Id.custom_action_right_button2);

                SeekBar = controlsView.FindViewById<SeekBar>(Resource.Id.seek_bar);

                SeekBar.SetOnSeekBarChangeListener(this);
                Panel.SetOnClickListener(this);
                PlayPauseButton.SetOnClickListener(this);
                MenuButton.SetOnClickListener(this);
                FullScreenButton.SetOnClickListener(this);

                videoTitle.Visibility = ViewStates.Gone;
                YouTubeButton.Visibility = ViewStates.Gone;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void ShowVideoTitle(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Gone;
                videoTitle.Visibility = visibility;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }


        public string VideoTitle
        {
            set => videoTitle.Text = value;
        }

        public void ShowUi(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Invisible;
                ControlsRoot.Visibility = visibility;

                ShowUiRenamed = show;

            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void ShowPlayPauseButton(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Gone;
                PlayPauseButton.Visibility = visibility;

                ShowPlayPauseButtonRenamed = show;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void EnableLiveVideoUi(bool enable)
        {
            try
            {
                if (enable)
                {
                    VideoDuration.Visibility = ViewStates.Invisible;
                    SeekBar.Visibility = ViewStates.Invisible;
                    VideoCurrentTime.Visibility = ViewStates.Invisible;

                    LiveVideoIndicator.Visibility = ViewStates.Visible;
                }
                else
                {
                    VideoDuration.Visibility = ViewStates.Visible;
                    SeekBar.Visibility = ViewStates.Visible;
                    VideoCurrentTime.Visibility = ViewStates.Visible;

                    LiveVideoIndicator.Visibility = ViewStates.Gone;
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        /// <summary>
        /// Set custom action to the left of the Play/Pause button
        /// </summary>

        public void SetCustomActionLeft1(Drawable icon, View.IOnClickListener clickListener)
        {
            try
            {
                CustomActionLeft.SetImageDrawable(icon);
                CustomActionLeft.SetOnClickListener(clickListener);
                ShowCustomActionLeft1(clickListener != null);

            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }
        
        /// <summary>
        /// Set custom action to the left of the Play/Pause button
        /// </summary>

        public void SetCustomActionLeft2(Drawable icon, View.IOnClickListener clickListener)
        {
            try
            {
                CustomActionLeft2.SetImageDrawable(icon);
                CustomActionLeft2.SetOnClickListener(clickListener);
                ShowCustomActionLeft2(clickListener != null);

            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        /// <summary>
        /// Set custom action to the right of the Play/Pause button
        /// </summary>

        public void SetCustomActionRight1(Drawable icon, View.IOnClickListener clickListener)
        {
            try
            {
                CustomActionRight.SetImageDrawable(icon);
                CustomActionRight.SetOnClickListener(clickListener);
                ShowCustomActionRight1(clickListener != null);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }
        
        /// <summary>
        /// Set custom action to the right of the Play/Pause button
        /// </summary>

        public void SetCustomActionRight2(Drawable icon, View.IOnClickListener clickListener)
        {
            try
            {
                CustomActionRight2.SetImageDrawable(icon);
                CustomActionRight2.SetOnClickListener(clickListener);
                ShowCustomActionRight2(clickListener != null);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void ShowCustomActionLeft1(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Gone;
                CustomActionLeft.Visibility = visibility;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }
        
        public void ShowCustomActionLeft2(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Gone;
                CustomActionLeft2.Visibility = visibility;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void ShowCustomActionRight1(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Gone;
                CustomActionRight.Visibility = visibility;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }
        
        public void ShowCustomActionRight2(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Gone;
                CustomActionRight2.Visibility = visibility;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void ShowMenuButton(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Gone;
                MenuButton.Visibility = visibility;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }


        public View.IOnClickListener CustomMenuButtonClickListener
        {
            set => OnMenuButtonClickListener = value;
        }

        public void ShowCurrentTime(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Gone;
                VideoCurrentTime.Visibility = visibility;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void ShowDuration(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Gone;
                VideoDuration.Visibility = visibility;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void ShowSeekBar(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Invisible;
                SeekBar.Visibility = visibility;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void ShowBufferingProgress(bool show)
        {
            try
            {
                ShowBufferingProgressRenamed = show;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void ShowYouTubeButton(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Gone;
                YouTubeButton.Visibility = visibility;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }


        public void AddView(View view)
        {
            try
            {
                ExtraViewsContainer.AddView(view, 0);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }


        public void RemoveView(View view)
        {
            try
            {
                ExtraViewsContainer.RemoveView(view);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }


        public IYouTubePlayerMenu Menu
        {
            get => YouTubePlayerMenu;
            set => YouTubePlayerMenu = value;
        }


        public void ShowFullscreenButton(bool show)
        {
            try
            {
                var visibility = show ? ViewStates.Visible : ViewStates.Gone;
                FullScreenButton.Visibility = visibility;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }


        public View.IOnClickListener CustomFullScreenButtonClickListener
        {
            set => OnFullScreenButtonListener = value;
        }

        public void OnClick(View view)
        {
            try
            {
                if (view == Panel)
                {
                    ToggleControlsVisibility();
                }
                else if (view == PlayPauseButton)
                {
                    OnPlayButtonPressed();
                }
                else if (view == FullScreenButton)
                {
                    OnFullScreenButtonPressed(); 
                }
                else if (view == MenuButton)
                {
                    OnMenuButtonPressed();
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        private void OnMenuButtonPressed()
        {
            try
            {
                if (OnMenuButtonClickListener == null)
                {
                    YouTubePlayerMenu.Show(MenuButton);
                }
                else
                {
                    OnMenuButtonClickListener.OnClick(MenuButton);
                }

            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        private void OnFullScreenButtonPressed()
        {
            try
            {
                if (OnFullScreenButtonListener == null)
                {
                    YouTubePlayerView.ToggleFullScreen();
                }
                else
                {
                    OnFullScreenButtonListener.OnClick(FullScreenButton);
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        private void OnPlayButtonPressed()
        {
            try
            {
                if (IsPlaying)
                {
                    YouTubePlayer.Pause();
                }
                else
                {
                    YouTubePlayer.Play();
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        private void UpdatePlayPauseButtonIcon(bool playing)
        {
            try
            {
                int img = playing ? Resource.Drawable.ic_pause_36dp : Resource.Drawable.ic_play_36dp;
                PlayPauseButton.SetImageResource(img);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        private void ToggleControlsVisibility()
        {
            try
            {
                float finalAlpha = IsVisible ? 0f : 1f;
                FadeControls(finalAlpha);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        private readonly Handler Handler = new Handler(Looper.MainLooper);
        private readonly MyRunnable FadeOutRunnable;

        private class MyRunnable : Object, IRunnable
        {
            private readonly DefaultPlayerUiController Controller;

            public MyRunnable(DefaultPlayerUiController controller)
            {
                Controller = controller;
            }

            public void Run()
            {
                Controller.FadeControls(0f);
            }
        }

        public void FadeControls(float finalAlpha)
        {
            try
            {
                if (!CanFadeControls || !ShowUiRenamed)
                {
                    return;
                }

                IsVisible = finalAlpha != 0f;

                // if the controls are shown and the player is playing they should automatically hide after a while.
                // if the controls are hidden remove fade out runnable
                if (finalAlpha == 1f && IsPlaying)
                {
                    StartFadeOutViewTimer();
                }
                else
                {
                    Handler.RemoveCallbacks(FadeOutRunnable);
                }


                ControlsRoot.Animate().Alpha(finalAlpha).SetDuration(300)
                    .SetListener(new AnimatorListenerAnonymousInnerClass(this, finalAlpha)).Start();
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        private class AnimatorListenerAnonymousInnerClass : Object, Animator.IAnimatorListener
        {
            private readonly DefaultPlayerUiController OuterInstance;

            private readonly float FinalAlpha;

            public AnimatorListenerAnonymousInnerClass(DefaultPlayerUiController outerInstance, float finalAlpha)
            {
                OuterInstance = outerInstance;
                FinalAlpha = finalAlpha;
            }

            public void OnAnimationStart(Animator animator)
            {
                try
                {
                    if (FinalAlpha == 1f)
                    {
                        OuterInstance.ControlsRoot.Visibility = ViewStates.Visible;
                    }
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                }

            }

            public void OnAnimationEnd(Animator animator)
            {
                try
                {
                    if (FinalAlpha == 0f)
                    {
                        OuterInstance.ControlsRoot.Visibility = ViewStates.Gone;
                    }
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                }

            }

            public void OnAnimationCancel(Animator animator)
            {
            }

            public void OnAnimationRepeat(Animator animator)
            {
            }
        }

        private void StartFadeOutViewTimer()
        {
            try
            {
                Handler.PostDelayed(FadeOutRunnable, 3000);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void OnYouTubePlayerEnterFullScreen()
        {
            try
            {
                FullScreenButton.SetImageResource(Resource.Drawable.ic_fullscreen_exit_24dp);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            } 
        }

        public void OnYouTubePlayerExitFullScreen()
        {
            try
            {
                FullScreenButton.SetImageResource(Resource.Drawable.ic_fullscreen_24dp);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        // YouTubePlayer callbacks  

        // TODO refactor this method 
        public void OnStateChange(int state)
        {
            try
            {
                NewSeekBarProgress = -1;

                UpdateControlsState(state);

                if (state == PlayerConstants.PlayerState.Playing || state == PlayerConstants.PlayerState.Paused || state == PlayerConstants.PlayerState.VideoCued)
                {
                    Panel.SetBackgroundColor(new Color(ContextCompat.GetColor(YouTubePlayerView.Context, global::Android.Resource.Color.Transparent)));
                    ProgressBar.Visibility = ViewStates.Gone;

                    if (ShowPlayPauseButtonRenamed)
                    {
                        PlayPauseButton.Visibility = ViewStates.Visible;
                    }
                    
                    CanFadeControls = true;
                    bool playing = state == PlayerConstants.PlayerState.Playing;
                    UpdatePlayPauseButtonIcon(playing);

                    if (playing)
                    {
                        StartFadeOutViewTimer();
                    }
                    else
                    {
                        Handler.RemoveCallbacks(FadeOutRunnable);
                    }

                }
                else
                {
                    UpdatePlayPauseButtonIcon(false);
                    FadeControls(1f);

                    if (state == PlayerConstants.PlayerState.Buffering)
                    {
                        Panel.SetBackgroundColor(new Color(ContextCompat.GetColor(YouTubePlayerView.Context, global::Android.Resource.Color.Transparent)));
                        if (ShowPlayPauseButtonRenamed)
                        {
                            PlayPauseButton.Visibility = ViewStates.Invisible;
                        }
                         
                        CanFadeControls = false;
                    }

                    if (state == PlayerConstants.PlayerState.Unstarted)
                    {
                        CanFadeControls = false;

                        ProgressBar.Visibility = ViewStates.Gone;
                        if (ShowPlayPauseButtonRenamed)
                        {
                            PlayPauseButton.Visibility = ViewStates.Visible;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        private void UpdateControlsState(int state)
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
                else if (state == PlayerConstants.PlayerState.Unstarted)
                {
                    ResetUi();
                }


                UpdatePlayPauseButtonIcon(!IsPlaying);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void OnCurrentSecond(int second)
        {
            try
            {
                CurrentSecond = second;

                // ignore if the user is currently moving the SeekBar
                if (SeekBarTouchStarted)
                {
                    return;
                }

                // ignore if the current time is older than what the user selected with the SeekBar
                if (NewSeekBarProgress > 0 && !Utils.FormatTime(second).Equals(Utils.FormatTime(NewSeekBarProgress)))
                {
                    return;
                }

                NewSeekBarProgress = -1;

                switch (Build.VERSION.SdkInt)
                {
                    case >= BuildVersionCodes.N:
                        SeekBar.SetProgress(second, true);
                        break;
                    // For API < 24 
                    default:
                        SeekBar.Progress = second;
                        break;
                } 
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            } 
        }

        public void OnVideoDuration(int duration)
        {
            try
            {
                VideoDuration.Text = Utils.FormatTime(duration);
                SeekBar.Max = duration;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void OnVideoLoadedFraction(int loadedFraction)
        {
            try
            {
                if (ShowBufferingProgressRenamed)
                {
                    SeekBar.SecondaryProgress = loadedFraction * SeekBar.Max;
                }
                else
                { 
                    SeekBar.SecondaryProgress = 0;
                } 
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            } 
        }



        public void OnVideoId(string videoId)
        {
            try
            {
                VideoIdYoutube = videoId;
                YouTubeButton.SetOnClickListener(new OnClickListenerAnonymousInnerClass(this, videoId));
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        private class OnClickListenerAnonymousInnerClass : Object, View.IOnClickListener
        {
            private readonly DefaultPlayerUiController OuterInstance;

            private readonly string VideoId;

            public OnClickListenerAnonymousInnerClass(DefaultPlayerUiController outerInstance, string videoId)
            {
                try
                {
                    OuterInstance = outerInstance;
                    VideoId = videoId;
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                }

            }

            public void OnClick(View view)
            {
                try
                {
                    Intent intent = new Intent(Intent.ActionView, Uri.Parse("http://www.youtube.com/watch?v=" + VideoId + "#t=" + OuterInstance.SeekBar.Progress));
                    OuterInstance.ControlsRoot.Context.StartActivity(intent);
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                }
            }
        }

        public void OnReady()
        {
        }

        public void OnPlaybackQualityChange(string playbackQuality)
        {
        }

        public void OnPlaybackRateChange(string rate)
        {
        }

        public void OnError(int error)
        {
        }

        public void OnApiChange()
        {
        }

        // SeekBar callbacks

        private bool SeekBarTouchStarted = false;

        // I need this variable because onCurrentSecond gets called every 100 mils, so without the proper checks on this variable in onCurrentSeconds the seek bar glitches when touched.
        private int NewSeekBarProgress = -1;

        public void OnProgressChanged(SeekBar seekBar, int i, bool b)
        {
            try
            {
                VideoCurrentTime.Text = Utils.FormatTime(i);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            try
            {
                SeekBarTouchStarted = true;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            try
            {
                if (IsPlaying)
                {
                    NewSeekBarProgress = seekBar.Progress;
                }

                YouTubePlayer.SeekTo(seekBar.Progress);
                SeekBarTouchStarted = false;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        private void ResetUi()
        {
            try
            {
                switch (Build.VERSION.SdkInt)
                {
                    case >= BuildVersionCodes.N:
                        SeekBar.SetProgress(0, true);
                        break;
                    // For API < 24 
                    default:
                        SeekBar.Progress = 0;
                        break;
                }

                SeekBar.Max = 0;
                VideoDuration.Post(() => { VideoDuration.Text = ""; });
                //youTubeButton.setOnClickListener(null);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }
    }

}