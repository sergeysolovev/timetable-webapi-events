# Timetable.Web

St. Petersburg University Timetables Web App

## Prerequisites

Check out this [README.md](../../../README.md#prerequisites).

## Quickstart

1. Configure public path for serving the assets. By default, it is set to `/timetable/`.
    * If the app is to be served from `<http|https>://<hostname>:<port>/timetable/`, keep the default public path;
    * Otherwise public path has to be overriden. For example, on production we need public path to be set to `/`. To do this, either set it's value in [webpack.prod.js](webpack.prod.js) or even better set the environment variable: `SET "PUBLIC_PATH=/"`. For development the options are the same: [webpack.dev.js](webpack.dev.js) or the environment variable.

2. Install the dependencies with `nuget restore ..` and `npm install`

3. Prepare the assets for serving
    * With webpack dev server: set `webpack:wds:use` to "true" in [Web.config](Web.config) and start WDS `SET "PUBLIC_PATH=/" && npm run start`
    * Standalone: set `webpack:wds:use` to "false" (default) and build the assets `SET "PUBLIC_PATH=/" && npm run build` and start the app. Another way to build the static assets is to `SET "PUBLIC_PATH=/" && MSBuild /t:BuildNpm /clp:ForceNoAlign`.

4. Start the app (IIS, IIS Express, etc) and enjoy!

## Production build

Make sure that the app is not configured to load assets from webpack dev server in [Web.config](Web.config)

```xml
<add key="webpack:wds:use" value="false" />
```

If the build is for live deployment, set public path to `/`.

```shell
SET "PUBLIC_PATH=/" && MSBuild /t:Build;BuildNpm /clp:ForceNoAlign
```

Alternatively, use [BuildAllProjects.bat](../../Build/BuildAllProjects.bat)
