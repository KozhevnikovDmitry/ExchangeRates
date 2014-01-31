@echo off

set RD=rd /s/q
for /f %%i in ('dir /Ad /s/b') do %RD% "%%i\obj" & %RD% "%%i\bin"
