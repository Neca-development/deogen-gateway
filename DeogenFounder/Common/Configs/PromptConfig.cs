namespace DeogenFounder.Common.Configs;

public class PromptConfig
{
    public PromptItem[] Items { get; set; } = [];
}

public class PromptItem
{
    public string Name { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
}
