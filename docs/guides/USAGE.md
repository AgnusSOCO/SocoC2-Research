# SharpC2 Usage Guide

## Team Server Setup

### Basic Setup
1. Start Team Server:
```bash
cd TeamServer/bin/Release/net6.0/linux-x64/publish/
./TeamServer <ip> <password>
```

2. Note the certificate thumbprint displayed on startup

### Handler Configuration
1. Create HTTP Handler:
```json
{
    "name": "http-1",
    "bindPort": 443,
    "connectAddress": "your.domain.com",
    "secure": true
}
```

2. Configure Domain Fronting (Optional):
```json
{
    "frontDomain": "cdn.example.com",
    "actualDomain": "c2.internal.com",
    "customHeaders": {
        "Host": "cdn.example.com",
        "User-Agent": "Mozilla/5.0..."
    }
}
```

## Client Usage

### Connection
1. Launch Client application
2. Enter Team Server IP and password
3. Verify certificate thumbprint matches

### Handler Management
1. Create Handlers:
   - Select handler type (HTTP/SMB/TCP)
   - Configure settings
   - Start handler

2. Generate Payloads:
   - Select handler
   - Choose payload format
   - Configure options

### Drone Management
1. Monitor Connections:
   - View active drones
   - Check connection status
   - Monitor tasks

2. Task Execution:
   - Select drone
   - Choose command
   - View output

## Advanced Features

### Malleable Profiles
1. Create Profile:
```yaml
profile:
  name: custom-1
  jitter:
    enabled: true
    minValue: 10
    maxValue: 50
  sleepTechnique:
    type: Obfuscated
    enableSleepMask: true
```

2. Apply Profile:
   - Select handler
   - Load profile
   - Update configuration

### Memory Obfuscation
1. Enable in Drone configuration:
```json
{
    "memoryObfuscation": true,
    "sleepMask": true
}
```

## Security Considerations
1. Network Security
   - Use HTTPS
   - Configure firewalls
   - Monitor traffic

2. Operational Security
   - Rotate configurations
   - Monitor for detection
   - Use domain fronting

## Troubleshooting
1. Connection Issues
   - Verify network connectivity
   - Check certificate
   - Validate handler configuration

2. Performance Issues
   - Adjust sleep intervals
   - Configure chunking
   - Monitor resource usage
