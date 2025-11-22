# Telerik Theme Overrides - Documentation

## Overview
The `telerik-overrides.css` file maps SocialManager's custom theme variables to Telerik UI for Blazor CSS variables, ensuring consistent theming across all Telerik components.

## File Structure

### **Location**
`src/SocialManager/SocialManager/wwwroot/css/telerik-overrides.css`

### **Load Order** (Critical!)
```html
1. css/theme-variables.css          ? Our custom variables
2. Telerik theme CSS                ? Telerik defaults
3. css/telerik-overrides.css        ? Our overrides (this file)
4. SocialManager.styles.css         ? Component-scoped styles
```

## Variable Mappings

### ?? **Primary Colors**

| Telerik Variable | Maps To | Value |
|------------------|---------|-------|
| `--kendo-color-primary` | `--color-primary` | `#a855f7` (Purple 500) |
| `--kendo-color-primary-hover` | `--color-primary-hover` | `#9333ea` (Purple 600) |
| `--kendo-color-primary-active` | `--color-primary-active` | `#7e22ce` (Purple 700) |
| `--kendo-color-primary-emphasis` | `--color-primary-emphasis` | `#6b21a8` (Purple 800) |

### ?? **Surface & Background**

| Telerik Variable | Maps To | Value |
|------------------|---------|-------|
| `--kendo-color-surface` | `--color-surface` | `#ffffff` |
| `--kendo-color-surface-alt` | `--color-surface-alt` | `#f9fafb` |
| `--kendo-color-app-surface` | `--color-background` | `#f3f4f6` |
| `--kendo-body-bg` | `--color-surface` | `#ffffff` |

### ?? **Typography**

| Telerik Variable | Maps To | Value |
|------------------|---------|-------|
| `--kendo-font-size` | `--font-size-base` | `1rem` (16px) |
| `--kendo-font-size-sm` | `--font-size-sm` | `0.875rem` (14px) |
| `--kendo-font-size-lg` | `--font-size-lg` | `1.125rem` (18px) |
| `--kendo-font-weight` | `--font-weight-normal` | `400` |
| `--kendo-font-weight-bold` | `--font-weight-bold` | `700` |

### ?? **Spacing**

| Telerik Variable | Maps To | Value |
|------------------|---------|-------|
| `--kendo-spacing` | `--spacing-md` | `1rem` (16px) |
| `--kendo-padding-x-sm` | `--spacing-sm` | `0.5rem` (8px) |
| `--kendo-padding-x-md` | `--spacing-md` | `1rem` (16px) |
| `--kendo-padding-x-lg` | `--spacing-lg` | `1.5rem` (24px) |

### ?? **Border Radius**

| Telerik Variable | Maps To | Value |
|------------------|---------|-------|
| `--kendo-border-radius` | `--radius-md` | `0.5rem` (8px) |
| `--kendo-border-radius-sm` | `--radius-sm` | `0.25rem` (4px) |
| `--kendo-border-radius-lg` | `--radius-lg` | `0.75rem` (12px) |

### ?? **Shadows**

| Telerik Variable | Maps To | Value |
|------------------|---------|-------|
| `--kendo-elevation-1` | `--shadow-sm` | Small shadow |
| `--kendo-elevation-2` | `--shadow-md` | Medium shadow |
| `--kendo-elevation-3` | `--shadow-lg` | Large shadow |
| `--kendo-elevation-4` | `--shadow-xl` | Extra large shadow |
| `--kendo-elevation-5` | `--shadow-2xl` | 2X large shadow |

### ?? **Status Colors**

| Status | Telerik Variable | Maps To | Value |
|--------|------------------|---------|-------|
| Success | `--kendo-color-success` | `--color-success` | `#10b981` (Green) |
| Info | `--kendo-color-info` | `--color-info` | `#3b82f6` (Blue) |
| Warning | `--kendo-color-warning` | `--color-warning` | `#f59e0b` (Amber) |
| Error | `--kendo-color-error` | `--color-error` | `#ef4444` (Red) |

## Component-Specific Overrides

### **Buttons**
```css
/* Variables */
--kendo-button-border-radius: var(--radius-md);
--kendo-button-padding-x: var(--button-padding-x-md);
--kendo-button-padding-y: var(--button-padding-y-md);

/* Classes */
.k-button-solid-primary {
    background-color: var(--color-primary);
    border-color: var(--color-primary);
    color: var(--color-text-inverse);
}
```

