using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KeyTools;
namespace KeyTools.Hotkeying
{
    public class Hotkey
    {

       

        /// <summary>
        /// Use the OR (|) to combine flags
        /// </summary>
        [Flags]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            // Either WINDOWS key was held down. These keys are labeled with the Windows logo.
            // Keyboard shortcuts that involve the WINDOWS key are reserved for use by the
            // operating system.
            Windows = 8
        }

        /// <summary>
        /// The automatically assigned Hotkeyid needed to register and unregister a hotkey
        /// </summary>
        public int HotkeyId { get; private set; }

        /// <summary>
        /// The registered key
        /// </summary>
        public Key Key { get; private set; }
        /// <summary>
        /// The registered key represented as Win32 virtual key
        /// </summary>
        public int VirtualKey { get; private set; }
        /// <summary>
        /// The specified Hotkey modifiers
        /// </summary>
        public KeyModifiers Modifiers { get; private set; }
        /// <summary>
        /// The callback when hotkey is pressed
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// Instatiates a new Hotkey which can be registered with the <c>HotKeyManager</c> class. See <see cref="HotkeyManager.AddHotkey(Hotkey)"/>
        /// </summary>
        /// <param name="key">A key from <see cref="System.Windows.Input.Key"/></param>
        /// <param name="modifiers">Modifierkeys needed to trigger hotkey. Bitwise or from <see cref="KeyModifiers"/></param>
        /// <param name="action">The callback which is executed when the hotkey is pressed</param>
        public Hotkey(Key key, KeyModifiers modifiers, Action action)
        {
            HotkeyId = GetHashCode();
            Key = key;
            VirtualKey = KeyInterop.VirtualKeyFromKey(key);
            Modifiers = modifiers;
            
            Action = action;
        }

        /// <summary>
        /// Instatiates a new Hotkey which can be registered with the <c>HotKeyManager</c> class. See <see cref="HotkeyManager.AddHotkey(Hotkey)"/>
        /// </summary>
        /// <param name="key">A virtual key from <see cref="VirtualKeyCodes.VirtualKeys"/></param>
        /// <param name="modifiers">Modifierkeys needed to trigger hotkey. Bitwise or from <see cref="KeyModifiers"/></param>
        /// <param name="action">The callback which is executed when the hotkey is pressed</param>
        public Hotkey(VirtualKeyCodes.VirtualKeys key, KeyModifiers modifiers, Action action)
        {
            HotkeyId = GetHashCode();
            Key = KeyInterop.KeyFromVirtualKey((int)key);
            VirtualKey = (int)key;
            Modifiers = modifiers;
            
            Action = action;
        }

        
        public override string ToString()
        {
            return Modifiers.ToString().Replace(","," +") + " + " + Key.ToString();
        }

    }
}
