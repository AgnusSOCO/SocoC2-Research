# SharpC2 GUI Usage Guide

## Windows Setup

### Prerequisites
1. Install Required Software:
   - .NET 6.0 SDK: Download from Microsoft's website
   - Visual Studio 2022 (optional): For development
   - Git for Windows: For source control

### Installation
1. Clone Repository:
   ```powershell
   git clone https://github.com/AgnusSOCO/SocoC2-Research.git
   cd SocoC2-Research
   ```

2. Build Client:
   ```powershell
   cd Client
   dotnet restore
   dotnet build
   dotnet publish -c Release -r win-x64 --self-contained true
   ```

3. Build Drone (if needed):
   ```powershell
   cd ..\Drone
   dotnet restore
   dotnet build
   dotnet publish -c Release -r win-x64 --self-contained true
   ```

### Windows-Specific Configuration
1. Firewall Settings:
   ```powershell
   # Allow Client through firewall
   New-NetFirewallRule -DisplayName "SharpC2 Client" -Direction Inbound -Program "C:\path\to\Client.exe" -Action Allow
   
   # Allow specific ports (example for HTTP handler)
   New-NetFirewallRule -DisplayName "SharpC2 Handler" -Direction Inbound -LocalPort 443 -Protocol TCP -Action Allow
   ```

2. Antivirus Configuration:
   ```powershell
   # Add exclusion for client directory
   Add-MpPreference -ExclusionPath "C:\path\to\SharpC2"
   
   # Add process exclusion
   Add-MpPreference -ExclusionProcess "Client.exe"
   ```

3. Network Setup:
   - Import SSL certificate:
   ```powershell
   Import-PfxCertificate -FilePath "cert.pfx" -CertStoreLocation "Cert:\LocalMachine\My" -Password $securePassword
   ```
   
   - Configure hosts file for domain fronting:
   ```powershell
   Add-Content -Path "C:\Windows\System32\drivers\etc\hosts" -Value "`n127.0.0.1 your.domain.com"
   ```

### Running on Windows
1. Start Client:
   ```powershell
   cd C:\path\to\Client\bin\Release\net6.0-windows\win-x64\publish
   .\Client.exe
   ```

2. Handler Management:
   - Use Windows Task Manager to monitor connections
   - Check Windows Event Viewer for issues
   - Use netstat to verify listening ports:
   ```powershell
   netstat -ano | findstr "LISTENING"
   ```

3. Troubleshooting Windows Issues:
   - Check Windows Event Logs:
   ```powershell
   Get-EventLog -LogName Application -Source "SharpC2*"
   ```
   
   - Verify network connectivity:
   ```powershell
   Test-NetConnection -ComputerName your.domain.com -Port 443
   ```
   
   - Check certificate:
   ```powershell
   Get-ChildItem -Path Cert:\LocalMachine\My | Where-Object {$_.Subject -like "*SharpC2*"}
   ```

## Getting Started

### Initial Connection
1. Launch the SharpC2 Client application
2. In the connection dialog:
   - Enter Team Server IP address
   - Enter your nickname
   - Enter the shared password
3. Verify the certificate thumbprint matches the Team Server output
4. Click "Connect" to establish connection

### Main Interface Layout
The interface is divided into several key areas:
- Left sidebar: Navigation menu
- Main content area: Active view
- Bottom panel: Status and notifications
- Top bar: Quick actions and settings

## Handler Management

### Creating a Handler
1. Navigate to "Handlers" in the left sidebar
2. Click "New Handler" button
3. Select handler type:
   - HTTP/HTTPS
   - SMB
   - TCP
   - External
4. Configure handler settings:
   ```
   Name: my-handler
   Port: 443
   Address: your.domain.com
   ```
5. Click "Create" to start the handler

### Managing Handlers
1. View active handlers:
   - Status indicator
   - Connected drones count
   - Traffic statistics
2. Handler actions:
   - Start/Stop
   - Edit configuration
   - Generate payloads
   - View logs

## Drone Management

### Drone Overview
1. Navigate to "Drones" in left sidebar
2. View drone list:
   - Status indicators
   - Last check-in time
   - Operating system
   - Username/hostname

### Interacting with Drones
1. Select a drone from the list
2. Available actions:
   - Execute commands
   - Upload/download files
   - Take screenshots
   - Process management
   - Network operations

### Task Execution
1. Select target drone
2. Choose command type:
   - Shell commands
   - PowerShell
   - Assembly execution
   - Process injection
3. Enter command parameters
4. Monitor task status in output window

## Profile Management

### Creating C2 Profiles
1. Navigate to "Profiles" section
2. Click "New Profile"
3. Configure settings:
   ```yaml
   name: custom-profile
   jitter:
     enabled: true
     min: 10
     max: 50
   ```
4. Save and apply to handlers

### Profile Templates
1. Use built-in templates:
   - Default profile
   - Stealth profile
   - High-speed profile
2. Customize template settings
3. Save as new profile

## File Operations

### File Browser
1. Navigate to "Files" section
2. Browse remote system:
   - Directory listing
   - File properties
   - Permissions
3. File actions:
   - Upload
   - Download
   - Delete
   - Execute

### File Staging
1. Configure staged files:
   - Select handler
   - Choose file
   - Set download path
2. Monitor download status

## Advanced Features

### Process Management
1. View running processes:
   - Process details
   - Memory usage
   - Loaded modules
2. Process actions:
   - Kill process
   - Inject code
   - Dump memory

### Network Operations
1. Port forwarding:
   - Configure local port
   - Set remote target
   - Start forwarding
2. SOCKS proxy:
   - Configure proxy port
   - Start proxy server
   - Monitor connections

## Troubleshooting

### Connection Issues
1. Check status indicators:
   - Team Server connection
   - Handler status
   - Drone check-in
2. Common solutions:
   - Verify network connectivity
   - Check certificate
   - Validate credentials

### Performance Optimization
1. Adjust settings:
   - Sleep intervals
   - Jitter values
   - Chunk sizes
2. Monitor resource usage:
   - Network traffic
   - Memory usage
   - CPU utilization

## Security Best Practices
1. User Interface Security:
   - Lock session when idle
   - Use secure passwords
   - Monitor audit logs
2. Operation Security:
   - Use HTTPS handlers
   - Enable domain fronting
   - Rotate configurations