**Usage:**
```razor
<TelerikButton ThemeColor="@ThemeConstants.Button.ThemeColor.Primary">
    Primary Button
</TelerikButton>
```

### **Inputs**
```css
--kendo-input-border-radius: var(--input-border-radius);
--kendo-input-border: 1px solid var(--input-border-color);
--kendo-input-focus-border: 1px solid var(--input-focus-border-color);
```

**Usage:**
```razor
<TelerikTextBox @bind-Value="@name" Placeholder="Enter name" />
```

### **Grid**
```css
--kendo-grid-header-bg: var(--color-surface-alt);
--kendo-grid-alt-bg: var(--color-surface-alt);
--kendo-grid-hover-bg: var(--color-purple-50);
--kendo-grid-selected-bg: var(--color-purple-100);
```

**Alternating Rows:**
```razor
<TelerikGrid Data="@GridData">
    <GridColumns>
        <GridColumn Field="@nameof(Employee.Name)" />
    </GridColumns>
</TelerikGrid>
```

### **Editor**
```css
--kendo-editor-bg: var(--color-surface);
--kendo-editor-text: var(--color-text-primary);
--kendo-editor-toolbar-bg: var(--color-surface-alt);
```

**Usage:**
```razor
<TelerikEditor @bind-Value="@Content" Height="400px" />
```

### **Dialog/Window**
```css
--kendo-window-bg: var(--color-surface);
--kendo-window-shadow: var(--shadow-xl);
```

**Usage:**
```razor
<TelerikDialog @bind-Visible="@isVisible">
    <DialogTitle>Confirm</DialogTitle>
    <DialogContent>Are you sure?</DialogContent>
</TelerikDialog>
```

## State Classes

### **Selected State**
```css
.k-selected,
.k-state-selected {
    background-color: var(--color-purple-100);
    color: var(--color-primary-dark);
}
```

### **Focus State**
```css
.k-focus,
.k-state-focus {
    box-shadow: 0 0 0 2px var(--color-purple-200);
}
```

### **Disabled State**
```css
.k-disabled,
.k-state-disabled {
    opacity: 0.5;
    cursor: not-allowed;
}
```

### **Hover State**
```css
.k-button-flat-primary:hover {
    color: var(--color-primary-hover);
    background-color: var(--color-purple-50);
}
```

## Notification Styles

### **Success Notification**
```css
.k-notification-success {
    background-color: var(--color-success-bg);
    border-color: var(--color-success);
    color: var(--color-success);
}
```

**Usage:**
```razor
<TelerikNotification @ref="@NotificationRef" />

@code {
    TelerikNotification NotificationRef { get; set; }
    
    void ShowSuccess()
    {
        NotificationRef.Show(new NotificationModel
        {
            Text = "Success!",
            ThemeColor = ThemeConstants.Notification.ThemeColor.Success
        });
    }
}
```

### **Error Notification**
```css
.k-notification-error {
    background-color: var(--color-error-bg);
    border-color: var(--color-error);
    color: var(--color-error);
}
```

## Important Overrides

Some Telerik styles require `!important` to override deeply nested styles:

### **Primary Color Consistency**
```css
.k-primary,
.k-button-solid-primary,
.k-selected.k-primary {
    background-color: var(--color-primary) !important;
}
```

### **Focus Ring Consistency**
```css
.k-focus,
.k-input:focus,
.k-textarea:focus {
    border-color: var(--color-primary) !important;
    box-shadow: 0 0 0 2px var(--color-purple-200) !important;
}
```

## Testing Checklist

### **Buttons**
- [ ] Primary button uses `--color-primary`
- [ ] Hover state uses `--color-primary-hover`
- [ ] Active state uses `--color-primary-active`
- [ ] Flat/Link buttons use correct colors

### **Inputs**
- [ ] Border uses `--color-border-muted`
- [ ] Focus border uses `--color-primary`
- [ ] Focus ring appears in correct color
- [ ] Padding matches design system

### **Grid**
- [ ] Header background correct
- [ ] Alternating rows styled
- [ ] Hover state visible
- [ ] Selected row highlighted

