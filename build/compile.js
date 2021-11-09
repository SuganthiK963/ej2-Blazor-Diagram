let { clean, restore, build, pack, push } = require('gulp-dotnet-cli');
const path = require('path');
let gulp = require('gulp');
let nupkgPath = path.resolve(process.cwd(), 'Nuget');
var common = require('./common.js');
var shelljs = global.shelljs = global.shelljs || require('shelljs');
var regexp = common.regexp;
var fs = global.fs = global.fs || require('fs');
var webpack = require('webpack-stream');
const glob = require('glob');
const config = require(__dirname + '/../config.json');

var isReleaseBranch = /^(release\/)/g.test(process.env.BRANCH_NAME);
var isHotfixBranch = /^(hotfix\/)/g.test(process.env.BRANCH_NAME);

var nugetPath = process.env.BRANCH_NAME === 'master' ? 'ej2-nuget-production/' : 'ej2-nuget/';
nugetPath = isReleaseBranch ? 'ej2-nuget-release/' : isHotfixBranch ? 'ej2-nuget-hotfix/' : nugetPath;

gulp.task('ci-report', function (done) {
    done();
});

gulp.task('clean', () => {
    return gulp.src(['./Syncfusion.Blazor/Syncfusion.Blazor.csproj'], { read: false })
        .pipe(clean());
});

gulp.task('restore', gulp.series('clean', () => {
    return gulp.src(['./Syncfusion.Blazor/Syncfusion.Blazor.csproj'], { read: false })
        .pipe(restore({ echo: true }));
}));

gulp.task('blazor-index-scripts', function (done) {
    // Init script generation
    var initScript = fs.readFileSync(__dirname + '/import-script.template', 'utf8');
    initScript = initScript.replace(/{{PACKAGE}}/g, '');
    fs.writeFileSync(__dirname + '/../Scripts/modules/sf-import-script.js', initScript);

    var files = fs.readdirSync(__dirname + '/../Scripts/modules/');
    shelljs.mkdir('-p', __dirname + '/../Scripts/bundles/');
    var entries = {};
    files.forEach(moduleFile => {
        if (moduleFile !== 'base.js') {
            var moduleName = moduleFile.split('.js')[0];
            entries[moduleName] = `./bundles/${moduleFile}`;
            fs.writeFileSync(__dirname + '/../Scripts/bundles/' + moduleFile, `import '../modules/${moduleFile}';`);
        }
        else {
            entries['syncfusion-blazor'] = `./index.js`;
        }
    });
    fs.writeFileSync(__dirname + '/../Scripts/entries.json', JSON.stringify(entries, null, '\t'));
    var template = fs.readFileSync(__dirname + '/index-script.template', 'utf8');
    fs.writeFileSync(__dirname + '/../Scripts/index.js', template);
    done();
});

gulp.task('blazor-individual-scripts', function (done) {
    // Init script generation
    var initScript = fs.readFileSync(__dirname + '/import-script.template', 'utf8');
    initScript = initScript.replace(/{{PACKAGE}}/g, '.Core');
    fs.writeFileSync(__dirname + '/../Scripts/modules/sf-import-script.js', initScript);

    var files = fs.readdirSync(__dirname + '/../Scripts/modules/');
    shelljs.mkdir('-p', __dirname + '/../Scripts/bundles/');
    var entries = {};
    var packages = config.packages;
    files.forEach(moduleFile => {
        if (moduleFile.startsWith('sf-import-')) {
            return;
        }
        if (moduleFile !== 'base.js') {
            var packageName;
            var moduleName = moduleFile.split('.js')[0];
            for (var packName in packages) {
                if (packages[packName].scripts.indexOf(moduleName) !== -1) {
                    packageName = packages[packName].packageName ? packages[packName].packageName : packName;
                    moduleName = `Syncfusion.Blazor/${packName}/wwwroot/scripts/${moduleName}`;
                    break;
                }
            }
            if (packageName) {
                entries[moduleName] = `./bundles/${moduleFile}`;
                var bundleContent = `__webpack_public_path__ = "_content/Syncfusion.Blazor.${packageName}/scripts/";
import '../modules/${moduleFile}';`
                fs.writeFileSync(__dirname + '/../Scripts/bundles/' + moduleFile, bundleContent);
            }
        }
        else {
            entries['Syncfusion.Blazor/Base/wwwroot/scripts/syncfusion-blazor'] = `./index.js`;
            var template = `__webpack_public_path__ = "_content/Syncfusion.Blazor.Core/scripts/";\n` + fs.readFileSync(__dirname + '/index-script.template', 'utf8');
            fs.writeFileSync(__dirname + '/../Scripts/index.js', template);
        }
    });
    fs.writeFileSync(__dirname + '/../Scripts/individual-entries.json', JSON.stringify(entries, null, '\t'));
    done();
});

