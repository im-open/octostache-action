# octostache-action

This action will scan the file(s) provided in the `files-with-substitutions` argument for Octopus variable substitution syntax `#{VariableName}`. If the files contain any `#{Variables}` that match an item in the `variables-file` or environment variables, it will replace the template with the actual value. If a variable is found in both the `variables-file` and in the environment variables, then the environment variable value will be used.

This is a container action so it will not work on Windows runners.

## Index<!-- omit in toc -->

- [octostache-action](#octostache-action)
  - [Inputs](#inputs)
  - [Outputs](#outputs)
  - [Usage Examples](#usage-examples)
    - [Variables File](#variables-file)
    - [Environment Variables](#environment-variables)
    - [Example Files that contain Octostache Substitution Syntax](#example-files-that-contain-octostache-substitution-syntax)
    - [Workflow](#workflow)
  - [Contributing](#contributing)
    - [Incrementing the Version](#incrementing-the-version)
    - [Source Code Changes](#source-code-changes)
    - [Recompiling Manually](#recompiling-manually)
    - [Updating the README.md](#updating-the-readmemd)
  - [Code of Conduct](#code-of-conduct)
  - [License](#license)

## Inputs

| Parameter                  | Is Required | Description                                                                                                                                                                                                                                  |
|----------------------------|-------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `variables-file`           | false       | An optional yaml file containing variables to use in the substitution.                                                                                                                                                                       |
| `files-with-substitutions` | true        | A comma separated list of files or [.NET-compatible glob patterns](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.filesystemglobbing.matcher?view=dotnet-plat-ext-6.0#remarks) with `#{variables}` that need substitution. |

## Outputs

No Outputs

## Usage Examples

### Variables File

When using the `variables-file` argument, this is the structure of the file containing the variable names and values that will be substituted.

```yaml
Environment: Dev
Version: 1.3.62
LaunchDarklyKey: abc
GoogleAnalyticsKey: 123
AppInsightsKey: a1b2c3
```

### Environment Variables

In addition to the `variables-file` argument, substitutions can be provided in the `env:` section of the action. The format matches what is supplied in the `variables-file`: `<var-name>: <var-value>`.

If the same item is provided in the `variables-file` and the `env:` section, the value in the `env:` section will be used.

### Example Files that contain Octostache Substitution Syntax

These are some sample files that contain the Octostache substitution syntax `#{}`. These files would be included in the `files-with-substitutions` argument above.

`DemoApp19.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
    <Version>#{VersionToReplace}</Version>
  </PropertyGroup>
</Project>
```

`build-variables.js`

```js
const { profiles } = require('../../Properties/launchSettings.json');
const { environmentTypes } = require('./constants');

const substitutionVariables = {
  BUILD_APPINSIGHTS_INSTRUMENTATION_KEY: '#{AppInsightsKey}',
  BUILD_GA_KEY: '#{GoogleAnalyticsKey}',
  BUILD_LAUNCH_DARKLY_KEY: '#{LaunchDarklyKey}}'
};
```

`index.html`

```html
<html>
  <head>
    <!--... head items ...-->
    <script type="text/javascript">var gaKey = '#{GoogleAnalyticsKey}';</script>
  </head>
  <body>
    <!--... application body ...-->
  </body>
</html>
```

### Workflow

```yml
name: Deploy

on:
  workflow_dispatch:

jobs:
  substitute-variables:
    runs-on: ubuntu-20.04
    steps:
      - uses: actions/checkout@v3
      
      # You may also reference just the major or major.minor version
      - uses: im-open/octostache-action@v4.0.2
        with:
          variables-file: ./substitution-variables.json
          files-with-substitutions: ./src/DemoApp19/DemoApp19.csproj,./src/DemoApp19/Bff/FrontEnd/scripts/build-variables.js,./src/**/*.html
        env:
          # Note that this value would be used over the value from the example variables file
          LaunchDarklyKey: ${{ secrets.LAUNCH_DARKLY_API_KEY }}
```

## Contributing

When creating PRs, please review the following guidelines:

- [ ] The action code does not contain sensitive information.
- [ ] At least one of the commit messages contains the appropriate `+semver:` keywords listed under [Incrementing the Version] for major and minor increments.
- [ ] The action has been recompiled.  See [Recompiling Manually] for details.
- [ ] The README.md has been updated with the latest version of the action.  See [Updating the README.md] for details.

### Incrementing the Version

This repo uses [git-version-lite] in its workflows to examine commit messages to determine whether to perform a major, minor or patch increment on merge if [source code] changes have been made.  The following table provides the fragment that should be included in a commit message to active different increment strategies.

| Increment Type | Commit Message Fragment                     |
|----------------|---------------------------------------------|
| major          | +semver:breaking                            |
| major          | +semver:major                               |
| minor          | +semver:feature                             |
| minor          | +semver:minor                               |
| patch          | *default increment type, no comment needed* |

### Source Code Changes

The files and directories that are considered source code are listed in the `files-with-code` and `dirs-with-code` arguments in both the [build-and-review-pr] and [increment-version-on-merge] workflows.  

If a PR contains source code changes, the README.md should be updated with the latest action version and the action should be recompiled.  The [build-and-review-pr] workflow will ensure these steps are performed when they are required.  The workflow will provide instructions for completing these steps if the PR Author does not initially complete them.

If a PR consists solely of non-source code changes like changes to the `README.md` or workflows under `./.github/workflows`, version updates and recompiles do not need to be performed.

### Recompiling Manually

This command utilizes [esbuild] to bundle the action and its dependencies into a single file located in the `dist` folder.  If changes are made to the action's [source code], the action must be recompiled by running the following command:

```sh
# Installs dependencies and bundles the code
npm run build
```

### Updating the README.md

If changes are made to the action's [source code], the [usage examples] section of this file should be updated with the next version of the action.  Each instance of this action should be updated.  This helps users know what the latest tag is without having to navigate to the Tags page of the repository.  See [Incrementing the Version] for details on how to determine what the next version will be or consult the first workflow run for the PR which will also calculate the next version.

## Code of Conduct

This project has adopted the [im-open's Code of Conduct](https://github.com/im-open/.github/blob/main/CODE_OF_CONDUCT.md).

## License

Copyright &copy; 2023, Extend Health, LLC. Code released under the [MIT license](LICENSE).

 <!-- Links -->
[Incrementing the Version]: #incrementing-the-version
[Recompiling Manually]: #recompiling-manually
[Updating the README.md]: #updating-the-readmemd
[source code]: #source-code-changes
[usage examples]: #usage-examples
[build-and-review-pr]: ./.github/workflows/build-and-review-pr.yml
[increment-version-on-merge]: ./.github/workflows/increment-version-on-merge.yml
[esbuild]: https://esbuild.github.io/getting-started/#bundling-for-node
[git-version-lite]: https://github.com/im-open/git-version-lite
