using System.Xml.Linq;
using DeogenFounder.Common.Enums;

namespace DeogenFounder.Common.Utils;

public static class ChatMessageHelperUtil
{
    public static string ApplyXmlMessagePattern(string from, MessageType type, string message)
    {
        return
            $@"""<?xml version=\""1.0\"" encoding=\""utf-8\""?>\n<message>\n    <from>{from}</from>\n <type>{type.ConvertToString()}</type> \n   <content>{message}</content>\n</message>""";
    }

    public static (string Recipient, string Text) ParseXmlAnswer(string message)
    {
        var doc = XDocument.Parse(message);
        var recipient = doc.Root?.Element("recipient")?.Value;
        if (recipient == null)
        {
            throw new Exception("Can' parse recipient");
        }
        
        // var type = doc.Root?.Element("type")?.Value;
        // var msgType = type switch
        // {
        //     "statement" => MessageType.Statement,
        //     "question" => MessageType.Question,
        //     null => throw new Exception("Can' parse type"),
        //     _ => throw new Exception("Can' parse type")
        // };

        var text = doc.Root
            ?.Element("content")
            ?.Element("text")?.Value;
        
        if (text == null)
        {
            throw new Exception("Can' parse text");
        }
        
        return (recipient, text);
    }

    public static string GetContentFromXml(string message)
    {
        var doc = XDocument.Parse(message);
        var content = doc.Root?.Element("content")?.Value;
        if (content == null)
        {
            throw new Exception("Can' parse content");
        }

        return content;
    }
}
