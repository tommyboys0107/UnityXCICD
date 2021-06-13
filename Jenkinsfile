pipeline {
  agent any
  stages {
    stage('Initialize') {
      steps {
        echo 'Start build pipeline. Clean assets and artifacts folder.'
        dir(path: '"${WORKSPACE}"/Assets') {
          sh 'git clean -f -d -x'
        }

      }
    }

  }
}