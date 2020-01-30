using System;

namespace KeyTools.Hotkeying
{
    public struct HotkeyMessage
    {
        public int MessageID { get; set; }
        public IntPtr wParam { get; set; }

        public HotkeyMessage(int messageID, IntPtr wparam)
        {
            MessageID = messageID;
            wParam = wparam;
        }
    }
}
