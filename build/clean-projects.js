'use strict';
var gulp = require('gulp');
var glob = require('glob');
var shelljs = require('shelljs');

gulp.task('clean-projects', (done) => {
    var files = glob.sync('**/{bin,obj}/', {ignore: 'node_modules/**'});
    shelljs.rm('-rf', files);
    done();
});