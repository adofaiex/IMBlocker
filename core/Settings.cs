namespace IMBlocker
{
    /// <summary>
    /// User-visible mod settings.
    /// </summary>
    public class Settings
    {
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Disable the Windows IME while no text field is focused, so keyboard
        /// input goes straight to the game. Automatically restored when the user
        /// clicks a TMP_InputField / InputField.
        /// </summary>
        public bool BlockIme { get; set; } = true;

        /// <summary>Language code override (e.g. "en", "zh"); null = follow game language.</summary>
        public string? Language { get; set; }
    }
}
