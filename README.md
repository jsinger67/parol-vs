# Parol Language Support for Visual Studio 2022/2026

<!-- markdownlint-disable Inline HTML -->
<br>
<img src="https://github.com/jsinger67/parol-vs/raw/main/Resources/ParolVS300x300.png?raw=true" alt="Logo" height=300 with=300>
<br><br><br>
<!-- markdownlint-enable Inline HTML -->


This is a Visual Studio extension to support [`parol`](https://github.com/jsinger67/parol.git)'s syntax
of grammar description files (.par files).

## Features

The extension supports syntax highlighting, document outline, goto definition, hover, rename.
It relies on the [parol language server](https://github.com/jsinger67/parol/tree/main/crates/parol-ls) for the best support. See below for instructions.

## Installation


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

## Language Client (LSP)

The extension uses the [Language Server Protocol](https://microsoft.github.io/language-server-protocol/) to communicate with the parol language server.
It is registered for the content type `parol` and starts the server via stdio.

Use the following configuration in Visual Studio to specify the path to the parol language server and its arguments:

Configuration via environment variables:

- `PAROL_LS_PATH` (optional, default: `parol-ls`)
- `PAROL_LS_ARGS` (optional, default: `--stdio`)

## Changes regarding the Language Server can be viewed here:

- [CHANGELOG parol-ls](https://github.com/jsinger67/parol/blob/main/crates/parol-ls/CHANGELOG.md)

<!-- markdownlint-disable Inline HTML -->
<br>
<img src="https://github.com/jsinger67/parol-vs/raw/main/Resources/Preview.png?raw=true" alt="Example">
<br><br><br>
<!-- markdownlint-enable Inline HTML -->

Please check regularly for updates.