gulp.task('bundle', gulp.series('blazor-index-scripts', () => {
    shelljs.rm('-rf', 'Syncfusion.Blazor/wwwroot/scripts/');
    global.webPackConfig = require(__dirname + '/../webpack.config.js');
    global.webPackConfig.output.publicPath = '_content/Syncfusion.Blazor/scripts/';
    global.webPackConfig.output.path = path.resolve(__dirname, '..', 'Syncfusion.Blazor/wwwroot/scripts/');
    global.isIndividualNuGet = false;
    return gulp.src('Scripts/index.js')
        .pipe(webpack(global.webPackConfig, null, changeHashKey))
        .pipe(gulp.dest('Syncfusion.Blazor/wwwroot/scripts/'))
        .on('end', () => {
            shipImportScripts();
        });;
}));

gulp.task('blazor-single-script', (done) => {
    var exclude = config.singleBundle.exclude.map(item => 'Scripts/modules/' + item);
    var files = glob.sync('Scripts/modules/sf-*', { ignore: exclude });
    let imports = '';
    config.singleBundle.ej2Modules.forEach(file => {
        imports += `import "./modules/${file}";\n`;
    });
    imports += 'import "./syncfusion-blazor.js";\n';
    files.forEach((file) => {
        let fileName = path.basename(file);
        imports += `import "./modules/${fileName}";\n`;
    });
    fs.writeFileSync(__dirname + '/../Scripts/single-bundle.js', imports);

    imports = '';
    imports += 'import "./modules/base.js";\n';
    imports += 'import "./syncfusion-blazor.js";\n';
    config.CRG.SfPdfViewer.dependencies.forEach(file => {
        imports += `import "./modules/${file}";\n`;
    });
    fs.writeFileSync(__dirname + '/../Scripts/single-pdfviewer-bundle.js', imports);

    imports = '';
    
    imports += 'import "./modules/base.js";\n';
    imports += 'import "./syncfusion-blazor.js";\n';
    config.CRG.SfDocumentEditor.dependencies.forEach(file => {
        imports += `import "./modules/${file}";\n`;
    });
    fs.writeFileSync(__dirname + '/../Scripts/single-documenteditor-bundle.js', imports);


    done();
});

gulp.task('bundle-single-script', gulp.series('blazor-single-script', () => {
    global.webPackConfig = require(__dirname + '/../webpack-single-file.config.js');
    global.isIndividualNuGet = false;
    return gulp.src('Scripts/single-bundle.js')
        .pipe(webpack(global.webPackConfig))
        .pipe(gulp.dest('Scripts/modules/'))
        .on('end', () => {
            var source = __dirname + '/../Scripts/modules/syncfusion-blazor.min.js';
            var dest = __dirname + '/../Syncfusion.Blazor/wwwroot/scripts/';
            shelljs.cp('-rf', source, dest);
            if (fs.existsSync(__dirname + '/../Scripts/bundles/')) {
                shelljs.rm('-rf', __dirname + '/../Scripts/bundles/');
            }
        });;
}));

gulp.task('themes-bundle', (done) => {
    var themesPath = __dirname + '/../Syncfusion.Blazor/Themes/wwwroot/';
    if (fs.existsSync(themesPath)) {
        shelljs.rm('-rf', themesPath);
    }
    shelljs.mkdir('-p', themesPath);
    shelljs.cp('-R', __dirname + '/../Syncfusion.Blazor/wwwroot/styles/*', themesPath);
    done();
});

