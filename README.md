# martinskuta.github.io

Personal website and blog built with [Statiq Web](https://statiq.dev/web) (.NET),
hosted on [GitHub Pages](https://pages.github.com/).

## Quick start

**Prerequisites:** .NET 8 SDK

```bash
# Clone with theme submodule
git clone --recurse-submodules https://github.com/martinskuta/martinskuta.github.io.git
cd martinskuta.github.io

# Preview locally at http://localhost:5080
dotnet run -- preview
```

## Writing a blog post

1. Create `input/posts/YYYY-MM-DD-slug.md`
2. Add front matter and Markdown content:

```markdown
---
Title: My Post Title
Lead: One-line subtitle shown in the post header.
Published: 2025-06-10
Tags:
  - dotnet
  - csharp
---

Your **Markdown** content here.

```csharp
// Code snippets are automatically syntax-highlighted by Prism.js
var x = 42;
```
```

3. Push to `main` → GitHub Actions deploys in ~60 seconds.

## Drafts and scheduled posts

| Goal | How |
|------|-----|
| **Draft** (never publish) | `IsPublished: false` in front matter |
| **Scheduled** (publish on a date) | `Published: 2025-07-01` (future date) — goes live on that day via the daily cron rebuild |

## Adding static pages

Add `.md` files to `input/pages/`. They automatically appear in the navigation bar.
Set `ShowInNavbar: false` to hide a page from the nav.

## Customizing the look

| File | Purpose |
|------|---------|
| `settings.yml` | Site title, description, host, Giscus config |
| `scss/_bootstrap-variable-overrides.scss` | Bootstrap color palette |
| `scss/_variable-overrides.scss` | CleanBlog masthead gradient |
| `scss/_overrides.scss` | Any additional CSS |
| `input/_head.cshtml` | Extra `<head>` content (Prism.js CSS) |
| `input/_scripts.cshtml` | Extra scripts before `</body>` |
| `input/_post-after-content.cshtml` | Content shown after each post (share buttons) |

## Configuring Giscus (comments)

1. Go to <https://giscus.app>
2. Enter `martinskuta/martinskuta.github.io` as the repo
3. Enable GitHub Discussions on the repo
4. Install the [giscus app](https://github.com/apps/giscus) on the repo
5. Copy the `data-repo-id` and `data-category-id` values into `settings.yml`

## Deployment

Every push to `main` triggers `.github/workflows/deploy.yml` which builds the site
and pushes the output to the `gh-pages` branch.

A daily cron in `.github/workflows/scheduled.yml` rebuilds the site so time-gated posts
(future `Published` dates) go live automatically on the right day.

## Custom domain

When you have a domain:

1. Add a `CNAME` file to `input/` containing your domain (e.g. `martinskuta.com`)
2. Set the domain value in both `.github/workflows/deploy.yml` and `scheduled.yml` (`cname:` key)
3. Configure DNS: point `@` to GitHub Pages IPs and/or `www` CNAME to `martinskuta.github.io`
