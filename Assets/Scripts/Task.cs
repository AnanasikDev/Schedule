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

    public void SetTitle(string title)
    {
        TitleInputField.text = title;
        UpdateTitle();
    }
    public void SetDuration(uint duration)
    {
        DurationInputField.text = (duration / 60).ToString();
        UpdateDuration();
    }

    public void UpdateTitle()
    {
        TaskInfo.Title = TitleInputField.text;
    }
    public void UpdateDuration()
    {
        if (uint.TryParse(DurationInputField.text, out TaskInfo.DurationSeconds)) TaskInfo.DurationSeconds *= 60;
        else TaskInfo.DurationSeconds = 0;

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
    public void DeleteTask()
    {
        TaskInvokingSystem.instance.DeleteTask(this);
        Destroy(gameObject);
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
