# AntiInvokeDetection
most string deobfuscators use Invoke to pick up the strings, however some obfuscators are using "GetCallingAssembly" to check if the method is being executed by another assembly
<br />
This tool, just replace "GetCallingAssembly" to "GetExecutingAssembly" for ignore the invoke detection!
