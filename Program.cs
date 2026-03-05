using System.Diagnostics;
using System.Runtime.InteropServices;
using Statiq.App;
using Statiq.Common;
using Statiq.Web;

// On ARM64 Windows, SharpScss's native libsass.dll fails under x64 emulation.
// Detect this and fall back to the dart-sass CLI (installed globally as `sass`).
bool useCliSass = RuntimeInformation.OSArchitecture == Architecture.Arm64
               && RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

if (useCliSass)
{
    // Pre-compile SCSS into output/ BEFORE Statiq runs so the CSS is already there
    Console.WriteLine("[INFO] ARM64 detected — compiling SCSS with dart-sass instead of SharpScss.");
    PreCompileScss();

    // Tell Statiq not to wipe output/ before building (preserves our pre-compiled CSS)
    args = [.. args, "--noclean"];
}

var bootstrapper = Bootstrapper
    .Factory
    .CreateWeb(args);

if (useCliSass)
{
    // Remove the SASS/SCSS templates so Statiq doesn't invoke the broken SharpScss module
    bootstrapper = bootstrapper
        .RemoveTemplate(MediaTypes.Sass)
        .RemoveTemplate(MediaTypes.Scss);
}

return await bootstrapper.RunAsync();

// ─────────────────────────────────────────────────────────────────────────────
static void PreCompileScss()
{
    Directory.CreateDirectory("output/scss");

    // We compile from scss/clean-blog.scss (our entry point) rather than
    // theme/input/scss/clean-blog.scss. This is necessary because dart-sass
    // resolves @import relative to the source file's directory first, which
    // would find the theme's empty placeholder files (_bootstrap-variable-overrides.scss
    // etc.) before our project-level overrides in scss/. By compiling from
    // scss/, all placeholder imports resolve to OUR files first.
    // On Windows, `sass` is a .cmd shim — must invoke via cmd.exe /c
    var info = new ProcessStartInfo
    {
        FileName = "cmd.exe",
        Arguments = "/c sass scss/clean-blog.scss output/scss/clean-blog.css" +
                    " --style compressed --no-source-map",
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardError = true,
    };

    using var proc = Process.Start(info)!;
    string stderr = proc.StandardError.ReadToEnd();
    proc.WaitForExit();

    if (proc.ExitCode != 0)
    {
        Console.Error.WriteLine($"[WARN] dart-sass compilation failed:\n{stderr}");
        Console.Error.WriteLine("[WARN] CSS may be missing. Ensure 'sass' is in PATH.");
    }
    else
    {
        Console.WriteLine("[INFO] SCSS compiled successfully with dart-sass.");
    }
}
