# parol-vs

## Workflow to develop and test the parol-vs project

```pwsh
$vsix = "C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\VSIXInstaller.exe"
dotnet build --configuration Release

& $vsix /rootSuffix:Exp /uninstall:parol_vs.88535f74-269a-4d60-b4ec-223df8f14c0a
# Erweiterungen -> Erweiterungen verwalten -> Experimentelle Instanz zurücksetzen

# Installieren der Erweiterung in der experimentellen Instanz von Visual Studio
& $vsix /rootSuffix:Exp .\bin\Release\net472\parol-vs.vsix

# Starten von Visual Studio in der experimentellen Instanz
devenv /rootsuffix Exp /log
```
