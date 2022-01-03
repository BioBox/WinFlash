
# WinFlash

This is essentially a minimalist version of [Anki](https://apps.ankiweb.net/).
If you don't care about bloat then just use that; it's much more polished and
feature-rich.

If you _do care_ about bloat however, then I think you'll find this to be a
good starting point. This is the Windows version of what I plan to make into a
mobile app with the power of [Xamarin](#xamarin). If you wish to make a
command-line version of this then you shouldn't have much trouble, as you
essentially only need to translate `deck.cs` into a language of your choice
and give it an interface.

## Usage

All save data is stored within the `Appdata\Roaming\WinFlash` directory which
is created upon startup. The name of each folder within this directory is the
name of a deck. Each of them should contain three json files:

* `<name>.json`: the contents of the deck
* `<name>-deck.json`: the deck's various settings
* `<name>-meta.json`: study information (**do not edit**)

You only need to create the first file. The other two will be created and
managed for you, though you are welcome to edit the settings if you wish.
You are expected to create these files yourself with the text editor of your
choice. A simple example can be found within `examples\`.

## Todo

1. Auto-resize the window after studying is complete.
2. Add a button on the main-menu to review difficult cards.
3. Give user the ability to edit the font per deck via its settings.
4. Add support for pictures, resizing them to fit the window when loaded.
5. Add a button to let user get back to main-menu while studying.
6. Show the amount of cards/new cards left to study

[xamarin]: https://dotnet.microsoft.com/en-us/apps/xamarin
