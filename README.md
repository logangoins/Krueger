# Description
Krueger is a .NET post-exploitation toolkit for remotely killing Endpoint Detection and Response (EDR) as apart of lateral movement procedures. Krueger accomplishes this by utilizing Windows Defender Application Control (WDAC), which is an in-built Microsoft created application control method that has the ability to block code at the kernel level. Using Krueger with administrative permissions over a target remote device, an attacker can quickly install a WDAC policy to disk and perform a remote reboot through Windows Management Instrumentation (WMI), preventing the EDR service from starting on boot. 

Krueger can also be run from memory using tools such as `execute-assembly` and `inlineExecute-Assembly`. To prevent the need to load a WDAC policy from disk while executing Krueger from memory, Krueger includes an embedded WDAC policy inside of the .NET assembly inserted at compile time in which it reads from memory and writes to a target at runtime

More information about this technique can be found on our blog at : BLOG LINK

![image](https://github.com/user-attachments/assets/9d6cc181-972e-4e2a-a5e6-beedd6656685)

# Usage

```
 ____  __.                                         
|    |/ _|______ __ __   ____   ____   ___________ 
|      < \_  __ \  |  \_/ __ \ / ___\_/ __ \_  __ \
|    |  \ |  | \/  |  /\  ___// /_/  >  ___/|  | \/
|____|__ \|__|  |____/  \___  >___  / \___  >__|   
        \/                  \/_____/      \/
~~~~~~
@_logangoins
@hullabrian

Krueger.exe [Options]

Options:
	-h/--help                -     Display this help menu
	--host <hostname>        -     Kill EDR on a specified host
	--username <username>    -     A username to use for authentication
	--domain <domain>        -     A domain to use for authentication
	--password <password>    -     A password to use for authentication
```

