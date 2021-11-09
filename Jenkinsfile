#!groovy

node('AspComponents') {
    try {
        deleteDir();
        stage('Import') {
            git url: 'http://gitlab.syncfusion.com/essential-studio/ej2-groovy-scripts.git', branch: 'master', credentialsId: env.JENKINS_CREDENTIAL_ID;
            shared = load 'src/shared.groovy';
        }
        stage('Checkout') {
            checkout scm;
            shared.gitlabCommitStatus('running');
        }

        if(checkCommitMessage()) {
            stage('Install') {
                runShell('git config --global user.email "essentialjs2@syncfusion.com"');
                runShell('git config --global user.name "essentialjs2"');
                runShell('git config --global core.longpaths true');
                runShell('npm -v');
                runShell('npm install');
            }

            stage('Build') {
                runShell('gulp update-dependency --option add');
                runShell('gulp build');
                runShell('gulp build-document-editor');
                runShell('gulp update-dependency --option remove');
            }

            stage('Code Analysis') {
                runShell('gulp code-analysis-report');
                runShell('gulp document-editor-code-analysis-report');
            }

            stage('Generate NuGet') {
                runShell('gulp generate-nuget');
            }

            stage('Publish') {
                if(shared.isProtectedBranch()) {
                    archiveArtifacts artifacts: 'Nuget/', excludes: null;
                    runShell('gulp publish-nuget');
                }
            }
        }
        shared.gitlabCommitStatus('success');
    }
                
    catch(Exception e) {
        archiveArtifacts artifacts: 'Nuget/', excludes: null;
        deleteDir();
        error(e);
    }
}

def runShell(String command) {
    if(isUnix()) {
        sh command;
    }
    else {
        bat command;
    }
}

// Check commit message for build setup
def checkCommitMessage() {
    def msg = executeCommand("git log -1 --pretty=%%B");
    println('Commit Message: ' + msg);
    if(msg.indexOf('[ci skip]') != -1) {
        println('CI SKIPPED');
        return false;
    }
    return true;
}

// Execute commands and retrieve the output
def executeCommand(String command) {
    if (isUnix()) {
        sh(script: 'set +x && ' + command + ' > tempFile', returnStdout: true);
    }
    else {
        bat(script: command + ' > tempFile', returnStdout: true);
    }    
    return readFile('tempFile').trim();
}
