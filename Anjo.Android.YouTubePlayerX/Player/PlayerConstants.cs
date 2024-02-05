namespace Anjo.Android.YouTubePlayerX.Player
{
    public static class PlayerConstants
    {
        public static class PlayerState
        {
            public static int Unknown = -10;
            public static int Unstarted = -1;
            public static int Ended = 0;
            public static int Playing = 1;
            public static int Paused = 2;
            public static int Buffering = 3;
            public static int VideoCued = 5;
        }

        public static class PlaybackQuality
        {
            public const string Unknown = "unknown";
            public const string Small = "small";
            public const string Medium = "medium";
            public const string Large = "large";
            public const string Hd720 = "hd720";
            public const string Hd1080 = "hd1080";
            public const string HighRes = "highres";
            public const string Default = "default";
        }

        public static class PlayerError
        {
            public const int Unknown = -10;
            public const int InvalidParameterInRequest = 0;
            public const int Html5Player = 1;
            public const int VideoNotFound = 2;
            public const int VideoNotPlayableInEmbeddedPlayer = 3;
        }

        public static class PlaybackRate
        {
            public const string Unknown = "-10";
            public const string Rate025 = "0.25";
            public const string Rate05 = "0.5";
            public const string Rate1 = "1";
            public const string Rate15 = "1.5";
            public const string Rate2 = "2";
        }
    }
}