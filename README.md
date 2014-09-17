BetterSerialMonitor
===================

A fairly simple cross-platform serial terminal, with more features than the Arduino one (for example). 
Allows sending text with intermixed raw bytes (prefixed with %, 0x, or &). For example, in "Send Text" mode, the string

    This is a %7F string

will send `This is a `, followed by the byte `7F` (the integer 127), then ` string`. You can also specify multiple consecutive bytes with one prefix if you write an even number of hex characters, for example:

    %7F4EAC
will send all three bytes consecutively, however `%7F4AE` will only send the bytes `7F` and `4A`, and then the character capital E (ASCII code `0x45`).

In "Send Data" mode, give decimal or hex bytes separated by spaces, commas, periods, or semicolons. 

Platforms
---------

It uses the .NET Framework, so it'll work on Windows easily and on MacOS and Linux if you have [Mono](http://www.mono-project.com/) installed. I have tested previous versions on Mac and Linux and they worked, however I cannot test on Mac regularly.  

Due to a limitation in Mono itself, this program won't automatically refresh the incoming data window on non-Windows platforms. You need to do this yourself by clicking on the window (the lower greyed-out text box). The click-to-refresh functionality is only available if you compile it with Mono. 

Installation
------------

I can package a simple Windows installer if anyone wants, however it doesn't require installation to work. It compiles to a single EXE that can be run standalone from anywhere. 