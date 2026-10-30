using BepInEx;

namespace IMBlocker.Loaders
{
    [BepInPlugin(IMBlocker.BuildInfo.ModId, IMBlocker.BuildInfo.DisplayName, IMBlocker.BuildInfo.Version)]
    [BepInProcess("A Dance of Fire and Ice.exe")]
    public class AdofaiBepInPlugin : BaseUnityPlugin
    {
        private BepInHandler? _handler;

        private void Awake()
        {
            _handler = new BepInHandler(Logger);
            Main.Initialize(_handler);
            _handler.TriggerToggle(true);
        }

        private void Update()
        {
            _handler?.TriggerUpdate(UnityEngine.Time.deltaTime);
        }

        private void OnGUI()
        {
            _handler?.TriggerGUI();
        }
    }
}
