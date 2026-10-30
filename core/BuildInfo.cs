namespace IMBlocker
{
    /// <summary>
    /// Single source of truth for mod metadata. Bump <see cref="Version"/> here;
    /// every csproj picks it up automatically at build time (see
    /// Directory.Build.props) and the loader adapters read these constants for
    /// their own manifests, so no other file needs editing.
    ///
    /// UMM additionally ships a static Info.json manifest next to this repo root;
    /// keep its Id/Version in sync when releasing.
    /// </summary>
    public static class BuildInfo
    {
        public const string ModId = "IMBlocker";
        public const string DisplayName = "IMBlocker";
        public const string Author = "adofaiex";
        public const string Description = "IME blocker for ADOFAI.";
        public const string Version = "0.1.7";
        public const string Repository = "https://github.com/adofaiex/IMBlocker";

        /// <summary>Prefixed tag used in log messages, e.g. "[IMBlocker][Module]".</summary>
        public const string LogPrefix = $"[{ModId}]";
    }
}
