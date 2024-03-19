const { profiles } = require('../../Properties/launchSettings.json');
const { environmentTypes } = require('./constants');

const substitutionVariables = {
  BUILD_APPINSIGHTS_INSTRUMENTATION_KEY: 'override_ai_key',
  BUILD_GA_KEY: 'override_ga_key',
  BUILD_LAUNCH_DARKLY_KEY: 'override_ld_key'
};

// .... Do the build things here
