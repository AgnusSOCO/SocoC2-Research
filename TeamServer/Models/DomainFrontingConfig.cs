using System.Collections.Generic;

namespace TeamServer.Models;

public class DomainFrontingConfig
{
    public string FrontDomain { get; set; }
    public string ActualDomain { get; set; }
    public Dictionary<string, string> CustomHeaders { get; set; } = new();
}
