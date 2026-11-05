using EventRegistrationAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// ---- Hosting settings (NEW) ----
// All of these are OPTIONAL. If you set none of them, the app behaves exactly like it did on your computer.
// A host such as Render lets you set them under "Environment":
//   PORT             the port to listen on. Hosting services set this for you.
//   ALLOWED_ORIGINS  the website address(es) allowed to call this API, separated by commas,
//                    for example  https://my-school-events.vercel.app   (leave it empty to allow any website)
//   DATA_FILE        where to save data.json, for example  /var/data/data.json
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port)) builder.WebHost.UseUrls($"http://0.0.0.0:{port}");   // overrides the Urls line in appsettings.json

var origins = (Environment.GetEnvironmentVariable("ALLOWED_ORIGINS") ?? "")
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(o => o.TrimEnd('/'))     // "https://x.vercel.app/" and "https://x.vercel.app" mean the same site
    .ToArray();

var dataFile = Environment.GetEnvironmentVariable("DATA_FILE");
if (string.IsNullOrWhiteSpace(dataFile)) dataFile = Path.Combine(builder.Environment.ContentRootPath, "data.json");
Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(dataFile))!);

// 1) Turn on controllers (the classes inside the Controllers folder)
builder.Services.AddControllers();

// 2) CORS: the web page lives on a different "address" than this API (for example Vercel and Render).
//    A browser only lets the page call the API if the API allows that address here.
//    With ALLOWED_ORIGINS set, only those websites are allowed. Without it, any website is allowed
//    (fine while testing; set ALLOWED_ORIGINS once your site is online).
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (origins.Length > 0) policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
        else policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// 3) Create ONE DataStore for the whole app. It keeps the events, students, registrations and accounts in memory
//    and saves them to data.json so nothing is lost. Controllers receive it automatically through their constructor.
builder.Services.AddSingleton(new DataStore(dataFile));

var app = builder.Build();

app.UseCors();           // must come before MapControllers
app.MapControllers();    // connects the [Route] attributes in the controllers to URLs like /api/events

// A friendly message if you open the API address in the browser
app.MapGet("/", () => "Event Registration API is running. Try /api/events");

// NEW: hosting services call this to check that the app is alive
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
