REM @name: System Information
REM @description: Displays details about Windows version, CPU, and physical memory.

@echo off
echo ==============================================
echo        BAT-Manager - System Info
echo ==============================================
echo Date and Time: %date% %time%
echo User:          %username%
echo Computer:      %computername%
echo.
systeminfo | findstr /B /C:"OS Name" /C:"OS Version" /C:"System Type" /C:"Total Physical Memory"
echo.
pause
