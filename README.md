# CZ4R Admin Client

Cross-platform desktop admin client for the CZ4R field-service job/worker management system, built with Avalonia UI.

## Build

```bash
dotnet build -c Release
```

## Publish

```bash
# Linux (single-file executable)
dotnet publish App -c Release -r linux-x64

# Windows (single-file executable)
dotnet publish App -c Release -r win-x64
```

Output in `App/bin/Release/net10.0/<rid>/publish/`.