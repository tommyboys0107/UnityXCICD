pipeline {
  agent any
  stages {
    stage('Initialize') {
      steps {
        echo "Clean workspace. (${WORK_SPACE})"
        dir(path: "${WORK_SPACE}") {
          sh 'git clean -f -d -x -e /[Ll]ibrary/'
        }

        sh 'curl -X POST https://api.telegram.org/bot$TELEGRAM_BOT_TOKEN/sendMessage -d parse_mode=markdown -d chat_id=987110561 -d text='+"\"Build pipeline starts! - *${JOB_NAME} #${BUILD_NUMBER}* - ([Go to Jenkins](${BUILD_URL}))\""
      }
    }

    stage('Build') {
      environment {
        SYMBOL_CONFIG = 'Release'
        BUILD_TARGET = 'Win64'
        UNITY_BUILD_METHOD = 'CliffLeeCL.ProjectBuilder.BuildProject'
      }
      steps {
        echo "Build ${SYMBOL_CONFIG} ${BUILD_TARGET} with Unity (${UNITY_PATH})"
        echo "Project path: ${UNITY_PROJECT_DIR}"
        echo "Output path: ${OUTPUT_PATH}"
        sh "${UNITY_PATH} -projectPath ${UNITY_PROJECT_DIR} -buildTarget ${BUILD_TARGET} -executeMethod ${UNITY_BUILD_METHOD} -logFile - -quit -batchmode -nographics -outputPath ${OUTPUT_PATH} -defineSymbolConfig ${SYMBOL_CONFIG}"
      }
    }

    stage('Deploy') {
      steps {
        archiveArtifacts 'Artifacts/**'
      }
    }

  }
  environment {
    WORK_SPACE = "${WORKSPACE}".replace("\\", "/")
    OUTPUT_PATH = "${WORK_SPACE}/Artifacts"
    UNITY_PATH = '"C:/Program Files/Unity/Hub/Editor/2020.3.11f1/Editor/Unity.exe"'
    UNITY_PROJECT_DIR = "${WORK_SPACE}"
    TELEGRAM_BOT_TOKEN = credentials('CliffTelegramBotToken')
  }
  post {
    success {
      sh 'curl -X POST https://api.telegram.org/bot$TELEGRAM_BOT_TOKEN/sendMessage -d parse_mode=markdown -d chat_id=987110561 -d text='+"\"Build pipeline ends successfully! - *${JOB_NAME} #${BUILD_NUMBER}* - ([Go to Jenkins](${BUILD_URL}))\""
    }

    unstable {
      sh 'curl -X POST https://api.telegram.org/bot$TELEGRAM_BOT_TOKEN/sendMessage -d parse_mode=markdown -d chat_id=987110561 -d text='+"\"Build pipeline ends but it's unstable! - *${JOB_NAME} #${BUILD_NUMBER}* - ([Go to Jenkins](${BUILD_URL}))\""
    }

    aborted {
      sh 'curl -X POST https://api.telegram.org/bot$TELEGRAM_BOT_TOKEN/sendMessage -d parse_mode=markdown -d chat_id=987110561 -d text='+"\"Build pipeline aborted! - *${JOB_NAME} #${BUILD_NUMBER}* - ([Go to Jenkins](${BUILD_URL}))\""
    }

    failure {
      sh 'curl -X POST https://api.telegram.org/bot$TELEGRAM_BOT_TOKEN/sendMessage -d parse_mode=markdown -d chat_id=987110561 -d text='+"\"Build pipeline failed! - *${JOB_NAME} #${BUILD_NUMBER}* - ([Go to Jenkins](${BUILD_URL}))\""
    }

  }
  triggers {
    pollSCM('H H(0-5) * * *')
  }
}