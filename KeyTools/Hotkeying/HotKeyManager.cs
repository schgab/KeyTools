using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace KeyTools.Hotkeying
{
    public class HotkeyManager :IDisposable
    {
        /// <summary>
        /// Gets all hotkeys identified by their ids
        /// </summary>
        public Dictionary<int,Hotkey> Hotkeys { get; private set; }

        /// <summary>
        /// Get the window handle the HotKeyManager is attached too
        /// </summary>
        public IntPtr WindowHandle { get; private set; }

        /// <summary>
        /// If true then <c>MessageHandler</c> needs to be called with the appropiate <c>MSG</c> objects
        /// </summary>
        public bool UsingCustomMessageLoop { get; private set; }

        /// <summary>
        /// Instatiates a new HotkeyManager object. Hold a reference to this class otherwise hotkeys will be unregistered
        /// </summary>
        /// <param name="handle">Window handle the HotkeyManager gets attached to</param>
        /// <param name="useOwnMessageLoop">Use this if you are using winforms. Call <see cref="MessageHandler(MSG)"/> in your message loop and pass the message</param>
        public HotkeyManager(IntPtr handle, bool useOwnMessageLoop = false)
        {
            Hotkeys = new Dictionary<int, Hotkey>();
            if (handle == IntPtr.Zero)
            {
                throw new Exception("No window handle specified");
            }
            WindowHandle = handle;
            if (!useOwnMessageLoop)
            {
                ComponentDispatcher.ThreadPreprocessMessage += PreprocessMessage;
            }
            UsingCustomMessageLoop = useOwnMessageLoop;
        }


        /// <summary>
        /// Call this method if you don't use a WPF application
        /// </summary>
        /// <param name="msg">The MSG object you receive in your <c>WndProc</c> method</param>
        public void MessageHandler(MSG msg)
        {
            if (UsingCustomMessageLoop)
            {
                var handled = false;
                PreprocessMessage(ref msg, ref handled);
            }
        }


        private void PreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (handled || msg.message != WinApi.WM_HOTKEY)
            {
                return;
            }
            int id = (int)msg.wParam;
            if (Hotkeys.ContainsKey(id))
            {
                handled = true;
                Hotkeys[id].Action();
            }
        }

        /// <summary>
        /// Used to register a hotkey
        /// </summary>
        /// <param name="hotkey">A <c>Hotkey</c> object. See <see cref="Hotkey"/></param>
        /// <returns></returns>
        public bool AddHotkey(Hotkey hotkey)
        {
            if (Hotkeys.ContainsKey(hotkey.HotkeyId))
            {
                return false;
            }
            var registered = WinApi.RegisterHotKey(WindowHandle, hotkey.HotkeyId, hotkey.Modifiers, hotkey.VirtualKey);
            if (registered)
            {
                Hotkeys.Add(hotkey.HotkeyId, hotkey);
            }
            return registered;

        }


        ~HotkeyManager()
        {
            Dispose();
        }
        /// <summary>
        /// Unregisters all hotkeys
        /// </summary>
        public void Dispose()
        {
           
            ComponentDispatcher.ThreadPreprocessMessage -= PreprocessMessage;
            foreach (var hk in Hotkeys.Values)
            {
                WinApi.UnregisterHotKey(WindowHandle, hk.HotkeyId);                
            }
            Hotkeys.Clear();
        }
    }
}
