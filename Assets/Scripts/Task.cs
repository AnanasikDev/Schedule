using UnityEngine;
using System;
using TMPro;

public class Task : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI DurationTMPro;
    [SerializeField] private TextMeshProUGUI TitleTMPro;

    public TaskStruct TaskInfo;

    public void OnSpawned()
    {
        DurationTMPro.text = TaskInfo.DurationSeconds.ToString();
        TitleTMPro.text = TaskInfo.Title;
    }
    public void OnFinished()
    {

    }
}
[Serializable]
public struct TaskStruct
{
    public int DurationSeconds;
    public string Title;
    public string Description;

    public TaskStruct(int durationSeconds, string title, string description)
    {
        DurationSeconds = durationSeconds;
        Title = title;
        Description = description;
    }
}