gulp.task('individual-bundle', gulp.series('blazor-individual-scripts', () => {
    var files = glob.sync('Syncfusion.Blazor/**/wwwroot/', { ignore: ['Syncfusion.Blazor/wwwroot/', 'Syncfusion.Blazor/Themes/wwwroot/'] });
    if (files.length) {
        shelljs.rm('-rf', files);
    }
    global.webPackConfig = require(__dirname + '/../webpack-individual.config.js');
    global.isIndividualNuGet = true;
    return gulp.src('Scripts/index.js')
        .pipe(webpack(global.webPackConfig, null, changeHashKey))
        .pipe(gulp.dest('./'))
        .on('end', () => {
            shipImportScripts();
            var source = __dirname + '/../Scripts/modules/syncfusion-blazor.min.js';
            var dest = __dirname + '/../Syncfusion.Blazor/Base/wwwroot/scripts/';
            shelljs.cp('-rf', source, dest);
            var pdfsourceFile = __dirname + '/../Scripts/modules/syncfusion-blazor-pdfviewer.min.js';
            var pdfdestFile = __dirname + '/../Syncfusion.Blazor/PdfViewer/wwwroot/scripts/';
            shelljs.cp('-rf', pdfsourceFile, pdfdestFile);
            var editorsourceFile = __dirname + '/../Scripts/modules/syncfusion-blazor-documenteditor.min.js';
            var editordestFile = __dirname + '/../Syncfusion.Blazor/DocumentEditor/wwwroot/scripts/';
            shelljs.cp('-rf', editorsourceFile, editordestFile);
        });
}));

function changeHashKey(error, result) {
    if (error) {
        console.log(error);
        return;
    }
    var serviceFile = __dirname + '/../Syncfusion.Blazor/Base/SyncfusionService.cs';
    var content = fs.readFileSync(serviceFile, 'utf8');
    content = content.replace(/internal string ScriptHashKey { get; set; } = (.*);/, `internal string ScriptHashKey { get; set; } = "${result.hash}";`);
    fs.writeFileSync(serviceFile, content);
}

function shipImportScripts() {
    var sourcePath = global.isIndividualNuGet ? '_content/Syncfusion.Blazor.Core/scripts/' : '_content/Syncfusion.Blazor/scripts/'
    var destPath = global.isIndividualNuGet ? 'Syncfusion.Blazor/Base/wwwroot/' : 'Syncfusion.Blazor/wwwroot/'
    shelljs.cp('-R', sourcePath, destPath);
    shelljs.rm('-rf', '_content');
}

gulp.task('build', gulp.series('bundle', 'bundle-single-script', 'restore', () => {
    var version = fs.readFileSync('./version.txt', 'utf8');
    console.log("compiling version@" + version);
    return gulp.src(['./Syncfusion.Blazor/Syncfusion.Blazor.csproj'], { read: false })
        .pipe(build({
            configuration: 'Release', version: version, echo: true, 
            msbuildArgs: ["/flp:Logfile=CodeAnalysisLog.log;Verbosity=Minimal"]
        })
    );
}));

gulp.task('change-nuspec', function (done) {
    console.log(process.argv);
    var file = fs.readFileSync(process.argv[4], 'UTF8');
    file = file.replace(/Release-XML/g, 'Debug');
    fs.writeFileSync(process.argv[4], file);
    done();
});

gulp.task('generate-nuget', gulp.series('clean-projects', (done) => {
    var version = fs.readFileSync('./version.txt', 'utf8');
    console.log(version);

    if (fs.existsSync(__dirname + '/../NuGet/')) {
        shelljs.rm('-rf', __dirname + '/../NuGet/');
    }
    shelljs.mkdir('-p', __dirname + '/../NuGet');

    var nugetConfig = fs.readFileSync(__dirname + '/../Syncfusion.Blazor/NuGet.config', 'utf8');
    if (nugetConfig.indexOf('"Separate NuGets"') === -1) {
        var packageSource = `  <add key="Separate NuGets" value="../NuGet" />
  </packageSources>`;
        nugetConfig = nugetConfig.replace('</packageSources>', packageSource);
        fs.writeFileSync(__dirname + '/../Syncfusion.Blazor/NuGet.config', nugetConfig);
    }

    var projects = config.projects;
    var index = process.argv.indexOf('--option');
    version = index !== -1 ? process.argv[index + 1] : version;
    projects.forEach(async (project) => {
        await generateNuGet(project, version, done);
    });
    shelljs.exec('gulp clean-projects');
    done();
}));

