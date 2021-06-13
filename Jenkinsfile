pipeline {
  agent any
  stages {
    stage('Initialize') {
      steps {
        echo 'Start build pipeline.'
        echo "Clean assets folder. (${WORKSPACE}/Assets)"
        dir(path: "${WORKSPACE}/Assets") {
          sh 'git clean -f -d -x'
        }

      }
    }

  }
}