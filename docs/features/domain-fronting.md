# Domain Fronting Configuration Guide

## Overview
Domain fronting allows the C2 traffic to appear as if it's communicating with a different domain than the actual C2 server. This document explains how to configure and use domain fronting in SharpC2.

## Configuration
### Handler Configuration
```csharp
var handler = new HttpHandler {
    DomainFronting = new DomainFrontingConfig {
        FrontDomain = "cdn.example.com",
        ActualDomain = "c2.internal.com"
    }
}
```

### Custom Headers
```csharp
handler.CustomHeaders.Add("User-Agent", "Mozilla/5.0...");
handler.CustomHeaders.Add("Accept", "text/html,application/json...");
```

## Security Considerations
- Ensure front domain is a legitimate CDN
- Rotate domains periodically
- Monitor for blocking/detection
- Use realistic headers

## Troubleshooting
1. Connection Issues
   - Verify CDN configuration
   - Check SSL certificate
   - Validate DNS resolution

2. Detection Prevention
   - Use common user agents
   - Match header ordering
   - Implement jitter
