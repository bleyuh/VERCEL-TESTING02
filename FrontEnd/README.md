# FrontEnd: School Event Registration

The web page: `index.html`, `style.css` and `app.js`. It needs the API to be running.

## Run it
1. Start the API first: open a terminal in the `API` folder and run `dotnet run`.
2. Open `index.html`:
   - double-click it, or
   - in VS Code, right-click `index.html` and choose Open with Live Server (or click Go Live at the bottom).
3. Click **Sign up** to create your own student account, or **Log in** and use a demo account (the page lists them, the password is `password123`). Click a row to fill in the form.

If you see "Cannot reach the API", the API is not running yet.

## What you see depends on who is logged in
- **Visitor:** Events and Participants, plus **Sign up** and **Log in**. Register asks you to log in.
- **Student:** Register (the form is filled in from the account) and "My registrations".
- **Department organizer / Master Admin:** also **Add event** (choose who can join, and the capacity switch: Open or Quota per program and year level) and **Attendance** (the Event Attendance Manager).

## Sign up page
A new student fills in: first, middle (optional) and last name, student number, student email, contact number, department, program and year level (each list depends on the one before it), a username, and a password typed twice, then ticks the agreement box. The page checks everything before sending it, and the API checks again. After sign-up the student is logged in. The Register form is then filled in from the account, including the contact number.

## Attendance Manager
Filter by event, department, program, year level and status (registered or checked in), search by name, ID, contact number or code, and sort by any column. The list refreshes itself every 10 seconds (you can turn that off). **Check in** and **Undo** work for your own events (the Master Admin can do it for all). **Export CSV** saves exactly the rows you are looking at.

## Putting it online (Vercel)
This folder is a plain static site, so it can be deployed to Vercel as it is (no build step). Before you do, open `config.js` and set `apiUrl` to your online API address. The full guide is in `DEPLOY.md` at the top of the project. In Vercel, set **Root Directory** to `FrontEnd`. `vercel.json` adds a few safe response headers.

## Files
- `config.js`: the one setting you change for hosting: where the API lives (default `http://localhost:5005/api`)
- `vercel.json`: Vercel settings
- `index.html`: the page structure (tabs: Events, Register, Participants, Add event, Attendance, plus the Log in page)
- `style.css`: the look
- `app.js`: collects the input, calls the API with `fetch`, shows the output. The API address is the `API_URL` line at the top. Change it if you change the port in `API/appsettings.json`. After logging in, the page keeps the token in `sessionStorage` and sends it in the `Authorization` header with every request (see the `api()` function).
