# Traffic Chunking Guide

## Overview
Traffic chunking helps evade detection by splitting large payloads into smaller chunks with variable delays.

## Configuration
### Profile Settings
```yaml
chunking:
  enabled: true
  minChunkSize: 512
  maxChunkSize: 4096
  minDelay: 10
  maxDelay: 100
```

### Implementation Details
- Random chunk sizes
- Variable delays between chunks
- Automatic reassembly
- Header-based tracking

## Best Practices
1. Size Selection
   - Match target application patterns
   - Avoid consistent sizes
   - Use realistic ranges

2. Timing Configuration
   - Implement natural delays
   - Add jitter to intervals
   - Match network conditions

## Troubleshooting
1. Performance Issues
   - Adjust chunk sizes
   - Tune delay intervals
   - Monitor reassembly

2. Detection Avoidance
   - Vary chunk patterns
   - Randomize delays
   - Match legitimate traffic
