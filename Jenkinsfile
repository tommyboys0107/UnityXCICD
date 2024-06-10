pipeline {
  agent any
  stages {
    stage('Initialize') {
      steps {
        echo "Clean workspace. (${WORK_SPACE})"
        dir(path: "${WORK_SPACE}") {
          sh 'git clean -f -d -x -e /[Ll]ibrary/'
        }

      }
    }

    stage('Build Win x64') {
      environment {
        UNITY_PATH = '"C:/Program Files/Unity/Hub/Editor/2020.3.11f1/Editor/Unity.exe"'
        UNITY_PROJECT_DIR = "${WORK_SPACE}"
        BUILD_TARGET = 'Win64'
        UNITY_BUILD_METHOD = 'CliffLeeCL.ProjectBuilder.BuildProject'
        OUTPUT_PATH = "${WORK_SPACE}/Artifacts"
        SYMBOL_CONFIG = 'Release'
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
  }
}