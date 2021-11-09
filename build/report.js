var fs = require('fs');
var gulp = require('gulp');
var table = require('table');

gulp.task('code-analysis-report', function (done) {
    var reportLog = fs.readFileSync(__dirname + '/../CodeAnalysisLog.log', 'utf8');
    var warnings = reportLog.match(/(.*?) warning (.*)/g);
    if (!warnings) {
        console.log("!!! No Warning Throws !!!");
    } else {
    console.log('\nTotal Warnings: ' + warnings.length + '\n\n');
    var tableData = [];
    var totalControls = [];
    for (var i = 0; i < warnings.length; i++) {
        try {
            var fileName = warnings[i].match(/(.*)\)\: /)[0].replace(':', '').trim();
            var controlName = fileName.split("Syncfusion.Blazor")[1].split("\\")[1];
            var warningCode = warnings[i].match(/\: \warning (.*)\: (\'[a-zA-Z]|[a-zA-Z])/)[1];
            var descRegex = new RegExp(`${warningCode}: (.*)`);
            var description = warnings[i].match(descRegex)[1].replace(/\[(.*)\]/, '').trim();
            tableData.push([controlName, fileName, warningCode, description]);
            if (totalControls.indexOf(controlName) === -1)
            {
                totalControls.push(controlName);
            }        
        }
        catch(e) {
            console.log('\n' + warnings[i] + '\n');
            console.log('\n' + e + '\n');
        }
    }
    var config = {
        columns: {
            0: {
                width: 15
            },
            1: {
                width: 50
            },
            2: {
                width: 10
            },
            3: {
                width: 150
            }
        }
    };
    tableOutput = table.table(tableData, config);
    if (!fs.existsSync(__dirname + '/../Report/')) {
        shelljs.mkdir('-p', __dirname + '/../Report/');
    }
    controlWiseWarnings(tableData, totalControls);
    fs.writeFileSync(__dirname + '/../Report/CodeAnalyzer.txt', tableOutput);
    fs.unlinkSync(__dirname + '/../CodeAnalysisLog.log');
    if (tableData.length) {
       throw "Please resolve the above warnings to succeed the code analysis";     
    }
    }
    done();
});

// Generate code analysis report for document editor alone.
gulp.task('document-editor-code-analysis-report', function (done) {
    var reportLog = fs.readFileSync(__dirname + '/../DocEditorCodeAnalysis.log', 'utf8');
    var warnings = reportLog.match(/(.*?) warning (.*)/g);
    if (!warnings) {
        console.log("!!! No Warning Throws !!!");
    } else {
        console.log('\nTotal Warnings: ' + warnings.length + '\n\n');
        var tableData = [];
        var totalControls = [];
        for (var i = 0; i < warnings.length; i++) {
            try {
                var fileName = warnings[i].match(/(.*)\)\: /)[0].replace(':', '').trim();
                var controlName = fileName.split("Syncfusion.Blazor")[1].split("\\")[1];
                var warningCode = warnings[i].match(/\: \warning (.*)\: (\'[a-zA-Z]|[a-zA-Z])/)[1];
                var descRegex = new RegExp(`${warningCode}: (.*)`);
                var description = warnings[i].match(descRegex)[1].replace(/\[(.*)\]/, '').trim();
                tableData.push([controlName, fileName, warningCode, description]);
                if (totalControls.indexOf(controlName) === -1)
                {
                    totalControls.push(controlName);
                }        
            }
            catch(e) {
                console.log('\n' + warnings[i] + '\n');
                console.log('\n' + e + '\n');
            }
        }
        var config = {
            columns: {
                0: {
                    width: 15
                },
                1: {
                    width: 50
                },
                2: {
                    width: 10
                },
                3: {
                    width: 150
                }
            }
        };
        tableOutput = table.table(tableData, config);
        if (!fs.existsSync(__dirname + '/../Report/')) {
            shelljs.mkdir('-p', __dirname + '/../Report/');
        }
        controlWiseWarnings(tableData, totalControls);
        fs.unlinkSync(__dirname + '/../DocEditorCodeAnalysis.log');
        if (tableData.length) {
            throw "Please resolve the above warnings to succeed the code analysis";
        }
    }
    done();
});

function controlWiseWarnings(tData, tControls) {
    var tabData = [];
    var config = {
        columns: {
            0: {
                width: 5
            },
            1: {
                width: 15
            },
            2: {
                width: 50
            },
            3: {
                width: 10
            },
            4: {
                width: 150
            }
        }
    };
    for(var i = 0; i <tControls.length; i++){
        var count = 0;
        for(var j = 0; j <tData.length; j++){
            if(tData[j][0] === tControls[i]){
                tabData.push([count++ + 1, tData[j][0], tData[j][1], tData[j][2], tData[j][3]]);
            }
        }
        fs.writeFileSync(__dirname + '/../Report/'+ tControls[i] +'.txt', table.table(tabData, config));
        console.log(table.table(tabData, config));
        count=0;
        tabData.length = 0;
    }

}
