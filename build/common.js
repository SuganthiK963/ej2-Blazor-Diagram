'use strict';

var fs = global.fs = global.fs || require('fs');
var shelljs = global.shelljs = global.shelljs || require('shelljs');

exports.stagingBranch = process.env.STAGING_BRANCH;

var regexp = {
    BUG: /^bug(\(([A-Za-z0-9]+[-][0-9]*)\))?\: /,
    CI_SKIP: /^ci-skip(\(([A-Za-z0-9]+[-][0-9]*)\))?\: /,
    COMMIT_TYPES: /^(bug|config|documentation|feature|sample|ci-skip)(\(([A-Za-z0-9]+[-][0-9]*)\))?\: (.*)$/,
};
exports.regexp = regexp;

/**
 * get current repository name 
 */
var currentRepo = "ej2-blazor-source";
exports.currentRepo = currentRepo;