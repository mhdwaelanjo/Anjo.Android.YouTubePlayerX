using Android.Views;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using Anjo.Android.YouTubePlayerX.Util;
using Context = Android.Content.Context;

namespace Anjo.Android.YouTubePlayerX.Ui.menu.defaultMenu
{
    internal class MenuAdapter : RecyclerView.Adapter
    {
        private readonly Context Context;
        private readonly IList<MenuItem> MenuItems;

        internal MenuAdapter(Context context, IList<MenuItem> menuItems)
        {
            try
            {
                Context = context;
                MenuItems = menuItems;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                View view = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.menu_item, parent, false);
                return new ViewHolder(this, view);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return null;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holders, int position)
        {
            try
            {
                if (holders is ViewHolder holder)
                {
                    holder.Root.SetOnClickListener(MenuItems[position].OnClickListener);
                    holder.TextView.Text = MenuItems[position].Text;
                    holder.TextView.SetCompoundDrawablesWithIntrinsicBounds(
                        ContextCompat.GetDrawable(Context, MenuItems[position].Icon), null, null, null);
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }


        public override int ItemCount => MenuItems.Count;

        internal class ViewHolder : RecyclerView.ViewHolder
        {
            private readonly MenuAdapter OuterInstance;

            internal readonly View Root;
            internal readonly TextView TextView;

            internal ViewHolder(MenuAdapter outerInstance, View menuItemView) : base(menuItemView)
            {
                try
                {
                    OuterInstance = outerInstance;
                    Root = menuItemView;
                    TextView = menuItemView.FindViewById<TextView>(Resource.Id.text);
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                }
            }
        }
    }
}