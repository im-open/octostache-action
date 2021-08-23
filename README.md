# authenticate-with-gh-package-registries

This action will scan the file provided in the `file-with-substitutions` argument for Octopus variable substitution syntax `#{VariableName}`.  If it finds a matching variable in the `variables-file`, it will replace the template with the actual value.

The variable value and keys can all be placed inside of one file.  The substitution has to be performed one file at a time though.

This is a container action so it will not work on Windows runners.

## Inputs

| Parameter                 | Is Required | Description                                                                                                                                                      |
| ------------------------- | ----------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `variables-file`          | true        | The json file that holds the variables and values to be used in the substitution.<br/>Defined as JSON object, e.g. `{"Variable1":"value1","Variable2":"value2"}` |
| `file-with-substitutions` | true        | The file that has substitutions to be performed.                                                                                                                 |
| `output-file`             | true        | The file the substitution results should be saved to.  This is generally the same value provided in `file-with-substitutions`.                                   |

## Outputs

No Outputs

## Usage Example

### Variables File

```json
{
    "Environment": "Dev",
    "Version": "1.3.62",
    "LaunchDarklyKey": "abc",
    "GoogleAnalyticsKey": "123",
    "AppInsightsKey": "a1b2c3",
}
```

### File with Substitution

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

### Workflow

```yml
name: Deploy

on:
  workflow_dispatch:

jobs:
  substitute-variables:
    runs-on: ubuntu-20.04
    steps:
      - uses: actions/checkout@v2

      - uses: im-open/octostache-action@v1.0.0
        with:
          variablesFile: ./substitution-variables.json
          templateFile: ./src/DemoApp19/DemoApp19.csproj
          outputFile: ./src/DemoApp19/DemoApp19.csproj

      - uses: im-open/octostache-action@v1.0.0
        with:
          variablesFile: ./substitution-variables.json
          templateFile: ./src/DemoApp19/Bff/FrontEnd/scripts/build-variables.js
          outputFile: ./src/DemoApp19/Bff/FrontEnd/scripts/build-variables.js
```


## Code of Conduct

This project has adopted the [im-open's Code of Conduct](https://github.com/im-open/.github/blob/master/CODE_OF_CONDUCT.md).

## License

Copyright &copy; 2021, Extend Health, LLC. Code released under the [MIT license](LICENSE).

[Authenticating to GitHub Packages - nuget]: https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry#authenticating-to-github-packages
[dotnet nuget add source]: https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-nuget-add-source
[Authenticating to GitHub Packages - npm]: https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-npm-registry#authenticating-to-github-packages
[npm private packages in ci/cd workflow]: https://docs.npmjs.com/using-private-packages-in-a-ci-cd-workflow