@echo off
dotnet publish -c Release -r win-x64 --self-contained true ^
 /p:PublishSingleFile=true ^
 /p:EnableCompressionInSingleFile=true ^
 /p:IncludeNativeLibrariesForSelfExtract=true ^
 /p:DebugType=none ^
 /p:OptimizationPreference=Speed ^
 /p:TieredCompilation=true ^
 /p:UseWindowsForms=true
pause
