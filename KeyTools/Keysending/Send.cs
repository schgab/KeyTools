using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KeyTools.Keysending
{
    public static class Send
    {
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
        public static void SendVirtualKeyCode(VirtualKeyCodes.VirtualKeys key)
        {
            var keys = new VirtualKeyCodes.VirtualKeys[1];
            keys[0] = key;
            SendVirtualKeyCode(keys);
            

        }
        public static void SendVirtualKeyCode(VirtualKeyCodes.VirtualKeys[] keys)
        {
            var keyboardInputs = keys.Select(c => new WinApi.KEYBDINPUT()
            {
                dwExtraInfo = IntPtr.Zero,
                dwFlags = (int)WinApi.DWFLAGS.None,
                wScan = 0,
                wVk = (ushort)c
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
