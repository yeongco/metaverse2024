using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DiaryControl : MonoBehaviour
{
    string filePath; // 파일 경로
    private string[] lines;
    private int currentIndex = 0;

    public Text displayText; // UI 텍스트 컴포넌트

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "savedDiaries.txt");
        if (File.Exists(filePath))
        {
            Debug.Log("파일 발견");
            lines = File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {
                displayText.text = lines[currentIndex];
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }

    public void NextLine()
    {
        if (lines != null && lines.Length > 0)
        {
            currentIndex++;
            if (currentIndex >= lines.Length)
            {
                currentIndex = 0; // 마지막 줄을 지나면 처음으로 돌아갑니다.
            }
            displayText.text = lines[currentIndex];
        }
    }

    public void PreviousLine()
    {
        if (lines != null && lines.Length > 0)
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = lines.Length - 1; // 첫 줄을 지나면 마지막으로 돌아갑니다.
            }
            displayText.text = lines[currentIndex];
        }
    }
}
