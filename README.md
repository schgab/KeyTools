# KeyTools
A small and easy to use Hotkey library with support for sending using windows api functions.


## Usage

### Hotkeying

Use the `Hotkey` class to define a hotkey and register it using an instance of `HotkeyManager` (Note: You need to hold a reference to this instance otherwise your hotkeys will get unregistered)


If you intend to use a WinForms Application you need to attach it to the message loop ([WndProc](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.wndproc?view=netframework-4.8)) and call the `MessageHandler` method of your HotkeyManager instance (Make sure you also pass true as second argument to the constructor). Create a HotkeyMessage instance and pass the Msg and wParam field of the windows Message object to the constructor. **If you use WPF skip this step**.

### Simulate keyboard

Use the `Send` class. Sending is done using the Windows API method `SendInput`

### Virtual Keycodes

All virtual Keycodes can be found in the static `VirtualKeyCodes` class

### Compiling

Download this project and compile it using Visual Studio in .Net 4.5 or use the binary on the [Release](https://github.com/schgab/KeyTools/releases) page.
