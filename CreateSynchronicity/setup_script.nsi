#This file is part of Create Synchronicity.
#
#Create Synchronicity is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
#Create Synchronicity is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
#You should have received a copy of the GNU General Public License along with Create Synchronicity.  If not, see <http://www.gnu.org/licenses/>.
#Created by:	Clément Pit--Claudel.
#Web site:		http://synchronicity.sourceforge.net.

!include MUI2.nsh
!define	/file	VERSION		"version-string.txt"

#Change the company name to ensure no conflict with mainline Create Synchronicity
!define 		COMPANY		"Create Software (With-Context-Menu)"
!define 		PRODUCTNAME	"Create Synchronicity (With-Context-Menu)"
!define			BINARYNAME  "Create Synchronicity.exe"

!define 		REGPATH		"Software\${COMPANY}"
!define 		SUBREGPATH	"${REGPATH}\${PRODUCTNAME}"

!define 		COMPANYPATH	"$PROGRAMFILES\${COMPANY}"
!define 		PROGRAMPATH	"${COMPANYPATH}\${PRODUCTNAME}"
!define			PRODUCTPATH	"${COMPANY}\${PRODUCTNAME}"

SetCompressor /SOLID lzma

Name "${PRODUCTNAME} ${VERSION}"
OutFile "..\Create_Synchronicity_Setup.exe"
InstallDir "${PROGRAMPATH}"
InstallDirRegKey HKLM "${SUBREGPATH}" "InstallPath"

RequestExecutionLevel admin
Var StartMenuFolder

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY

!define MUI_STARTMENUPAGE_REGISTRY_ROOT			"HKCU"
!define MUI_STARTMENUPAGE_REGISTRY_KEY			"${SUBREGPATH}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME	"StartMenuFolder"
!define MUI_STARTMENUPAGE_DEFAULTFOLDER			"${PRODUCTPATH}"
!insertmacro MUI_PAGE_STARTMENU AppStartMenu $StartMenuFolder

!insertmacro MUI_PAGE_INSTFILES

!define MUI_FINISHPAGE_RUN "$INSTDIR\${BINARYNAME}"
!define MUI_FINISHPAGE_RUN_TEXT "Launch ${PRODUCTNAME} now"
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "Bulgarian"
!insertmacro MUI_LANGUAGE "Czech"
!insertmacro MUI_LANGUAGE "Dutch"
!insertmacro MUI_LANGUAGE "Danish"
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "Estonian"
!insertmacro MUI_LANGUAGE "French"
!insertmacro MUI_LANGUAGE "German"
!insertmacro MUI_LANGUAGE "Hebrew"
!insertmacro MUI_LANGUAGE "Indonesian"
!insertmacro MUI_LANGUAGE "Italian"
!insertmacro MUI_LANGUAGE "Korean"
!insertmacro MUI_LANGUAGE "Portuguese"
!insertmacro MUI_LANGUAGE "Polish"
!insertmacro MUI_LANGUAGE "Russian"
!insertmacro MUI_LANGUAGE "SimpChinese"
!insertmacro MUI_LANGUAGE "Spanish"
!insertmacro MUI_LANGUAGE "Swedish"

!macro ExitIfRunning
	Beginning:
		FindProcDLL::FindProc "${BINARYNAME}"
		IntCmp $R0 0 OkCase
			MessageBox MB_ABORTRETRYIGNORE|MB_ICONEXCLAMATION "Create Synchronicity is running. Please close it before continuing." IDABORT AbortCase IDRETRY RetryCase
				Goto OkCase
		
	AbortCase:
		Abort
	
	RetryCase:
		Goto Beginning
	
	OkCase:
!macroend

#According to the website
#https://nsis.sourceforge.io/FindProcDLL_plug-in
#As of NSIS 2.46 the FindProcDLL plugin no longer works...

Function .onInit
	!insertmacro MUI_LANGDLL_DISPLAY
#!insertmacro ExitIfRunning
FunctionEnd

Function un.onInit
	!insertmacro MUI_UNGETLANGUAGE
#!insertmacro ExitIfRunning
FunctionEnd

Section "Installer Section" InstallSection
	SetOutPath $INSTDIR

	File "bin\Release\${BINARYNAME}"
	File "bin\Release\Release notes.txt"
	File "bin\Release\COPYING"

	SetOutPath "$INSTDIR\languages"
	File "bin\Release\languages\*.lng"
	File "bin\Release\languages\local-names.txt"

	!insertmacro MUI_STARTMENU_WRITE_BEGIN AppStartMenu
	CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
	CreateShortCut "$SMPROGRAMS\$StartMenuFolder\${PRODUCTNAME}.lnk" "$INSTDIR\${BINARYNAME}"
	CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
	!insertmacro MUI_STARTMENU_WRITE_END
	
	# add to 'programs and features'
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANY}" \
                 "DisplayName" "Create Synchronicity (With-Context-Menu)"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANY}" \
                 "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANY}" \
                 "DisplayIcon" "$\"$INSTDIR\${BINARYNAME}$\""
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANY}" \
                 "Publisher" "${COMPANY}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANY}" \
                 "InstallLocation" "$\"$INSTDIR$\""
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANY}" \
                 "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANY}" \
                 "NoRepair" 1

	WriteRegStr HKLM "${SUBREGPATH}" "InstallPath" $INSTDIR
	WriteUninstaller "$INSTDIR\Uninstall.exe"
SectionEnd

Section "Uninstall"
	Delete "$INSTDIR\${BINARYNAME}"
	Delete "$INSTDIR\Release notes.txt"
	Delete "$INSTDIR\COPYING"
	Delete "$INSTDIR\Uninstall.exe"
	Delete "$INSTDIR\app.log"

	!insertmacro MUI_STARTMENU_GETFOLDER AppStartMenu $StartMenuFolder

	Delete "$SMPROGRAMS\$StartMenuFolder\${PRODUCTNAME}.lnk"
	Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk"
	RMDir "$SMPROGRAMS\$StartMenuFolder"
	RMDir "$SMPROGRAMS\${COMPANY}\" #remove the "Create Software" folder if empty

	RMDir /r "$INSTDIR\languages\"
	RMDir /r "$INSTDIR\config\"
	RMDir /r "$INSTDIR\log\"
	RMDir "$INSTDIR\"
	RMDir "${COMPANYPATH}\" #remove the "Create Software" folder if empty

	RMDir /r "$APPDATA\${COMPANY}\${PRODUCTNAME}\"
	RMDir "$APPDATA\${COMPANY}\" #remove the "Create Software" folder if empty
	
	# remove from 'programs and features'
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${COMPANY}"

	DeleteRegKey HKLM "${SUBREGPATH}"
	DeleteRegKey /ifempty HKLM "${REGPATH}" #remove the "Create Software" key if empty
SectionEnd
