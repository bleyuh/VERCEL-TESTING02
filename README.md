# School Event Registration

```
CCIT102-PROJ/
  API/        C# ASP.NET Core Web API (goes to Render, in a Docker container)
  FrontEnd/   index.html, style.css, app.js, config.js (goes to Vercel as a static site)
  DEPLOY.md   step-by-step: put it online
  .gitignore  keeps bin, obj and data.json out of GitHub
```

## Run it on your computer
1. In `API/`: `dotnet run`  (listens on http://localhost:5005)
2. Open `FrontEnd/index.html` (double-click it, or use Live Server in VS Code).
3. Log in with a demo account (password `password123`) or click **Sign up**.

More detail: `API/README.md` and `FrontEnd/README.md`.

## Put it online
Read **DEPLOY.md**. Short version: the website (FrontEnd) goes on Vercel, the API goes on Render, and `FrontEnd/config.js` tells the website where the API is.
