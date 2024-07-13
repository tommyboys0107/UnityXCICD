[![Build Unity Project](https://github.com/tommyboys0107/UnityXCICD/actions/workflows/build-unity-project.yml/badge.svg?branch=main)](https://github.com/tommyboys0107/UnityXCICD/actions/workflows/build-unity-project.yml)
# 目錄
- [簡介](#簡介)
- [議程影片](#議程影片)
- [最小可行 CI/CD](#最小可行-CI/CD)
- [投影片](#投影片)
- [Unity 專案](#Unity-專案)
  - [Project Builder 相關重點整理](#Project-Builder-相關重點整理)

# 簡介
此專案包含兩個議程的實作內容
- 2024 TGDF 的議程《CI/CD - Unity X GitHub Actions》，使用 GitHub Actions 搭配 Unity 遊戲引擎製作最小可行 CI/CD，包含 workflow file。
- 2021 TGDF 的議程《別再讓出版本綁架你的時間！CI & CD - Unity X Jenkins》，使用 Jenkins 搭配 Unity 遊戲引擎製作最小可行 CI/CD，包含 Jenkins pipeline script - Jenkinsfile。
- 包含《打擊感實驗室》Unity 範例專案，提供 Unity Project Builder 可以給外部 CI/CD 工具呼叫。

# 議程影片
可在 IGDATaiwan YouTube 上看到
- 2024 TGDF《CI/CD - Unity X GitHub Actions》：待釋出後補上
- [2021 TGDF《別再讓出版本綁架你的時間！CI & CD - Unity X Jenkins》](https://www.youtube.com/watch?v=G-ri4fHKFo8)

# 投影片
- [2024 TGDF《CI/CD - Unity X GitHub Actions》](https://cliffleeclstudio.pse.is/UnityXGitHubActions)
- [2021 TGDF《別再讓出版本綁架你的時間！CI & CD - Unity X Jenkins》](https://cliffleeclstudio.pse.is/3htpqg)

# 最小可行 CI/CD
在這議程中我也提出一個最小可行 CI/CD ，是在最初實作 CI/CD 概念時，可以先實作最小可行 CI/CD 流程，就可以把觸發建置到取得產物自動化，後續就可以再從此延伸成更理想穩健的 CI 流程。
## Unity X GitHub Actions
![Unity X GitHub Actions 最小可行 CI/CD](https://github.com/user-attachments/assets/a1458049-9a02-4cca-96fe-0bad83343b8d)

## Unity X Jenkins
![Unity X Jenkins 最小可行 CI/CD](https://user-images.githubusercontent.com/8335755/124946707-d1114c80-e041-11eb-8498-5d403dc74eeb.png)

# Unity 專案
在本 Unity 專案中，主要要參考的是 *Assets/CliffLeeCL/Script/Editor/ProjectBuilder.cs*，功能是在讓專案可以透過外部呼叫 command line 建置專案，這步完成後就可以在串接 CI/CD 工具時很快就能接入。
## Project Builder 相關重點整理
- ProjectBuilder class 要放在 Editor 資料夾內
- 給外部執行的 function，要是 public class 與 public static function
- Environment.GetCommandLineArgs 抓取自訂參數，方便微調專案設定
