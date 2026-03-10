---
Title: Draft Post — Not Published Yet
Lead: This post is a draft and will not appear on the site until Excluded is removed.
Published: 2025-04-01
Tags: [dotnet]
Excluded: true
---

## This is a draft

The `Excluded: true` front-matter key tells Statiq to skip this file entirely during the build.
It exists in the repository so you can work on it over time without it going live.

When you're ready to publish:

1. Remove the `Excluded: true` line (or change it to `Excluded: false`)
2. Update the `Published` date if needed
3. `git commit` and `git push`
4. GitHub Actions will build and deploy in ~60 seconds

## Scheduled posts — use a future Published date instead

If you want a post to go live automatically on a specific date, set `Published` to that
future date and **do not** set `Excluded: true`. The daily GitHub Actions cron rebuilds
the site every morning at 08:00 UTC; the post will automatically appear on that date.

```markdown
Title: My Scheduled Post
Published: 2026-01-15
Tags:
  - dotnet
---
Post content here. No Excluded key needed — the date controls when it goes live.
```

> **How it works**: The site is configured so that posts with a future `Published` date are
> neither listed in the archive nor written to disk as HTML. On the scheduled date, the
> daily cron rebuilds the site and the post naturally appears.