async function generateNuGet(project, version, done) {
    console.log('\n\n\n****************************************************************************************\n');
    console.log(project + '\n');
    console.log('****************************************************************************************');
    // This version processing only for CI integration and local NuGet package generation.
    // This is not necessary for Essential Studio (ES) build processing. i.e. It won't process for ES build.
    var packVersion = version ? ` /p:Version=${version}` : '';
    var requiredProps = ['GenerateDocumentationFile'];
    if (version) {
        console.log("Version - " + version);
        var projectContent = fs.readFileSync(`${__dirname}/../${project}`, 'utf8');
        var dependencies = projectContent.match(/\<PackageReference Include="Syncfusion.Blazor.(.*) \/>/g);
        // Change dependencies version in the current project file.
        if (dependencies && dependencies.length) {
            for (var i = 0; i < dependencies.length; i++) {
                var updatedDependency = dependencies[i].replace(/Version="(.*)"/, `Version="${version}"`);
                projectContent = projectContent.replace(dependencies[i], updatedDependency);
            }
            fs.writeFileSync(`${__dirname}/../${project}`, projectContent);
        }

        for (var i = 0; i < requiredProps.length; i++) {
            var regexProp = new RegExp(`<${requiredProps[i]}>`);
            var hasProp = projectContent.match(regexProp);
            if (!hasProp) {
                console.log(`${requiredProps[i]} property not found in ${project}`);
                process.exit(1);
            }
        }
    }
    await new Promise((resolve, reject) => {
        var restore = shelljs.exec(`dotnet restore ${project} --configfile "Syncfusion.Blazor/NuGet.config"`);
        if (version && restore.code) {
            reject(restore.stdout);
            done(restore.stdout);
            process.exit(1);
        }
        var pack = shelljs.exec(`dotnet pack ${project} -c Release --output NuGet${packVersion}`);
        if (version && pack.code) {
            reject(pack.stdout);
            done(pack.stdout);
            process.exit(1);
        }
        else {
            resolve();
        }
    });
}

gulp.task('local-pack', () => {
    var packJson = JSON.parse(fs.readFileSync('./package.json', 'utf-8'));
    var nugetVersion = packJson.version;
    return gulp.src('./Syncfusion.Blazor/Syncfusion.Blazor.csproj')
        .pipe(restore())
        .pipe(build())
        .pipe(pack({
            output: nupkgPath,
            noBuild: true,
            version: nugetVersion,
            noRestore: true,
            echo: true
        }));
});

//publish nuget packages to a nexus
gulp.task('publish-nuget', (done) => {
    if (process.env.BRANCH_NAME === 'master' || process.env.BRANCH_NAME === 'development' || isReleaseBranch || isHotfixBranch) {
        // shelljs.exec('gulp ship-interop', { silent: true });
        return gulp.src('./Nuget/*.nupkg', { read: false })
            .pipe(push({
                apiKey: process.env.EJ2_NEXUS_NUGET,
                source: 'http://nexus.syncfusion.com/repository/' + nugetPath
            }));
        done();
    } else {
        done();
    }
});

gulp.task('updatepack-version', function (done) {
    var pack = JSON.parse(fs.readFileSync('./package.json', 'utf-8'));
    var version = pack.version;
    var modifiedVersion = version.split('.');
    if ((parseInt(modifiedVersion[2]) + 1) > 99) {
        version = modifiedVersion[0] + '.' + (parseInt(modifiedVersion[2]) + 1) + '.' + 0;
    } else {
        version = modifiedVersion[0] + '.' + modifiedVersion[1] + '.' + (parseInt(modifiedVersion[2]) + 1);
    }
    pack.version = version;
    fs.writeFileSync('./package.json', JSON.stringify(pack, null, '\t'));
    done();
});

function shellDone(exitCode) {
    if (exitCode !== 0) {
        process.exit(1);
    }
}
exports.shellDone = shellDone;

gulp.task('ci-skip', function (done) {
    var simpleGit = require('simple-git')();
    simpleGit.log(function (err, log) {
        var stagingBranch = common.stagingBranch;
        if (process.env.BRANCH_NAME === stagingBranch) {
            var user = process.env.GITLAB_USER;
            var token = process.env.GITLAB_TOKEN;
            var origin = 'http://' + user + ':' + token + '@gitlab.syncfusion.com/essential-studio/' + common.currentRepo + '.git';
            shelljs.exec('git remote set-url origin ' + origin + ' && git pull origin ' + stagingBranch);
            shelljs.exec('git add -f package.json');
            shelljs.exec('git commit -m \"ci-skip(BLAZ-000): Branch merged and package is published [ci skip]\" --no-verify');
            shelljs.exec('git branch -f ' + stagingBranch + ' HEAD && git checkout ' + stagingBranch, shellDone);
            shelljs.exec('git push -f --set-upstream origin ' + stagingBranch + ' --no-verify', { silent: true }, function (exitCode) {
                done();
                shellDone(exitCode);
            });
        } else {
            done();
        }
    });
});

gulp.task('clean', function (done) {
    shelljs.rm('-rf', './Syncfusion.Blazor/bin/');
    shelljs.rm('-rf', './Syncfusion.Blazor/obj/');
    shelljs.rm('-rf', './bin/');
    done();
});

// FxCop integration  for DocumentEditor component.
gulp.task('clean-document-editor', () => {
    return gulp.src(['./Syncfusion.Blazor/DocumentEditor/Syncfusion.Blazor.DocumentEditor.csproj'], { read: false })
        .pipe(clean());
});

gulp.task('document-editor-reference-update', function (done) {
    var projectContent = fs.readFileSync('./Syncfusion.Blazor/DocumentEditor/Syncfusion.Blazor.DocumentEditor.csproj', 'utf8');
    var dependencies = projectContent.match(/\<PackageReference Include="Syncfusion.Blazor.(.*) \/>/g);
    
    if (dependencies && dependencies.length) {
        for (var i = 0; i < dependencies.length; i++) {
            var updateSourceDepen = dependencies[i].replace("<PackageReference", "<!--<PackageReference").replace(" />"," />-->") 
            projectContent = projectContent.replace(dependencies[i], updateSourceDepen); 
        }
        fs.writeFileSync('./Syncfusion.Blazor/DocumentEditor/Syncfusion.Blazor.DocumentEditor.csproj', projectContent);
    }
    shelljs.exec('dotnet add Syncfusion.Blazor/DocumentEditor/Syncfusion.Blazor.DocumentEditor.csproj reference Syncfusion.Blazor/Syncfusion.Blazor.csproj');
    done();
});

gulp.task('restore-document-editor', gulp.series('clean-document-editor', 'document-editor-reference-update', () => {
    return gulp.src(['./Syncfusion.Blazor/DocumentEditor/Syncfusion.Blazor.DocumentEditor.csproj'], { read: false })
        .pipe(restore({ echo: true, configfile:"Syncfusion.Blazor/NuGet.config" }));
}));

gulp.task('build-document-editor', gulp.series('restore-document-editor', () => {
    var version = fs.readFileSync('./version.txt', 'utf8');
    console.log("compiling version@" + version);
    return gulp.src(['./Syncfusion.Blazor/DocumentEditor/Syncfusion.Blazor.DocumentEditor.csproj'], { read: false })
        .pipe(build({
            configuration: 'Release', version: version, echo: true, noDependencies: true,
            msbuildArgs: ["/flp:Logfile=DocEditorCodeAnalysis.log;Verbosity=Minimal"]
        }))
        .on('end', () => {
            shelljs.exec('git restore Syncfusion.Blazor/DocumentEditor/*');
        });
}));

gulp.task('update-dependency', async (done) => {
    var command = process.argv[4];
    var FxCopPackName = 'Microsoft.CodeAnalysis.NetAnalyzers';
    await PackageUpdation('./Syncfusion.Blazor/Syncfusion.Blazor.csproj', command, FxCopPackName, done);
    if (command === 'add') {
        await PackageUpdation('./Syncfusion.Blazor/DocumentEditor/Syncfusion.Blazor.DocumentEditor.csproj', command, FxCopPackName, done);
    }
    done();
});

// Adding or removing the required NuGet packages.
async function PackageUpdation(path, command, packageName, done) {
    var gitStatus = shelljs.exec(`dotnet ${command} ${path} package ${packageName}`, { silent: true });
    if (gitStatus.code !== 0) {
        done();
        process.exit(1); 
    }
}