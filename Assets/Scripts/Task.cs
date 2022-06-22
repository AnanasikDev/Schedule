using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class Task : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI DurationTMPro;
    [SerializeField] private TextMeshProUGUI TitleTMPro;
    [SerializeField] private Toggle DoneToggle;

    public TaskStruct TaskInfo;

    public void OnSpawned()
    {
        DurationTMPro.text = TaskInfo.DurationSeconds.ToString();
        TitleTMPro.text = TaskInfo.Title;
        DoneToggle.isOn = false;
    }
    public void OnFinished()
    {
        DoneToggle.isOn = true;
    }
}
[Serializable]
public struct TaskStruct
{
    public uint DurationSeconds;
    public string Title;
    public string Description;

    public TaskStruct(uint durationSeconds, string title, string description)
    {
        DurationSeconds = durationSeconds;
        Title = title;
        Description = description;
    }
}
