REM @name: Clean Temporary Files
REM @description: Cleans temporary files from Windows Temp directory. Requires Administrator privileges.
REM @runAsAdmin: true

@echo off
echo ==============================================
echo     BAT-Manager - Clean Temp Files
echo ==============================================
echo.
echo Deleting system temporary files...
del /s /f /q "%SystemRoot%\Temp\*.*" 2>nul
echo Cleanup completed!
echo.
pause
