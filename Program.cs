using System.Diagnostics;
using System.Runtime.InteropServices;
using Statiq.App;
using Statiq.Common;
using Statiq.Web;

// Use dart-sass CLI when available (cross-platform), otherwise only fall back on ARM64 Windows.
bool sassAvailable = IsSassAvailable();
bool useCliSass = sassAvailable || (RuntimeInformation.OSArchitecture == Architecture.Arm64
               && RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

if (useCliSass)
{
    Console.WriteLine(sassAvailable
        ? "[INFO] 'sass' CLI found on PATH — compiling SCSS with dart-sass."
        : "[INFO] ARM64 Windows detected — compiling SCSS with dart-sass instead of SharpScss.");

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
static bool IsSassAvailable()
{
    try
    {
        var info = new ProcessStartInfo
        {
            FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "sass",
            Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "/c sass --version" : "--version",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        using var proc = Process.Start(info)!;
        proc.WaitForExit(5000);
        return proc.ExitCode == 0;
    }
    catch
    {
        return false;
    }
}

static void PreCompileScss()
{
    Directory.CreateDirectory("output/scss");

    // We compile from scss/clean-blog.scss (our entry point) rather than
    // theme/input/scss/clean-blog.scss. This is necessary because dart-sass
    // resolves @import relative to the source file's directory first, which
    // would find the theme's empty placeholder files (_bootstrap-variable-overrides.scss
    // etc.) before our project-level overrides in scss/. By compiling from
    // scss/, all placeholder imports resolve to OUR files first.

    var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    var fileName = isWindows ? "cmd.exe" : "sass";
    var arguments = isWindows
        ? "/c sass scss/clean-blog.scss output/scss/clean-blog.css --style compressed --no-source-map"
        : "scss/clean-blog.scss output/scss/clean-blog.css --style compressed --no-source-map";

    var info = new ProcessStartInfo
    {
        FileName = fileName,
        Arguments = arguments,
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardError = true,
        RedirectStandardOutput = true,
    };

    using var proc = Process.Start(info)!;
    string stdout = proc.StandardOutput.ReadToEnd();
    string stderr = proc.StandardError.ReadToEnd();
    proc.WaitForExit();

    if (proc.ExitCode != 0)
    {
        Console.Error.WriteLine($"[WARN] dart-sass compilation failed:\n{stderr}\n{stdout}");
        Console.Error.WriteLine("[WARN] CSS may be missing. Ensure 'sass' is in PATH.");
    }
    else
    {
        Console.WriteLine("[INFO] SCSS compiled successfully with dart-sass.");
    }
}
