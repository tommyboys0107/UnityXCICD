pipeline {
  agent any
  stages {
    stage('Initialize') {
      steps {
        echo "Clean assets folder. (${WORKSPACE}/Assets)"
        dir(path: "${WORKSPACE}/Assets") {
          sh 'git clean -f -d -x'
        }

      }
    }

    stage('Build Win x64') {
      steps {
        echo "Build Win x64 with Unity (${UNITY_PATH})"
        sh "${UNITY_PATH} \
                  -projectPath ${UNITY_PROJECT_DIR} \
                  -buildTarget Win64 \
                  -executeMethod ${UNITY_BUILD_METHOD} \
                  -logFile ${UNITY_OUTPUT_PATH}/UnityBuildLog.txt \
                  -quit -batchmode -nographics \
                  -outputPath ${UNITY_OUTPUT_PATH} \
                  -defineSymbolConfig Release"
      }
    }

  }
  environment {
    UNITY_PATH = 'C:\\Program Files\\Unity\\Hub\\Editor\\2020.3.11f1\\Editor\\Unity.exe'
    UNITY_PROJECT_DIR = "${WORKSPACE}"
    UNITY_BUILD_METHOD = 'CliffLeeCL.ProjectBuilder.BuildProject'
    UNITY_OUTPUT_PATH = "${WORKSPACE}/Artifacts"
  }
}