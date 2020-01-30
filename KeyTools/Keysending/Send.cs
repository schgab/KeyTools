using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace KeyTools.Keysending
{
    /// <summary>
    /// A class using the native Windows API to emulate keyboard input
    /// </summary>
    public static class Send
    {

        /// <summary>
        /// Sends a unicode string as keypresses to the foreground window
        /// </summary>
        /// <param name="toSend">A unicode string</param>
        public static void SendUnicode(string toSend)
        {
            var keyboardInputs = toSend.Select(c => new WinApi.KEYBDINPUT()
            {
                dwExtraInfo = IntPtr.Zero,
                dwFlags = (int)WinApi.DWFLAGS.KEYEVENTF_UNICODE,
                wScan = c,
                wVk = 0x0
            });

            var inputs = keyboardInputs.Select(c => new WinApi.INPUT
            {
                type = (int)WinApi.InputType.Keyboard,
                input = new WinApi.COMBINEDINPUT() { keyboardInput = c}
            }).ToArray();

            WinApi.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(WinApi.INPUT)));
            
        }

        /// <summary>
        /// Emulates a key being held down
        /// </summary>
        /// <param name="key">The virtual key to be held down</param>
        public static void SendKeyDown(VirtualKeyCodes.VirtualKeys key)
        {
            WinApi.INPUT input = new WinApi.INPUT();
            input.input = new WinApi.COMBINEDINPUT() { keyboardInput = CreateKeyDown(key) };
            input.type = 1;
            WinApi.SendInput(1, new WinApi.INPUT[] { input }, Marshal.SizeOf(typeof(WinApi.INPUT)));
        }

        /// <summary>
        /// Releases a held key
        /// </summary>
        /// <param name="key">The key to be released</param>
        public static void SendKeyUp(VirtualKeyCodes.VirtualKeys key)
        {
            WinApi.INPUT input = new WinApi.INPUT();
            input.input = new WinApi.COMBINEDINPUT() { keyboardInput = CreateKeyUp(key) };
            input.type = 1;
            WinApi.SendInput(1, new WinApi.INPUT[] { input }, Marshal.SizeOf(typeof(WinApi.INPUT)));
        }


        /// <summary>
        /// Emulates a single keypress
        /// </summary>
        /// <param name="key">A <see cref="VirtualKeyCodes.VirtualKeys"/> key</param>
        public static void SendVirtualKeyCode(VirtualKeyCodes.VirtualKeys key)
        {            
            SendVirtualKeyCode(new VirtualKeyCodes.VirtualKeys[] { key });
        }

        /// <summary>
        /// Helper method for creating a keydown keybdinput
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static WinApi.KEYBDINPUT CreateKeyDown(VirtualKeyCodes.VirtualKeys key)
        {
            return new WinApi.KEYBDINPUT()
            {
                dwExtraInfo = IntPtr.Zero,
                dwFlags = (int)WinApi.DWFLAGS.KEYEVENTF_SCANCODE,
                wScan = (ushort)WinApi.MapVirtualKey((uint)key, 0),
                wVk = (ushort)key
            };
        }

        /// <summary>
        /// Helper method for creating a keyup keybdinput
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static WinApi.KEYBDINPUT CreateKeyUp(VirtualKeyCodes.VirtualKeys key)
        {
            return new WinApi.KEYBDINPUT()
            {
                dwExtraInfo = IntPtr.Zero,
                dwFlags = (int)(WinApi.DWFLAGS.KEYEVENTF_KEYUP | WinApi.DWFLAGS.KEYEVENTF_SCANCODE),
                wScan = (ushort)WinApi.MapVirtualKey((uint)key,0),
                wVk = (ushort)key
            };
        }

        /// <summary>
        /// Presses the specified sequence of <paramref name="keys"/>
        /// </summary>
        /// <param name="keys"></param>
        public static void SendVirtualKeyCode(IEnumerable<VirtualKeyCodes.VirtualKeys> keys)
        {
            var keyboardInputs = keys.SelectMany(c =>
            {
                var list = new List<WinApi.KEYBDINPUT>();
                list.Add(CreateKeyDown(c));
                list.Add(CreateKeyUp(c));
                return list;
            });

            var inputs = keyboardInputs.Select(c => new WinApi.INPUT
            {
                type = (int)WinApi.InputType.Keyboard,
                input = new WinApi.COMBINEDINPUT() { keyboardInput = c }
            }).ToArray();

            WinApi.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(WinApi.INPUT)));
        }
    }
}
