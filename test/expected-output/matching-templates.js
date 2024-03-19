const { profiles } = require('../../Properties/launchSettings.json');
const { environmentTypes } = require('./constants');

const substitutionVariables = {
  BUILD_APPINSIGHTS_INSTRUMENTATION_KEY: 'a1b2c3_appInsights',
  BUILD_GA_KEY: '123_googleAnalytics',
  BUILD_LAUNCH_DARKLY_KEY: 'abc_launchDarkly'
};

// .... Do the build things here
