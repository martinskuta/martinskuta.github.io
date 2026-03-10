---
Title: Hello World — My First Post
Lead: Every blog needs a first post. Here's mine.
Published: 2025-03-05
Tags: [meta, blogging]
---

Welcome to my blog! 🎉

This is the first post, mostly to verify that everything is wired up correctly.
Here's a quick tour of what this blog supports.

## Code Snippets

Syntax highlighting works out of the box. Here's a minimal ASP.NET Core app:

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello from .NET!");

app.Run();
```

And some shell commands:

```bash
dotnet run -- preview
# → open http://localhost:5080
```

## Tables

| Feature            | Status |
|--------------------|--------|
| Markdown posts     | ✅     |
| Code highlighting  | ✅     |
| RSS feed           | ✅     |
| Full-text search   | ✅     |
| Comments           | ✅     |
| Reactions          | ✅     |

## Embedding a video

You can drop a plain HTML `<iframe>` anywhere in a Markdown post:

<iframe width="560" height="315"
  src="https://www.youtube.com/embed/dQw4w9WgXcQ"
  title="YouTube video"
  frameborder="0"
  allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
  allowfullscreen>
</iframe>

---

That's it for the first post. More to come!
