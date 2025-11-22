# Semantic CSS Guidelines - SocialManager Project

## Overview
This document provides comprehensive guidelines for writing semantic CSS in the SocialManager project, ensuring consistency, maintainability, and adherence to best practices.

## Core Principles

### 1. Semantic Class Names
Use class names that describe **what** the element is, not **how** it looks.

? **Good:**
```css
.dashboard-header { }
.stat-card { }
.post-list-item { }
.platform-badge { }
```

? **Bad:**
```css
.purple-box { }
.big-text { }
.left-section { }
.mt-4 { }
```

### 2. Theme Variables
Always use theme variables from `theme-variables.css` for all styling values.

? **Good:**
```css
.stat-card {
    padding: var(--spacing-lg);
    background-color: var(--color-surface);
    border-radius: var(--radius-lg);
    box-shadow: var(--shadow-md);
}
```

? **Bad:**
```css
.stat-card {
    padding: 24px;
    background-color: #ffffff;
    border-radius: 12px;
    box-shadow: 0 4px 6px rgba(0,0,0,0.1);
}
```

### 3. Isolated CSS
Every Razor component should have its own `.razor.css` file with scoped styles.

**File structure:**
```
src/SocialManager/SocialManager/
??? Pages/
?   ??? Home.razor
?   ??? Home.razor.css          ?
?   ??? Blog/
?       ??? EntryList.razor
?       ??? EntryList.razor.css ?
?       ??? PostEditor.razor
?       ??? PostEditor.razor.css ?
??? Layout/
    ??? MainLayout.razor
    ??? MainLayout.razor.css    ?
    ??? NavigationSidebar.razor
    ??? NavigationSidebar.razor.css ?
```

## Naming Conventions

### Component-Element Pattern
```
.{component}-{element}[-{modifier}]
```

**Examples:**
```css
/* Dashboard Component */
.dashboard-container { }
.dashboard-header { }
.dashboard-stats-grid { }

/* Stat Card Component */
.stat-card { }
.stat-card-header { }
.stat-card-content { }
.stat-label { }
.stat-value { }
.stat-footer { }

/* Post List Component */
.post-list { }
.post-list-item { }
.post-item-header { }
.post-item-title { }
.post-item-meta { }
```

### State Modifiers
```
.{component}.{state}
.{component}-{element}.{state}
```

**Examples:**
```css
.post-list-item.selected { }
.stat-card.highlighted { }
.platform-icon.enabled { }
.platform-icon.disabled { }
```

### Container Hierarchy
```
.{page}-{section}-{container}
```

**Examples:**
```css
.dashboard-stats-grid { }
.dashboard-recent-posts { }
.dashboard-quick-actions { }
.editor-blog-content { }
.editor-social-column { }
```

## File Organization

### CSS File Structure
```css
/* ============================================
   Component Name - Isolated Styles
   ============================================
   Description of component and its purpose
   ============================================ */

/* ============================================
   Main Container
   ============================================ */
.component-container {
    /* Primary container styles */
}

/* ============================================
   Sub-Section Name
   ============================================ */
.section-header {
    /* Styles for this section */
}

.section-content {
    /* More styles */
}

/* ============================================
   Responsive Adjustments
   ============================================ */
@media (max-width: 767px) {
    /* Mobile styles */
}

@media (min-width: 768px) and (max-width: 1023px) {
    /* Tablet styles */
}

@media (min-width: 1024px) {
    /* Desktop styles */
}
```

### Header Template
Every CSS file should start with:
```css
/* ============================================
   [Component Name] - Isolated Styles
   ============================================
   Semantic CSS classes for [Component] component
   using theme variables for consistency
   ============================================ */
```

## Theme Variable Usage

### Color Variables

| Category | Variable | Usage |
|----------|----------|-------|
| **Primary Colors** | `--color-primary` | Primary brand color |
| | `--color-primary-hover` | Hover states |
| | `--color-primary-active` | Active/pressed states |
| **Text** | `--color-text-primary` | Main text color |
| | `--color-text-secondary` | Secondary/muted text |
| | `--color-text-inverse` | White text on dark backgrounds |
| **Surfaces** | `--color-surface` | Card/panel backgrounds |
| | `--color-surface-alt` | Alternate surface color |
| | `--color-background` | Page background |
| **Borders** | `--color-border-subtle` | Light borders |
| | `--color-border-muted` | Medium borders |
| **Status** | `--color-success` | Success states |
| | `--color-error` | Error states |
| | `--color-warning` | Warning states |
| | `--color-info` | Info states |

