using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProvider.Types;

public class TextMessage
{
    public required string Content { get; init; }
    public required string ContentHtml { get; init; }

    public TextMessageModifier? Modifier { get; init; }
}

public enum TextMessageModifier
{
    FirstChat
}
