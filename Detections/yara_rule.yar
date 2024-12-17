import "pe"

rule KRUEGER {
	meta :
		description = "Identifies a Krueger binary"
		author = "Jonathan Beierle"
		reference = "https://github.com/logangoins/Krueger"
	strings:
		$policydst1 = "\\C$\\Windows\\System32\\CodeIntegrity\\SiPolicy.p7b"
		$policydst2 = "ADMIN$\\System32\\CodeIntegrity\\SiPolicy.p7b"

		/* Strings and bytes used to identify an embedded WDAC policy */
		$wdac1 = { 0E 37 44 A2 C9 44 06 4C B5 51 F6 01 6E 56 30 76 }  /* Bytes used for several section headers in WDAC policies */
		$wdac2 = "_?r"
		$wdac3 = "PTbS^}"
		$wdac4 = "TJ-"

		$s1 = "Krueger.exe"
		$s2 = "Krueger.SiPolicy.p7b"
	condition:
		pe.imphash() == "f34d5f2d4577ed6d9ceec516c1f5a744" or
		(
			(  /* Test for embedded WDAC policy */
				all of ($wdac*) and
				#wdac1 >= 3
			) and 
			any of ($s*) or
			any of ($policydst*)
		)
}

private rule ENFORCED_BLOCKING_WDAC_POLICY {
	meta:
		description = "Identifies if a file is a compiled WDAC policy that enforces blocking rule(s)"
		author = "Jonathan Beierle"
	strings:
		$filesignature = { 07 00 00 00 0E }
		$blockbytes = { FF FF FF FF FF FF FF FF }
		$enforcebyte = { 8C }
	condition:
		$filesignature at 0x00 and ($blockbytes at 0xE0 or $blockbytes at 0xF8) and $enforcebyte at 0x26
}

rule BLOCK_WINDOWS_DEFENDER {
	meta:
		description = "Identifies if a compiled WDAC policy references Windows Defender details in a potentially malicious manner"
		author = "Jonathan Beierle"
	strings:
		/* Executables that may be blocked */
		$blockexe1 = "MsSense.exe" fullword wide
		$blockexe2 = "MsMpEng.exe" fullword wide
		$blockexe3 = "MsDefenderCoreService.exe" fullword wide

		/* Executables descriptions that may be blocked */
		$blockattr1 = "Windows Defender Advanced Threat Protection Service Executable" fullword wide
		$blockattr2 = "Antimalware Service Executable" fullword wide
		$blockattr3 = "Antimalware Core Service" fullword wide
	condition:
		ENFORCED_BLOCKING_WDAC_POLICY and any of ($blockexe*,$blockattr*)
}

rule BLOCK_CROWDSTRIKE {
	meta:
		description = "Identifies if a compiled WDAC policy references CrowdStrike Falcon details in a potentially malicious manner"
		author = "Jonathan Beierle"
	strings:
		$blockexe1 = "CSFalconService.exe" fullword wide
		$blockdriver1 = "CSAgent.sys" fullword wide
		$blockattr1 = "CrowdStrike Falcon Sensor" fullword wide
	condition:
		ENFORCED_BLOCKING_WDAC_POLICY and any of ($blockexe*,$blockdriver1,$blockattr*)
}
