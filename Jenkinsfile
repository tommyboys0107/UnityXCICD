pipeline {
  agent any
  stages {
    stage('Initialize') {
      steps {
        echo 'Start build pipeline. Clean assets and artifacts folder. ${WORKSPACE} ${env.WORKSPACE} "${WORKSPACE}" "${env.WORKSPACE}" ${TEST} ${env.TEST} "${TEST}" "${env.TEST}"'
        dir(path: 'Assets') {
          sh 'git clean -f -d -x'
        }

      }
    }

  }
  environment {
    TEST = 'test'
  }
}