var gulp = require('gulp');
var user = process.env.GITLAB_USER;
var token = process.env.GITLAB_TOKEN;
var branch = process.env.BRANCH_NAME;
var webpack = require('webpack');
var webpackGulp = require('webpack-stream');

gulp.task('ship-interop', function (done) {
    var localPath = './ej2-blazor-embed';
    var gitPath = 'https://' + user + ':' + token + '@gitlab.syncfusion.com/essential-studio/ej2-blazor-embed';
    var clone = shelljs.exec('git clone ' + gitPath + ' -b ' + branch + ' ' + localPath, { silent: true });
    if (clone.code !== 0) {
        done();
        return;
    }
    else {
        shelljs.exec('gulp minify-ship-interop', { silent: true });
        shelljs.cd(localPath);
        var addFiles = 'git add ./Resources/Scripts/ejs.interop.min.js';
        shelljs.exec(addFiles);
        var message = 'latest ejs.interop.min.js has been shipped to ej2-blazor-embed repository';
        shelljs.exec('git commit -m \"' + message + '\" --no-verify');
        shelljs.exec('git push -f --set-upstream origin --no-verify', { silent: true });
        shelljs.cd('../');
        shelljs.rm('-rf', localPath);
        done();
    }
});

gulp.task('minify-ship-interop', function(done) {
    var webpackConfig = {
        entry: './Scripts/syncfusion-blazor.js',
        output: {
            filename: 'syncfusion-blazor.min.js'
        },
        mode: 'production'
    };
    gulp.src('.')
        .pipe(webpackGulp(webpackConfig, webpack))
        .pipe(gulp.dest('./Syncfusion.Blazor/wwwroot'))
        .on('end', function (e) {
            done();
        })
        .on('error', function (e) {
            console.log(' ------ Error while uglify(minify) of ejs.interop.js ------ ');
            process.exit(1);
            done(e);
        });
});

gulp.task('ie-interop', function(done) {

    var interopDir = './Scripts';
    var ieInteropFileName = '/syncfusion-blazor-ie.js';

    var interop = fs.readFileSync(interopDir + '/syncfusion-blazor.js', 'utf8');

    fs.writeFileSync(interopDir + ieInteropFileName, "\nrequire('regenerator-runtime');\n\n" + interop, 'utf8');

    var webpackConfig = {
        entry: interopDir + ieInteropFileName,
        output: {
            filename: 'syncfusion-blazor-ie.min.js'
        },
        mode: 'production',
        module: {
            rules: [{
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: [
                            ["@babel/env",
                            {
                                targets: {
                                    ie: "11"
                                }
                            }]
                        ]
                    }
                }
            }]
        }
    };
    gulp.src('.')
        .pipe(webpackGulp(webpackConfig, webpack))
        .pipe(gulp.dest(interopDir))
        .on('end', function (e) {
            shelljs.rm('-rf', interopDir + ieInteropFileName)
            done();
        })
        .on('error', function (e) {
            console.log(' ------ Error while uglify(minify) of syncfusion-blazor-ie.js ------ ');
            shelljs.rm('-rf', interopDir + ieInteropFileName)
            process.exit(1);
            done(e);
        });
});