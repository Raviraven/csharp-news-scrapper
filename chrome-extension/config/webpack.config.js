'use strict';

const { merge } = require('webpack-merge');

const common = require('./webpack.common.js');
const PATHS = require('./paths');

// Merge webpack configuration files
const config = (env, argv) => merge(common, {
  entry: {
    index: PATHS.src + '/index.ts',
    contentScript: PATHS.src + '/contentScript.ts',
    background: PATHS.src + '/background.ts',
  },
  devtool: argv.mode === 'production' ? false : 'source-map'
});

module.exports = config;
