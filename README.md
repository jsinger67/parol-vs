# Parol Language Support for Visual Studio 2022/2026

<!-- markdownlint-disable Inline HTML -->
<br>
<img src="https://github.com/jsinger67/parol-vs/raw/main/Resources/ParolVS300x300.png?raw=true" alt="Logo" height=300 width=300>
<br><br><br>
<!-- markdownlint-enable Inline HTML -->


This is a Visual Studio extension to support [`parol`](https://github.com/jsinger67/parol.git)'s syntax
of grammar description files (.par files).

>Please note that this extension is still in an early stage of development and not yet available in the Visual Studio Marketplace.

## Features

The extension supports syntax highlighting, document outline, goto definition, hover, rename.
It relies on the [parol language server](https://github.com/jsinger67/parol/tree/main/crates/parol-ls) for the best support. See below for instructions.

## Installation of the Visual Studio extension

### Visual Studio Marketplace

Not yet available in the Visual Studio Marketplace.

### Prebuilt binaries

Download the latest release from the [releases page](https://github.com/jsinger67/parol-vs/releases).
Alternatively you can download the built extension from build artifacts of the latest run of the [GitHub Actions](https://github.com/jsinger67/parol-vs/actions/workflows/build.yml).

## Installation of the parol language server

### Prebuilt binaries

Download the latest prebuilt binaries from the [releases page](https://github.com/jsinger67/parol/releases).

### Build from source

```pwsh
git clone https://github.com/jsinger67/parol.git
cd parol
cargo install --path crates/parol-ls
```

### Install with cargo

```pwsh
cargo install parol-ls
```



## Language Client (LSP)

The extension uses the [Language Server Protocol](https://microsoft.github.io/language-server-protocol/) to communicate with the parol language server.
It is registered for the content type `parol` (.par files) and starts the server via stdio.

Use the following configuration in Visual Studio to specify the path to the parol language server and its arguments:

Configuration via environment variables:

- `PAROL_LS_PATH` (optional, default: `parol-ls`)
- `PAROL_LS_ARGS` (optional, default: `--stdio`)

## Changes regarding the Language Server can be viewed here:

- [CHANGELOG parol-ls](https://github.com/jsinger67/parol/blob/main/crates/parol-ls/CHANGELOG.md)

<!-- markdownlint-disable Inline HTML -->
<br>
<img src="https://github.com/jsinger67/parol-vs/raw/main/Resources/Preview.png?raw=true" alt="Example" height=800 width=800>
<br><br><br>
<!-- markdownlint-enable Inline HTML -->

Please check regularly for updates.

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
