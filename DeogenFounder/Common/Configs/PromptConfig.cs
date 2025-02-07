namespace DeogenFounder.Common.Configs;

public class PromptConfig
{
    public PromptItem[] Items { get; set; } = [];
}

public class PromptItem
{
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
