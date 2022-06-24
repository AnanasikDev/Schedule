using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class Task : MonoBehaviour
{

    [SerializeField] private TMP_InputField DurationInputField;
    [SerializeField] private TMP_InputField TitleInputField;
    [SerializeField] private Toggle DoneToggle;

    public TaskStruct TaskInfo;

    public void ResetTask()
    {
        DoneToggle.isOn = false;
    }
    public void UpdateTitle()
    {
        TaskInfo.Title = TitleInputField.text;
    }
    public void UpdateDuration()
    {
        TaskInfo.DurationSeconds = (uint)Convert.ToInt32(DurationInputField.text) * 60;
        TaskInvokingSystem.instance.CalculateTimeThresholds();
    }
    public void OnSpawned()
    {
        DurationInputField.text = TaskInfo.DurationSeconds.ToString();
        TitleInputField.text = TaskInfo.Title;
        DoneToggle.isOn = false;
    }
    public void OnFinished()
    {
        DoneToggle.isOn = true;
    }
}
[Serializable]
public class TaskStruct
{
    public uint DurationSeconds = 30;
    public string Title = "Task";
    public string Description = "";

    public TaskStruct()
    {

    }
    public TaskStruct(uint durationSeconds, string title, string description)
    {
        DurationSeconds = durationSeconds;
        Title = title;
        Description = description;
    }
}
