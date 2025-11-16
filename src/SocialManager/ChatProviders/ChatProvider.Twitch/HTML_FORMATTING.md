# Twitch Chat Provider - HTML Formatting

This document describes the HTML formatting features of the Twitch Chat Provider.

## Overview

The `TwitchChatProvider` generates HTML-formatted messages that include:
- **Badges** - Subscriber, moderator, VIP, broadcaster badges, etc.
- **Emotes** - Twitch emotes rendered as images
- **Mentions** - @username mentions highlighted with special styling

## HTML Structure

### Badges
Badges are rendered at the beginning of the message:

```html
<span class="twitch-badges">
    <span class="twitch-badge twitch-badge-moderator" data-version="1" title="moderator"></span>
    <span class="twitch-badge twitch-badge-subscriber" data-version="12" title="subscriber"></span>
</span>
```

### Emotes
Emotes are replaced with `<img>` tags:

```html
<img src="https://static-cdn.jtvnw.net/emoticons/v2/{emote-id}/default/dark/1.0" 
     alt="Kappa" 
     title="Kappa" 
     class="twitch-emote" />
```

### Mentions
@mentions are wrapped in styled spans:

```html
<span class="twitch-mention">@username</span>
```

## CSS Styling

Include the `TwitchChatStyles.css` file in your project to get default styling for:
- Badge display and positioning
- Emote sizing and alignment
- Mention highlighting with hover effects
- Dark/light theme support

### Custom Styling

You can override the default styles by targeting these CSS classes:

```css
.twitch-badges { /* Badge container */ }
.twitch-badge { /* Individual badge */ }
.twitch-badge-{badge-name} { /* Specific badge type */ }
.twitch-emote { /* Emote image */ }
.twitch-mention { /* Mention highlight */ }
```

## Badge Images

The CSS file includes default badge URLs for common badge types:
- `moderator`
- `subscriber`
- `vip`
- `broadcaster`
- `partner`
- `turbo`
- `premium`

For channel-specific badge images (like custom subscriber badges), you'll need to fetch them from the Twitch API and update the CSS accordingly.

## Example Output

A message like:
```
Hello @viewer! Check out this emote Kappa
```

From a moderator subscriber would be rendered as:

```html
<span class="twitch-badges">
    <span class="twitch-badge twitch-badge-moderator" data-version="1" title="moderator"></span>
    <span class="twitch-badge twitch-badge-subscriber" data-version="6" title="subscriber"></span>
</span> Hello <span class="twitch-mention">@viewer</span>! Check out this emote <img src="https://static-cdn.jtvnw.net/emoticons/v2/25/default/dark/1.0" alt="Kappa" title="Kappa" class="twitch-emote" />
```

## Usage in Blazor

To use the HTML formatted messages in a Blazor component:

```razor
@((MarkupString)chatMessage.ContentHtml)
```

Don't forget to include the CSS file in your app or component:

```html
<link rel="stylesheet" href="TwitchChatStyles.css" />
```

## Notes

- Emote positions are provided by TwitchLib and are accurate
- Mention detection is simple (words starting with @) and may need refinement for your use case
- Badge images use Twitch's CDN URLs
- The HTML is safe (all user content is HTML-encoded before processing)
