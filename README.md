BetterSerialMonitor
===================

A fairly simple cross-platform serial terminal, with more features than the Arduino one (for example). 
Allows sending text with intermixed raw bytes (prefixed with %, 0x, or &). For example, in "Send Text" mode, the string
    This is a %7F string
will send "This is a ", followed by the byte 7F, then " string". 

In "Send Data" mode, give decimal or hex bytes separated by spaces, commas, or semicolons. 