### Spacing Variables

```css
--spacing-xs:   0.25rem;  /* 4px  */
--spacing-sm:   0.5rem;   /* 8px  */
--spacing-md:   1rem;     /* 16px */
--spacing-lg:   1.5rem;   /* 24px */
--spacing-xl:   2rem;     /* 32px */
--spacing-2xl:  3rem;     /* 48px */
--spacing-3xl:  4rem;     /* 64px */
```

**Usage:**
```css
.component {
    padding: var(--spacing-lg);
    gap: var(--spacing-md);
    margin-bottom: var(--spacing-xl);
}
```

### Typography Variables

```css
/* Font Sizes */
--font-size-xs:   0.75rem;    /* 12px */
--font-size-sm:   0.875rem;   /* 14px */
--font-size-base: 1rem;       /* 16px */
--font-size-lg:   1.125rem;   /* 18px */
--font-size-xl:   1.25rem;    /* 20px */
--font-size-2xl:  1.5rem;     /* 24px */
--font-size-3xl:  1.875rem;   /* 30px */

/* Font Weights */
--font-weight-normal:    400;
--font-weight-medium:    500;
--font-weight-semibold:  600;
--font-weight-bold:      700;

/* Line Heights */
--line-height-tight:    1.25;
--line-height-normal:   1.5;
--line-height-relaxed:  1.75;
```

### Border Radius Variables

```css
--radius-sm:   0.25rem;  /* 4px  */
--radius-md:   0.5rem;   /* 8px  */
--radius-lg:   0.75rem;  /* 12px */
--radius-xl:   1rem;     /* 16px */
--radius-2xl:  1.5rem;   /* 24px */
--radius-full: 9999px;   /* Fully rounded */
```

### Shadow Variables

```css
--shadow-sm:  /* Small shadow */
--shadow-md:  /* Medium shadow */
--shadow-lg:  /* Large shadow */
--shadow-xl:  /* Extra large shadow */
--shadow-2xl: /* 2X large shadow */
```

### Transition Variables

```css
--transition-fast:   0.15s ease;
--transition-base:   0.2s ease;
--transition-slow:   0.3s ease;
--transition-slower: 0.5s ease;
```

## Utility Classes vs Semantic Classes

### When to Use Utility Classes

? **Use Kendo utility classes for:**
- **Layout**: `k-d-flex`, `k-d-grid`, `k-flex-col`, `k-flex-row`
- **Alignment**: `k-justify-content-*`, `k-align-items-*`
- **Spacing (one-off)**: `k-gap-*`, `k-p-*`, `k-m-*`
- **Sizing**: `k-h-*`, `k-w-*`, `k-min-h-*`
- **Overflow**: `k-overflow-*`

**Example:**
```razor
<div class="dashboard-header k-d-flex k-justify-content-between k-align-items-center">
     ? Semantic          ? Layout utilities
</div>
```

### When to Use Semantic Classes

? **Use semantic classes for:**
- **Component identity**: What the element IS
- **Content styling**: Colors, typography, shadows
- **Component-specific layout**: Specific to this component
- **State variations**: Selected, active, disabled

**Example:**
```razor
<div class="stat-card">
     ? Semantic - describes what it is
    <p class="stat-label">Total Posts</p>
        ? Semantic - describes the content
    <h3 class="stat-value">42</h3>
         ? Semantic - describes the data
</div>
```

```css
.stat-card {
    background-color: var(--color-surface);
    border-radius: var(--radius-lg);
    box-shadow: var(--shadow-md);
}

.stat-label {
    font-size: var(--font-size-sm);
    color: var(--color-text-secondary);
}

.stat-value {
    font-size: var(--font-size-2xl);
    font-weight: var(--font-weight-bold);
}
```

## Common Patterns

### Card Components
```css
.card-component {
    background-color: var(--color-surface);
    border-radius: var(--radius-lg);
    box-shadow: var(--shadow-md);
    padding: var(--spacing-lg);
    transition: box-shadow var(--transition-base);
}

.card-component:hover {
    box-shadow: var(--shadow-lg);
}

.card-header {
    margin-bottom: var(--spacing-lg);
}

.card-title {
    font-size: var(--font-size-xl);
    font-weight: var(--font-weight-semibold);
    margin: 0;
}

.card-content {
    /* Content styles */
}
```

