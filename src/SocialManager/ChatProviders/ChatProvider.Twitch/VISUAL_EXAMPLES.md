# Twitch Chat HTML Formatting - Visual Examples

This document shows visual examples of the HTML formatting output.

## Example 1: Simple Message
**Input:** "Hello world!"
**Output:**
```html
Hello world!
```
**Visual:** Hello world!

---

## Example 2: Message with Badge (Moderator)
**Input:** "Welcome everyone!" (from a moderator)
**Output:**
```html
<span class="twitch-badges">
    <span class="twitch-badge twitch-badge-moderator" data-version="1" title="moderator"></span>
</span> Welcome everyone!
```
**Visual:** [???] Welcome everyone!

---

## Example 3: Message with Multiple Badges
**Input:** "Thanks for subscribing!" (from a moderator subscriber)
**Output:**
```html
<span class="twitch-badges">
    <span class="twitch-badge twitch-badge-moderator" data-version="1" title="moderator"></span>
    <span class="twitch-badge twitch-badge-subscriber" data-version="12" title="subscriber"></span>
</span> Thanks for subscribing!
```
**Visual:** [???][?] Thanks for subscribing!

---

## Example 4: Message with Emote
**Input:** "Great stream Kappa"
**Output:**
```html
Great stream <img src="https://static-cdn.jtvnw.net/emoticons/v2/25/default/dark/1.0" alt="Kappa" title="Kappa" class="twitch-emote" />
```
**Visual:** Great stream ![Kappa emote]

---

## Example 5: Message with Mention
**Input:** "Hey @streamer, love your content!"
**Output:**
```html
Hey <span class="twitch-mention">@streamer</span>, love your content!
```
**Visual:** Hey **@streamer**, love your content!

---

## Example 6: Complex Message (Everything Combined)
**Input:** "Hi @viewer! Welcome Kappa PogChamp" (from a VIP subscriber)
**Output:**
```html
<span class="twitch-badges">
    <span class="twitch-badge twitch-badge-vip" data-version="1" title="vip"></span>
    <span class="twitch-badge twitch-badge-subscriber" data-version="6" title="subscriber"></span>
</span> Hi <span class="twitch-mention">@viewer</span>! Welcome <img src="https://static-cdn.jtvnw.net/emoticons/v2/25/default/dark/1.0" alt="Kappa" title="Kappa" class="twitch-emote" /> <img src="https://static-cdn.jtvnw.net/emoticons/v2/88/default/dark/1.0" alt="PogChamp" title="PogChamp" class="twitch-emote" />
```
**Visual:** [??][?] Hi **@viewer**! Welcome ![Kappa] ![PogChamp]

---

## Example 7: Message with Mention and Punctuation
**Input:** "Thanks @user!"
**Output:**
```html
Thanks <span class="twitch-mention">@user</span>!
```
**Visual:** Thanks **@user**!

---

## Example 8: Broadcaster Message
**Input:** "Going live soon!" (from the broadcaster)
**Output:**
```html
<span class="twitch-badges">
    <span class="twitch-badge twitch-badge-broadcaster" data-version="1" title="broadcaster"></span>
</span> Going live soon!
```
**Visual:** [??] Going live soon!

---

## Styled Examples

When the CSS is applied, the messages will look like:

### With Dark Theme:
```
[??? Moderator Badge] [? Subscriber Badge] Hey @viewer (purple highlight), check this Kappa (emote image) out!
```

### With Light Theme:
```
[??? Moderator Badge] [? Subscriber Badge] Hey @viewer (darker purple highlight), check this Kappa (emote image) out!
```

---

## Badge Icons Reference

| Badge Type | Visual Representation |
|------------|----------------------|
| Broadcaster | ?? |
| Moderator | ??? |
| VIP | ?? |
| Subscriber | ? |
| Partner | ? |
| Staff | ?? |
| Admin | ?? |
| Turbo | ? |
| Premium | ?? |

---

## CSS Styling Effects

### Badges
- Displayed inline at the start of the message
- 18x18 pixels in size
- 4px gap between badges
- 4px margin after badge group

### Emotes
- Displayed inline with text
- 28px height (width auto-scales)
- 2px margin on sides
- Pixelated rendering for retro look

### Mentions
- Purple background highlight (rgba overlay)
- Bold text
- Rounded corners (3px radius)
- Hover effect (darker background)
- Smooth transition (0.2s)

---

## Real-World Example

A typical chat conversation might look like:

```
[???] ModUser: Welcome to the stream!
[?] SubUser: Thanks @ModUser! First time here
RegularUser: This is awesome Kappa
[??] Streamer: Thanks everyone PogChamp Let's get started!
[???][?] VIPMod: @Streamer ready when you are!
```

Each line would be styled according to the CSS with badges as small icons, mentions highlighted, and emotes as images.
