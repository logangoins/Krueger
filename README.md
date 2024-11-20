# Description
Krueger is a .NET post-exploitation toolkit for remotely killing Endpoint Detection and Response (EDR) processes as apart of lateral movement procedures. Krueger accomplishes this by utilizing Windows Defender Application Control (WDAC), which is an in-built Microsoft created application control method that has the ability to block process creation at the kernel level. Using Krueger with Administrative permissions over a target remote device, an attacker can quickly install a WDAC policy to disk and perform a remote reboot through Windows Management Instrumentation (WMI), preventing the EDR service from starting on boot. 

Krueger can also be run from memory using tools such as `execute-assembly` and `InlineExecute-Assembly`. To prevent the need to load a WDAC policy from disk while executing Krueger from memory, Krueger includes an embedded WDAC policy inside of the .NET assembly inserted at compile time in which it reads from memory and writes to a target at runtime

More information about this technique can be found on our blog at : BLOG LINK

![image](https://github.com/user-attachments/assets/9d6cc181-972e-4e2a-a5e6-beedd6656685)