### **Editor**
- [ ] Toolbar background matches
- [ ] Content area styled correctly
- [ ] Border colors consistent

### **Notifications**
- [ ] Success notification green
- [ ] Info notification blue
- [ ] Warning notification amber
- [ ] Error notification red

## Customization Examples

### **Change Primary Color**
```css
/* In theme-variables.css */
:root {
    --color-primary: #3b82f6; /* Blue instead of purple */
}

/* Telerik automatically picks up the change! */
```

### **Adjust Button Padding**
```css
/* In theme-variables.css */
:root {
    --button-padding-x-md: 1.25rem; /* More horizontal padding */
    --button-padding-y-md: 0.75rem; /* More vertical padding */
}
```

### **Change Border Radius**
```css
/* In theme-variables.css */
:root {
    --radius-md: 1rem; /* More rounded */
}
```

## Browser Compatibility

| Browser | CSS Variables | Custom Properties |
|---------|---------------|-------------------|
| Chrome 49+ | ? Yes | ? Yes |
| Firefox 31+ | ? Yes | ? Yes |
| Safari 9.1+ | ? Yes | ? Yes |
| Edge 15+ | ? Yes | ? Yes |

## Performance Notes

### **CSS Variable Cascade**
```
1. Browser defaults
2. Telerik theme defaults
3. theme-variables.css (our base)
4. telerik-overrides.css (our mappings)
5. Component-scoped styles
```

### **Optimization Tips**
- ? CSS variables are computed once at load
- ? Changes cascade automatically
- ? No JavaScript required
- ? Minimal performance impact

## Common Issues & Solutions

### **Issue: Primary color not applying**
**Solution:** Check load order in index.html
```html
<!-- Correct order -->
<link rel="stylesheet" href="css/theme-variables.css" />
<link rel="stylesheet" href="_content/Telerik.UI.for.Blazor/css/kendo-theme-default/all.css" />
<link rel="stylesheet" href="css/telerik-overrides.css" />
```

### **Issue: Focus ring wrong color**
**Solution:** Check if `--color-primary` is defined before Telerik loads

### **Issue: Component doesn't use custom colors**
**Solution:** Add explicit override with `!important` if needed

## Future Enhancements

### **Dark Mode Support**
```css
@media (prefers-color-scheme: dark) {
    :root {
        --kendo-body-bg: #1f2937;
        --kendo-body-text: #f9fafb;
        --kendo-color-surface: #1f2937;
        --kendo-color-app-surface: #111827;
    }
}
```

### **Theme Switching**
```javascript
// JavaScript to switch themes
document.documentElement.setAttribute('data-theme', 'dark');
```

```css
[data-theme="dark"] {
    --kendo-body-bg: #1f2937;
    /* ... dark theme overrides */
}
```

## Best Practices

### ? **Do**
- Use CSS variables for all theme values
- Test changes across multiple components
- Document custom overrides
- Follow the established naming convention
- Use semantic variable names

### ? **Don't**
- Hardcode color values
- Override with `!important` unless necessary
- Modify Telerik theme files directly
- Create component-specific color variables
- Use inline styles for theme colors

## Variable Reference Quick Guide

```css
/* Colors */
var(--color-primary)              /* Primary purple */
var(--color-primary-hover)        /* Hover state */
var(--color-surface)              /* White background */
var(--color-text-primary)         /* Dark text */

/* Spacing */
var(--spacing-sm)                 /* 8px */
var(--spacing-md)                 /* 16px */
var(--spacing-lg)                 /* 24px */

/* Radius */
var(--radius-sm)                  /* 4px */
var(--radius-md)                  /* 8px */
var(--radius-lg)                  /* 12px */

/* Shadows */
var(--shadow-md)                  /* Medium shadow */
var(--shadow-lg)                  /* Large shadow */
```

## Conclusion

The Telerik overrides system provides:
1. **Consistency** - All Telerik components match your theme
2. **Maintainability** - Change once, apply everywhere
3. **Flexibility** - Easy to customize and extend
4. **Performance** - CSS variables are fast and efficient
5. **Developer Experience** - Clear, semantic mappings

By mapping SocialManager theme variables to Telerik's CSS variables, we ensure a consistent look and feel across the entire application while maintaining the flexibility to easily change themes in the future.
