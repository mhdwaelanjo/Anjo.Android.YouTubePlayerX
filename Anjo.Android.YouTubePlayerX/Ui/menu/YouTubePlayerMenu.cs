using Android.Views;

namespace Anjo.Android.YouTubePlayerX.Ui.menu
{
    public interface IYouTubePlayerMenu
    {
        void Show(View anchorView);
        void Dismiss();

        void AddItem(MenuItem menuItem);
        void RemoveItem(int itemIndex);
        int ItemCount { get; }
    }
}