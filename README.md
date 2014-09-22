BetterSerialMonitor
===================

A fairly simple cross-platform serial terminal, with more features than the Arduino one (for example). 
Allows sending text with intermixed raw bytes (prefixed with %, 0x, or &). For example, in "Send Text" mode, the string

    This is a %7F string

will send `This is a `, followed by the byte `7F` (the integer 127), then ` string`. 

In "Send Data" mode, give decimal or hex bytes separated by spaces, commas, or semicolons. 

Platforms
---------

It uses the .NET Framework, so it'll work on Windows easily and on MacOS and Linux if you have [Mono](http://www.mono-project.com/) installed. I have tested previous versions on Mac and Linux and they worked, however I cannot test on Mac regularly.  

Due to a limitation in Mono itself, this program won't automatically refresh the incoming data window on non-Windows platforms. You need to do this yourself by clicking on the window (the lower greyed-out text box). 

Installation
------------

I can package a simple Windows installer if anyone wants, however it doesn't require installation to work. It compiles to a single EXE that can be run standalone from anywhere. 
