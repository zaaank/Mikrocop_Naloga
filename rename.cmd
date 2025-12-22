@echo off
if "%1"=="" (
    echo Usage: rename.cmd NewProjectName
    exit /b 1
)

powershell -ExecutionPolicy Bypass -File "%~dp0rename.ps1" -NewName "%1"
