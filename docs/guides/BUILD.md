# Building SharpC2 from Source

## Prerequisites
- .NET 6.0 SDK or later
- Linux environment for Team Server
- Windows environment for Client (optional)
- Git

## Environment Setup
1. Install .NET SDK:
```bash
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y dotnet-sdk-6.0
```

2. Clone Repository:
```bash
git clone https://github.com/AgnusSOCO/SocoC2-Research.git
cd SocoC2-Research
```

## Building Components

### Team Server
```bash
cd TeamServer
dotnet restore
dotnet build
dotnet publish -c Release -r linux-x64 --self-contained true
```

### Client
```bash
cd Client
dotnet restore
dotnet build
dotnet publish -c Release -r win-x64 --self-contained true
```

### Drone
```bash
cd Drone
dotnet restore
dotnet build
dotnet publish -c Release -r win-x64 --self-contained true
```

### Stagers
```bash
cd Stagers/ExeStager
dotnet restore
dotnet build
dotnet publish -c Release -r win-x64 --self-contained true
```

## Build Artifacts
After successful builds, find the compiled binaries in:
- TeamServer: `TeamServer/bin/Release/net6.0/linux-x64/publish/`
- Client: `Client/bin/Release/net6.0-windows10.0.19041.0/win-x64/publish/`
- Drone: `Drone/bin/Release/net48/win-x64/publish/`
- Stagers: `Stagers/ExeStager/bin/Release/net48/win-x64/publish/`

## Common Issues
1. Missing Dependencies
   - Run `dotnet restore` in each project directory
   - Verify .NET SDK version
   - Check for platform-specific dependencies

2. Build Errors
   - Clean solution: `dotnet clean`
   - Delete bin/obj folders
   - Rebuild: `dotnet build`
