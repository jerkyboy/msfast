@echo off
"C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe" "\Development\MSFast\src\Build\MySpace.MSFast.Automation.proj" /t:deploy_web_MSFastAutomation /p:Configuration=Release