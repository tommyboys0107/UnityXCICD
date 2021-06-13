pipeline {
  agent any
  stages {
    stage('Initialize') {
      steps {
        echo "Start build pipeline. Clean assets and artifacts folder. ${WORKSPACE} ${env.WORKSPACE} ${TEST} ${env.TEST}"
        dir(path: "${WORKSPACE}/Assets") {
          sh 'git clean -f -d -x'
        }

      }
    }

  }
  environment {
    TEST = 'test'
  }
}