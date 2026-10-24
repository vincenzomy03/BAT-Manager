REM @name: DNS Connection Test
REM @description: Checks network connectivity and latency to Cloudflare and Google DNS servers.

@echo off
echo ==============================================
echo       BAT-Manager - Connection Test
echo ==============================================
echo.
echo [1/2] Pinging Cloudflare DNS (1.1.1.1)...
ping -n 4 1.1.1.1
echo.
echo [2/2] Pinging Google DNS (8.8.8.8)...
ping -n 4 8.8.8.8
echo.
echo Test completed!
pause
