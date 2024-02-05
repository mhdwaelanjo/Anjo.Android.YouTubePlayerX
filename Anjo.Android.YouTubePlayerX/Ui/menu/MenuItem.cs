using Android.Views;
using Anjo.Android.YouTubePlayerX.Util;

namespace Anjo.Android.YouTubePlayerX.Ui.menu
{
    public abstract class MenuItem
    {

        private readonly string text;

        private readonly int icon;


        public MenuItem(string text, int icon, View.IOnClickListener onClickListener)
        {
            try
            {
                this.text = text;
                this.icon = icon;
                OnClickListener = onClickListener;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        public string Text => text;

        public int Icon => icon;

        public View.IOnClickListener OnClickListener { get; }

        public override bool Equals(object o)
        {
            try
            {
                if (this == o)
                {
                    return true;
                }

                if (o == null || GetType() != o.GetType())
                {
                    return false;
                }

                MenuItem menuItem = (MenuItem) o;

                return icon == menuItem.icon && text.Equals(menuItem.text);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return false;
            }
        }

        public override int GetHashCode()
        {
            try
            {
                int result = text.GetHashCode();
                result = 31 * result + icon;
                return result;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return 0;
            }
        }
    }
}