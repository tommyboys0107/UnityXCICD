# 目錄
- [簡介](#簡介)
- [議程影片連結](#議程影片連結)
- [投影片連結](#投影片連結)
- [Unity 專案](#Unity-專案)
  - [Project Builder 相關重點整理](#Project-Builder-相關重點整理)
- [最小可行 CI](#最小可行-CI)

# 簡介
此專案是我為2021 TGDF 的議程《別再讓出版本綁架你的時間！CI & CD - Unity X Jenkins》所製作，主要是使用 Jenkins automation server 搭配 Unity 遊戲引擎製作最小可行 CI 流程，Repository 中包含之前曾製作過的《打擊感實驗室》Unity 專案，以及 Jenkins pipeline script - Jenkinsfile，在 Jenkins 中可以直接以這個專案學習 CI 流程的串接與 pipeline script 寫法。

# 議程影片連結
目前可在 IGDATaiwan YouTube 上看到
https://www.youtube.com/watch?v=G-ri4fHKFo8

# 投影片連結
https://cliffleeclstudio.pse.is/3htpqg

# Unity 專案
在本 Unity 專案中，主要要參考的是 *Assets/CliffLeeCL/Script/Editor/ProjectBuilder.cs*，功能是在讓專案可以透過外部呼叫 command line 建置專案，這步完成後就可以在串接 Jenkins 時很快就能接入。
## Project Builder 相關重點整理
- ProjectBuilder class 要放在 Editor 資料夾內
- 給外部執行的 function，要是 public class 與 public static function
- Environment.GetCommandLineArgs 抓取自訂參數，方便微調專案設定
- 建置結果狀態碼不是 0 即代表建置失敗，Jenknis 也會失敗中斷

# 最小可行 CI
在這議程中我也有提出一個最小可行 CI 流程，是在最初實作 CI 概念時，可以先實作最小可行 CI 流程，就可以把觸發建置到取得產物自動化，後續就可以再從此延伸成更理想穩健的 CI 流程。
![最小可行CI](https://user-images.githubusercontent.com/8335755/124946707-d1114c80-e041-11eb-8498-5d403dc74eeb.png)
