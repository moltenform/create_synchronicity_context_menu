@echo OFF

if /i "%~1"=="/?" goto help
if /i "%~1"=="/h" goto help
if /i "%~1"=="/help" goto help
if /i "%~1"=="-?" goto help
if /i "%~1"=="-h" goto help
if /i "%~1"=="-help" goto help

set TAG=2000
set CHECKSUMS=CHECKSUMS
set PREVCD=%CD%
set changeFontFromVerdana=false
set updateRevisionNumber=false
set buildzipfiles=false
set buildlinux=false

goto start

:help
echo This file is part of Create Synchronicity.
echo Create Synchronicity is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
echo Create Synchronicity is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
echo You should have received a copy of the GNU General Public License along with Create Synchronicity.  If not, see http://www.gnu.org/licenses/.
echo Created by:   Clément Pit--Claudel.
echo Web site:     http://synchronicity.sourceforge.net.
echo.
echo Usage: build.bat
echo This script builds all versions of Create Synchronicity.
echo.
echo Note: you should first modify the script to specify paths to 7z, devenv, etc!
goto end

:start

if not exist "..\Create Synchronicity.sln" (
	echo not found: "..\Create Synchronicity.sln"
    goto end
)

if exist "%ROOT%\build" (
	echo build directory exists, please delete: "%ROOT%\build"
    goto end
)

cd ..\..
set ROOT=%CD%
set BUILD=%ROOT%\build
set BIN=%ROOT%\bin

set NAME=Create Synchronicity
set FILENAME=Create_Synchronicity
set LOG="%BUILD%\buildlog-%TAG%.txt"

mkdir "%BUILD%"

set path7z="C:\data\l3\software\FisherAppsFull\Utils\7zip\x64\7z.exe"
if not exist "%path7z%" (
	echo not found: %path7z%
    goto end
 )
 
set pathDevenv="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe"
if not exist %pathDevenv% (
	echo not found: pathDevenv
    goto end
 )
 
set pathMd5sum="C:\data\l4a\software_4a\cmder\vendor\git-for-windows\usr\bin\md5sum.exe"
if not exist %pathMd5sum% (
	echo not found: pathMd5sum
    goto end
 )
 
set pathSed="C:\data\l4a\software_4a\cmder\vendor\git-for-windows\usr\bin\sed.exe"
if not exist %pathSed% (
	echo not found: pathSed
    goto end
 )
 
set pathMakensis="C:\data\l3\software\FisherAppsFull\Coding\nsis-3.03\makensis.exe"
if not exist %pathMakensis% (
	echo not found: pathMakensis
    goto end
 )

(echo Packaging log for %TAG% & date /t & time /t & echo.) > %LOG%

rem Visual Studio doesn't let you define a common font, but some users do not have Verdana. Always build using a common font for all forms, and make sure that it's supported.
if not %changeFontFromVerdana%=="true" goto afterchangeFontFromVerdana1
echo (**) Changing "Verdana" to Main.LargeFont in interface files.
(
	echo.
	echo -----
	
	for /R "CreateSynchronicity\Interface" %%f IN (*.vb) do (
		copy "%%f" "%%f.bak"
		"%pathSed%" -i "s/Me.Font = .*/Me.Font = Main.LargeFont/" "%%f"
	)
) >> %LOG%
:afterchangeFontFromVerdana1


if not %updateRevisionNumber%=="true" goto afterupdateRevisionNumber1
echo (**) Updating revision number
(
	echo.
	echo -----
	
	cd "%ROOT%\CreateSynchronicity"
	subwcrev.exe "%ROOT%" Revision.template.vb Revision.vb
	cd "%ROOT%"
	
	echo.
	echo -----
) >> %LOG%
:afterupdateRevisionNumber1

echo (**) Building program (release)
%pathDevenv% "%ROOT%\CreateSynchronicity\Create Synchronicity.sln" /build Release /Out %LOG%

echo (**) Building program (debug)
%pathDevenv% "%ROOT%\CreateSynchronicity\Create Synchronicity.sln" /build Debug /Out %LOG%


if not %buildlinux%=="true" goto afterbuildlinux
echo (**) Building program (Linux)
%pathDevenv% "%ROOT%\CreateSynchronicity\Create Synchronicity.sln" /build Linux /Out %LOG%
:afterbuildlinux

echo (**) Building installer
(
	echo.
	echo -----
	
	%pathMakensis% "%ROOT%\CreateSynchronicity\setup_script.nsi"
		
	echo.
	echo -----
	
	move %FILENAME%_Setup.exe "%BUILD%\%FILENAME%_Setup-%TAG%.exe"
) >> %LOG%

if not %buildzipfiles%=="true" goto afterbuildzipfiles
echo (**) Building zip files
(
	echo.
	echo -----
	
	cd "CreateSynchronicity\%BIN%\Release"
	%path7z% a "%BUILD%\%FILENAME%-%TAG%.zip" "%NAME%.exe" "Release notes.txt" "COPYING" "languages\*"
	%path7z% a "%BUILD%\%FILENAME%-%TAG%-Extensions.zip" "compress.dll" "ICSharpCode.SharpZipLib.dll"
	%path7z% a "%BUILD%\%FILENAME%-%TAG%-Scripts.zip" "scripts\*"
	copy "Release notes.txt" "%BUILD%\release-notes.txt" 
	cd "%ROOT%"

	cd "CreateSynchronicity\%BIN%\Debug"
	%path7z% a "%BUILD%\%FILENAME%-%TAG%-DEBUG.zip" "%NAME%.exe" "Release notes.txt" "COPYING" "languages\*"
	cd "%ROOT%"

	cd "CreateSynchronicity\%BIN%\Linux"
	%path7z% a "%BUILD%\%FILENAME%-%TAG%-Linux.zip" "%NAME%.exe" "Release notes.txt" "run.sh" "COPYING" "languages\*"
	cd "%ROOT%"
) >> %LOG%

echo (**) Computing checksums
(
	echo.
	echo -----
	echo Updating %CHECKSUMS%
	
	cd %BUILD%
	%pathMd5sum% "%FILENAME%-%TAG%.zip" "%FILENAME%-%TAG%-DEBUG.zip" "%FILENAME%_Setup-%TAG%.exe" "%FILENAME%-%TAG%-Linux.zip" "%FILENAME%-%TAG%-Extensions.zip" "%FILENAME%-%TAG%-Scripts.zip" >> %CHECKSUMS%
	cd %ROOT%
) >> %LOG%
:afterbuildzipfiles

if not %changeFontFromVerdana%=="true" goto afterchangeFontFromVerdana2
echo (**) Changing font name back from Main.LargeFont to "Verdana" in interface files.
(
	echo.
	echo -----
	
	for /R "Create Synchronicity\Interface" %%f IN (*.vb) do move /Y "%%f.bak" "%%f"
) >> %LOG%
:afterchangeFontFromVerdana2

goto end
:end
cd "%PREVCD%"
