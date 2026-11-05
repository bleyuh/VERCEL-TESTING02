# Putting the project online (Vercel + Render)

**Why two places?** Vercel hosts websites (HTML, CSS, JavaScript) very well, but it does not run a normal .NET API. So:

| Part | Where | Why |
|---|---|---|
| `FrontEnd/` (the website) | **Vercel** | static site, free |
| `API/` (the C# API) | **Render** | runs Docker containers, has a free plan |

You need free accounts on **GitHub**, **Render** and **Vercel** (you can sign in to Render and Vercel with your GitHub account). Vercel only deploys from a Git repository or with its command-line tool; it has no "upload a zip" button.

---

## Step 1: put the project on GitHub
1. On github.com click **New repository**, give it a name (for example `school-events`), leave it empty (no README), and create it.
2. Open a terminal in the `CCIT102-PROJ` folder (the one that contains `API`, `FrontEnd` and `DEPLOY.md`) and run:
   ```
   git init
   git add .
   git commit -m "School event registration"
   git branch -M main
   git remote add origin https://github.com/YOUR-USERNAME/school-events.git
   git push -u origin main
   ```
   The `.gitignore` in this folder keeps `bin`, `obj` and `data.json` out of GitHub. That is what you want.

## Step 2: put the API on Render
1. On render.com choose **New** and then **Web Service**, and connect your GitHub repository.
2. Fill in:
   - **Language:** Docker
   - **Root Directory:** `API`
   - **Instance type:** Free
   - **Health Check Path** (under Advanced): `/health`
3. Click **Create Web Service** and wait until the log says the service is live (the first build takes a few minutes).
4. Copy the address at the top of the page. It looks like `https://school-events-api.onrender.com`.
5. Check it: open `https://YOUR-API.onrender.com/health` (it should say `{"status":"ok"}`) and `https://YOUR-API.onrender.com/api/events` (it should show the sample events).

## Step 3: tell the website where the API is
1. Open `FrontEnd/config.js` and replace the address with yours. Keep the `/api` at the end and do not add a `/` after it:
   ```js
   window.APP_CONFIG = {
     apiUrl: 'https://YOUR-API.onrender.com/api'
   };
   ```
2. Save, then commit and push:
   ```
   git add .
   git commit -m "Point the website to the online API"
   git push
   ```

## Step 4: put the website on Vercel
1. On vercel.com choose **Add New...** and then **Project**, and import the same GitHub repository.
2. In the settings screen:
   - **Framework Preset:** Other
   - **Root Directory:** click **Edit** and choose `FrontEnd` (**this one is important**)
   - Leave **Build Command** and **Output Directory** empty.
3. Click **Deploy**. When it finishes, copy your address, for example `https://school-events.vercel.app`.

(Using the command line instead, from inside the `FrontEnd` folder: `npm install -g vercel`, then `vercel`, then `vercel --prod`. Say yes to the questions and keep the defaults.)

## Step 5: let only your website call the API (recommended)
1. In Render open your service and go to **Environment**.
2. Add a variable: **Key** `ALLOWED_ORIGINS`, **Value** your Vercel address (for example `https://school-events.vercel.app`, no `/` at the end). If you have more than one address, separate them with commas.
3. Save. Render restarts the API.

Until you do this, the API accepts calls from any website.

## Step 6: try it
Open your Vercel address. **The first visit can take up to a minute** while the free API wakes up. Then log in (for example `FAITH` / `password123`) or sign up.

---

## Things you must know before sharing the link

**1. Data is not kept on the free plan.** Render's free service goes to sleep after 15 minutes without visitors, and every time it sleeps, restarts or redeploys, the files it wrote are erased. That includes `data.json`. So **new sign-ups, events and registrations disappear**, and the sample data and demo accounts come back. This is fine for a demo or class presentation. To keep real data you need a database (for example a free hosted Postgres). Ask for that version if you need it.

**2. Everyone can see the demo passwords.** The login page lists the demo accounts, and `FAITH` (Master Admin) uses `password123`. Anyone with your link can log in as the Master Admin. Before you share the link widely, change the passwords in `DataStore.SeedAccounts()` (in `API/Data/DataStore.cs`) and remove the demo table from the login page, or share the link only with classmates.

**3. Sign-up has no email or SMS verification.** Anyone can create a student account with any student number that is not taken yet.

## Troubleshooting
| Problem | What to do |
|---|---|
| Vercel shows 404 | **Root Directory** was not set to `FrontEnd`. In the Vercel project go to Settings, then General, set it, and redeploy. |
| The page says "Cannot reach the server" | The free API may be waking up. Wait a minute and refresh. Also open `YOUR-API/health` to see if it answers. |
| The browser console shows a CORS error | `ALLOWED_ORIGINS` must be exactly your Vercel address: `https`, no path, no `/` at the end. Fix it in Render, or delete the variable to allow any website. |
| The page still tries `localhost` | `config.js` was not changed or not pushed. Push it, wait for Vercel to redeploy, and refresh the page (Ctrl+F5). |
| Render cannot find the Dockerfile | Set **Root Directory** to `API`. If it still fails, leave Root Directory empty and set **Dockerfile Path** to `./API/Dockerfile` and **Docker Build Context Directory** to `./API`. |
| Render build fails about the .NET version | The two `8.0` tags in `API/Dockerfile` must match `<TargetFramework>` in `EventRegistrationAPI.csproj` (`net8.0`). |

## Changing things later
Edit the files, then `git add .`, `git commit -m "message"` and `git push`. Render and Vercel redeploy by themselves.