### List Items
```css
.list-item {
    border: 1px solid var(--color-border-subtle);
    border-radius: var(--radius-md);
    padding: var(--spacing-md);
    cursor: pointer;
    transition: all var(--transition-base);
}

.list-item:hover {
    background-color: var(--color-purple-50);
    border-color: var(--color-border-muted);
    box-shadow: var(--shadow-sm);
}

.list-item.selected {
    background-color: var(--color-purple-100);
    border-color: var(--color-primary);
}

.list-item-title {
    font-weight: var(--font-weight-semibold);
    margin: 0 0 var(--spacing-sm) 0;
}

.list-item-meta {
    font-size: var(--font-size-sm);
    color: var(--color-text-secondary);
}
```

### Page Headers
```css
.page-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: var(--spacing-lg);
}

.page-title {
    font-size: var(--font-size-2xl);
    font-weight: var(--font-weight-bold);
    margin: 0;
}

.page-description {
    font-size: var(--font-size-sm);
    color: var(--color-text-secondary);
    margin: var(--spacing-xs) 0 0 0;
}
```

### Empty States
```css
.empty-state {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    gap: var(--spacing-md);
    padding: var(--spacing-3xl) var(--spacing-lg);
    text-align: center;
}

.empty-state-icon {
    color: var(--color-text-secondary);
    opacity: 0.5;
}

.empty-state-title {
    font-weight: var(--font-weight-bold);
    margin: 0;
}

.empty-state-description {
    color: var(--color-text-secondary);
    margin: 0;
}
```

## Responsive Design

### Mobile-First Approach
```css
/* Base styles (mobile) */
.component {
    padding: var(--spacing-md);
}

/* Tablet */
@media (min-width: 768px) {
    .component {
        padding: var(--spacing-lg);
    }
}

/* Desktop */
@media (min-width: 1024px) {
    .component {
        padding: var(--spacing-xl);
    }
}
```

### Common Breakpoints
```css
/* Mobile */
@media (max-width: 767px) { }

/* Tablet */
@media (min-width: 768px) and (max-width: 1023px) { }

/* Desktop */
@media (min-width: 1024px) { }

/* Large Desktop */
@media (min-width: 1280px) { }
```

## Testing Checklist

### Per-Component Checklist
- [ ] CSS file created with proper header
- [ ] All classes follow semantic naming
- [ ] All values use theme variables
- [ ] No hardcoded colors/spacing/sizes
- [ ] Responsive breakpoints defined
- [ ] Hover/focus/active states styled
- [ ] Component documented in sections
- [ ] Build succeeds without warnings

### Visual Testing
- [ ] Component renders correctly
- [ ] Colors match design system
- [ ] Spacing is consistent
- [ ] Typography is readable
- [ ] Hover states work
- [ ] Focus states visible
- [ ] Mobile layout correct
- [ ] Tablet layout correct
- [ ] Desktop layout correct

### Theme Testing
- [ ] Change theme variables
- [ ] Verify updates apply
- [ ] Check all states
- [ ] Test responsive behavior

## Best Practices

### ? DO

1. **Use semantic class names**
   ```css
   .stat-card { }
   .post-list-item { }
   ```

2. **Use theme variables**
   ```css
   padding: var(--spacing-lg);
   ```

3. **Document sections**
   ```css
   /* ============================================
      Section Name
      ============================================ */
   ```

4. **Group related styles**
   ```css
   .stat-card { }
   .stat-card-header { }
   .stat-label { }
   .stat-value { }
   ```

5. **Use transitions**
   ```css
   transition: all var(--transition-base);
   ```

### ? DON'T

1. **Don't use implementation names**
   ```css
   /* Bad */
   .purple-box { }
   .big-text { }
   ```

2. **Don't hardcode values**
   ```css
   /* Bad */
   padding: 24px;
   color: #6b21a8;
   ```

3. **Don't create global classes in component CSS**
   ```css
   /* Bad - too generic */
   .container { }
   .button { }
   ```

4. **Don't use !important unless necessary**
   ```css
   /* Bad */
   color: red !important;
   ```

5. **Don't mix inline styles**
   ```razor
   <!-- Bad -->
   <div class="stat-card" style="padding: 24px;">
   ```

## Conclusion

Following these guidelines ensures:
- **Consistency** across all components
- **Maintainability** through semantic naming
- **Scalability** via theme variables
- **Quality** through proper testing
- **Clarity** with good documentation

Every new component should follow these patterns from the start, creating a cohesive and professional codebase.
