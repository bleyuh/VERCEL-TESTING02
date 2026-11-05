# Builds the API into a small image that Render (or any Docker host) can run.
# If you change <TargetFramework> in EventRegistrationAPI.csproj (for example to net10.0),
# change the two "8.0" tags below to the same number.

# ---- step 1: compile the project ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY EventRegistrationAPI.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---- step 2: the small image that actually runs ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
# Render tells the app which port to use through the PORT variable (Program.cs reads it)
EXPOSE 10000
ENTRYPOINT ["dotnet", "EventRegistrationAPI.dll"]
