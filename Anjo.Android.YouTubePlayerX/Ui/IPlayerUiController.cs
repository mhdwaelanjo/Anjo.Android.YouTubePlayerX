using Android.Graphics.Drawables;
using Android.Views;
using Anjo.Android.YouTubePlayerX.Ui.menu;

namespace Anjo.Android.YouTubePlayerX.Ui
{
    public interface IPlayerUiController
    {
        void ShowUi(bool show);
        void ShowPlayPauseButton(bool show);

        void ShowVideoTitle(bool show);

        string VideoTitle { set; }

        void EnableLiveVideoUi(bool enable);


        void SetCustomActionLeft1(Drawable icon, View.IOnClickListener clickListener);

        void SetCustomActionRight1(Drawable icon, View.IOnClickListener clickListener);
        void ShowCustomActionLeft1(bool show);
        void ShowCustomActionRight1(bool show);
        
        void SetCustomActionLeft2(Drawable icon, View.IOnClickListener clickListener);

        void SetCustomActionRight2(Drawable icon, View.IOnClickListener clickListener);
        void ShowCustomActionLeft2(bool show);
        void ShowCustomActionRight2(bool show);

        void ShowFullscreenButton(bool show);

        View.IOnClickListener CustomFullScreenButtonClickListener { set; }

        void ShowMenuButton(bool show);

        View.IOnClickListener CustomMenuButtonClickListener { set; }

        void ShowCurrentTime(bool show);
        void ShowDuration(bool show);

        void ShowSeekBar(bool show);
        void ShowBufferingProgress(bool show);

        void ShowYouTubeButton(bool show);

        /// <summary>
        /// Adds a View to the top of the player </summary>
        /// <param name="view"> View to be added </param>
        void AddView(View view);

        /// <summary>
        /// Removes a View added with <seealso>
        ///     <cref>AddView(View view)</cref>
        /// </seealso>
        /// </summary>
        /// <param name="view"> View to be removed </param>
        void RemoveView(View view);


        IYouTubePlayerMenu Menu { get; set; }

    }

}