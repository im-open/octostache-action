# octostache-action

This action will scan the file(s) provided in the `files-with-substitutions` argument for Octopus variable substitution syntax `#{VariableName}`. If the files contain any `#{Variables}` that match an item in the `variables-file` or environment variables, it will replace the template with the actual value. If a variable is found in both the `variables-file` and in the environment variables, then the environment variable value will be used.

This is a container action so it will not work on Windows runners.

## Index

- [octostache-action](#octostache-action)
  - [Index](#index)
  - [Inputs](#inputs)
  - [Outputs](#outputs)
  - [Usage Example](#usage-example)
    - [Variables File](#variables-file)
    - [Environment Variables](#environment-variables)
    - [Example Files that contain Octostache Substitution Syntax](#example-files-that-contain-octostache-substitution-syntax)
    - [Workflow](#workflow)
  - [Contributing](#contributing)
    - [Recompiling](#recompiling-manually)
    - [Incrementing the Version](#incrementing-the-version)
  - [Code of Conduct](#code-of-conduct)
  - [License](#license)

## Inputs

| Parameter                  | Is Required | Description                                                                                                                                                                                                                                  |
| -------------------------- | ----------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `variables-file`           | false       | An optional yaml file containing variables to use in the substitution.                                                                                                                                                                       |
| `files-with-substitutions` | true        | A comma separated list of files or [.NET-compatible glob patterns](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.filesystemglobbing.matcher?view=dotnet-plat-ext-6.0#remarks) with `#{variables}` that need substitution. |

## Outputs

No Outputs

## Usage Example

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

      - uses: im-open/octostache-action@v4.0.0
        with:
          variables-file: ./substitution-variables.json
          files-with-substitutions: ./src/DemoApp19/DemoApp19.csproj,./src/DemoApp19/Bff/FrontEnd/scripts/build-variables.js,./src/**/*.html
        env:
          # Note that this value would be used over the value from the example variables file
          LaunchDarklyKey: ${{ secrets.LAUNCH_DARKLY_API_KEY }}
```

## Contributing

When creating new PRs please ensure:

1. For major or minor changes, at least one of the commit messages contains the appropriate `+semver:` keywords listed under [Incrementing the Version](#incrementing-the-version).
1. The action code does not contain sensitive information.

When a pull request is created and there are changes to code-specific files and folders, the build workflow will run and it will recompile the action and push a commit to the branch if the PR author has not done so. The usage examples in the README.md will also be updated with the next version if they have not been updated manually. The following files and folders contain action code and will trigger the automatic updates:

- `action.yml`
- `package.json`
- `package-lock.json`
- `src/**`
- `.build-linux/**`
- `.build-win/**`

There may be some instances where the bot does not have permission to push changes back to the branch though so these steps should be done manually for those branches. See [Recompiling Manually](#recompiling-manually) and [Incrementing the Version](#incrementing-the-version) for more details.

### Recompiling Manually

If changes are made to the action's code in this repository, or its dependencies, the action can be re-compiled by running the following command:

```sh
# Installs dependencies
npm install

# Build the code
npm run build
```

The `build` command builds the code for both windows and linux operating systems. These changes will be committed so the action can utilize the built executables at run time.

### Incrementing the Version

Both the `auto-update-readme` and PR merge workflows will use the strategies below to determine what the next version will be.  If the build workflow was not able to automatically update the README.md action examples with the next version, the README.md should be updated manually as part of the PR using that calculated version.

This action uses [git-version-lite] to examine commit messages to determine whether to perform a major, minor or patch increment on merge. The following table provides the fragment that should be included in a commit message to active different increment strategies.
| Increment Type | Commit Message Fragment |
| -------------- | ------------------------------------------- |
| major | +semver:breaking |
| major | +semver:major |
| minor | +semver:feature |
| minor | +semver:minor |
| patch | _default increment type, no comment needed_ |

## Code of Conduct

This project has adopted the [im-open's Code of Conduct](https://github.com/im-open/.github/blob/master/CODE_OF_CONDUCT.md).

## License

Copyright &copy; 2021, Extend Health, LLC. Code released under the [MIT license](LICENSE).

[git-version-lite]: https://github.com/im-open/git-version-lite
[authenticating to github packages - nuget]: https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry#authenticating-to-github-packages
[dotnet nuget add source]: https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-nuget-add-source
[authenticating to github packages - npm]: https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-npm-registry#authenticating-to-github-packages
[npm private packages in ci/cd workflow]: https://docs.npmjs.com/using-private-packages-in-a-ci-cd-workflow
