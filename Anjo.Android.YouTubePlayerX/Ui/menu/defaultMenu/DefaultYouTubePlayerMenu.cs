using Android.Content;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Anjo.Android.YouTubePlayerX.Util;

namespace Anjo.Android.YouTubePlayerX.Ui.menu.defaultMenu
{
    public class DefaultYouTubePlayerMenu : IYouTubePlayerMenu
    {

        private readonly Context Context;
        private readonly IList<MenuItem> MenuItems;


        private PopupWindow PopupWindow;


        public DefaultYouTubePlayerMenu(Context context)
        {
            try
            {
                Context = context;
                MenuItems = new List<MenuItem>();
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        public void Show(View anchorView)
        {
            try
            {
                PopupWindow = CreatePopupWindow();
                PopupWindow.ShowAsDropDown(anchorView, 0,
                    -Context.Resources.GetDimensionPixelSize(Resource.Dimension._8dp) * 4);

                if (MenuItems.Count == 0)
                {
                    Log.Error(typeof(IYouTubePlayerMenu).FullName, "The menu is empty");
                }
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        public void Dismiss()
        {
            try
            {
                PopupWindow?.Dismiss();
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        public void AddItem(MenuItem menuItem)
        {
            try
            {
                MenuItems.Add(menuItem);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }
        }

        public void RemoveItem(int itemIndex)
        {
            try
            {
                MenuItems.RemoveAt(itemIndex);
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
            }

        }

        public int ItemCount => MenuItems.Count;


        private PopupWindow CreatePopupWindow()
        {
            try
            {
                LayoutInflater inflater = (LayoutInflater) Context.GetSystemService(Context.LayoutInflaterService);

                if (inflater == null)
                {
                    throw new Exception("can't access LAYOUT_INFLATER_SERVICE");
                }

                View view = inflater.Inflate(Resource.Layout.player_menu, null);

                RecyclerView recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view);
                UpRecyclerView = recyclerView;

                PopupWindow popupWindow =
                    new PopupWindow(view, ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
                    {
                        Focusable = true,
                        Width = ViewGroup.LayoutParams.WrapContent,
                        Height = ViewGroup.LayoutParams.WrapContent,
                        ContentView = view
                    };

                return popupWindow;
            }
            catch (Exception e)
            {
                Utils.DisplayReportResultTrack(e);
                return null;
            }
        }

        private RecyclerView UpRecyclerView
        {
            set
            {
                try
                {
                    value.HasFixedSize = true;
                    value.SetLayoutManager(new LinearLayoutManager(Context));

                    MenuAdapter adapter = new MenuAdapter(Context, MenuItems);
                    value.SetAdapter(adapter);
                }
                catch (Exception e)
                {
                    Utils.DisplayReportResultTrack(e);
                }

            }
        }
    }

}