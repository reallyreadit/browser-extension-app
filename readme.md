# Browser Extension App
The command line application that is called by browser extensions on Windows and Linux.
## Building
Production build command:

    dotnet publish -c Release -r win-x64 -p:PublishTrimmed=true -p:PublishSingleFile=true
## Distributing
The single file executable is bundled with the Electron app during the packaging stage.