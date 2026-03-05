# Copilot Instructions

## Project overview

Personal website and blog built with [Statiq Web](https://statiq.dev/web) (.NET 8), using the **CleanBlog** theme (git submodule at `theme/`). Hosted on GitHub Pages.

## Build & preview

```bash
# Local preview at http://localhost:5080
dotnet run -- preview

# One-shot build to output/
dotnet run -- build
```

> **ARM64 Windows only:** `Program.cs` detects ARM64 and falls back to the dart-sass CLI instead of SharpScss. The `sass` command must be in PATH (`npm install -g sass`). The build uses `--noclean` to preserve pre-compiled CSS.

## Theme customization

The site uses the **CleanBlog** theme as a git submodule (`theme/`). **Never edit files inside `theme/`.**

To override a theme template, create a file with the same relative path under `input/`. Statiq resolves `input/` files before `theme/input/` files.

| Theme file | Override location | What it controls |
|---|---|---|
| `theme/input/_navigation.cshtml` | `input/_navigation.cshtml` | Fixed top navbar (brand/logo, nav links) |
| `theme/input/_header.cshtml` | `input/_header.cshtml` | Masthead banner below the navbar (background image, page title) |
| `theme/input/_head.cshtml` | `input/_head.cshtml` | Extra `<head>` content |
| `theme/input/_footer.cshtml` | `input/_footer.cshtml` | Footer |
| `theme/input/_scripts.cshtml` | `input/_scripts.cshtml` | Scripts before `</body>` |

**Important naming distinction:**
- **Navbar / top bar** — the fixed navigation strip at the very top of every page → `input/_navigation.cshtml`
- **Header / masthead** — the large full-width banner with background image that sits *below* the navbar → `input/_header.cshtml`

## Assets

Static assets (images, GIFs, etc.) go in `input/assets/images/`. Reference them in templates as `/assets/images/<filename>`.

## Site settings

All global configuration (site title, description, Giscus comments, header image) lives in `settings.yml`.
