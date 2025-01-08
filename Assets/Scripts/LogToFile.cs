using System;
using System.IO;
using UnityEngine;

public class LogToFile : MonoBehaviour
{
    private string logFilePath;
    private StreamWriter logWriter;

    private const long MaxLogFileSize = 5 * 1024 * 1024;

    private void Awake()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "app_log.txt");

        CheckLogFileSize();

        logWriter = new StreamWriter(logFilePath, true);
        logWriter.AutoFlush = true;

        Application.logMessageReceived += HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string logEntry = $"{DateTime.Now}: [{type}] {logString}";
        if (type == LogType.Error || type == LogType.Exception)
        {
            logEntry += $"\nStackTrace: {stackTrace}";
        }

        logWriter.WriteLine(logEntry);
    }

    public void SendLogReport()
    {
      
        Debug.Log($"Log report saved at: {logFilePath}");

        Application.OpenURL($"file://{logFilePath}");
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
        logWriter?.Close();
    }
    private void CheckLogFileSize()
    {
        if (File.Exists(logFilePath))
        {
            FileInfo fileInfo = new FileInfo(logFilePath);
            if (fileInfo.Length > MaxLogFileSize)
            {
                logWriter.Close();
                File.Delete(logFilePath);
                logWriter = new StreamWriter(logFilePath, true);
                logWriter.AutoFlush = true;
                Debug.Log("Log file exceeded maximum size and was cleared.");
            }
        }
    }
}
