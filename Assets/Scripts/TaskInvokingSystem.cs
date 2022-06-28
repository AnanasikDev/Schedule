using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class TaskInvokingSystem : MonoBehaviour
{
    [SerializeField] private Task PrefabTask;
    [SerializeField] private Transform TasksHandler;

    [SerializeField] private List<TaskStruct> TasksInfo;

    [SerializeField] private Vector3 origin = new Vector3(0, 0, 0);
    [SerializeField] private float TimeBarShift = -200;
    private float TaskHeight = 60;
    [SerializeField] private uint TasksScrollThreshold = 3;
    [SerializeField] private float ScrollingSpeed = 8f;
    [SerializeField] private string TasksAmountTextTemplate = "{0} tasks"; // 0 - amount

    [SerializeField] private Canvas _Canvas;
    [SerializeField] private Button AddTaskButton;
    [SerializeField] private RectTransform TimeBarParent;
    [SerializeField] private RectTransform TimeBar;
    [SerializeField] private TextMeshProUGUI TimeElapsedTMPro;
    [SerializeField] private TextMeshProUGUI TasksAmountTMPro;
    [SerializeField] private TMP_InputField TitleInputField;

    [Header("Read-only")]
    public List<Task> Tasks;
    [SerializeField] private List<uint> timeThresholds; // Массив точек времени i, когда i-ая задача заканчивается и начинается i+1
    [SerializeField] private int CurrentTaskIndex = 0;
    [SerializeField] private uint TimeElapsed = 0;
    [SerializeField] private float scalePreserverance = 1f;
    [SerializeField] private bool Repeating = false;
    [SerializeField] private float gapHeight;
    public string Title;

    public static TaskInvokingSystem instance { get; private set; }

    public void SetTitle(string title)
    {
        Title = title;
        TitleInputField.text = Title;
    }
    public void UpdateTitle() => Title = TitleInputField.text;

    public void DeleteTask(Task task)
    {
        Tasks.Remove(task);
        TasksInfo.Remove(task.TaskInfo);
        TasksAmountTMPro.text = string.Format(TasksAmountTextTemplate, Tasks.Count);

        RecalculateTasksPositions();

        CalculateTimeThresholds();
        Restart();
        SetUpTimeBar();
    }
    public void CalculateTimeThresholds()
    {
        TimeBar.localPosition = Vector3.up * TimeBar.rect.height;
        TimeElapsed = 0;

        timeThresholds = new List<uint>();
        uint thr = 0;
        for (int i = 0; i < TasksInfo.Count; i++)
        {
            thr += TasksInfo[i].DurationSeconds;
            timeThresholds.Add(thr);
        }
    }
    public void SetRepeating(bool repeating) => Repeating = repeating;

    private void Start()
    {
        instance = this;

        //scalePreserverance = _Canvas.GetComponent<CanvasScaler>().referenceResolution.x / _Canvas.GetComponent<RectTransform>().rect.width;
        //EasyDebug.Log("Scale Preservance = ", scalePreserverance, _Canvas.GetComponent<CanvasScaler>().referenceResolution.x, _Canvas.GetComponent<RectTransform>().rect.width);

        scalePreserverance = _Canvas.GetComponent<CanvasScaler>().scaleFactor;

        CalculateTimeThresholds();

        Application.runInBackground = true;

        GenerateTasksGUI();

        TimeBar.localPosition = Vector3.up * TimeBar.rect.height;

        InvokeRepeating("TickUpdate", 1, 1);
    }
    private void Update()
    {
        if (Tasks.Count >= TasksScrollThreshold)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                TasksHandler.transform.localPosition =
                    new Vector3
                    (
                        TasksHandler.localPosition.x,
                        Mathf.Clamp(TasksHandler.localPosition.y + -Hexath.SnapNumberToStep(Input.mouseScrollDelta.y, 1) * ScrollingSpeed, 0, (Tasks.Count - TasksScrollThreshold) * TaskHeight + gapHeight),
                        TasksHandler.localPosition.z
                    );
            }
        }
    }
    private void TickUpdate()
    {
        if (timeThresholds.Count == 0) return;

        if (TimeElapsed >= timeThresholds[timeThresholds.Count - 1])
        {
            if (Repeating)
                Restart();
            return;
        }

        TimeElapsed++;

        TimeElapsedTMPro.text = Math.Round(TimeElapsed / 60f, 2).ToString() + " mins elapsed";

        TimeBar.transform.localPosition += Vector3.down * (50 * scalePreserverance / TasksInfo[CurrentTaskIndex].DurationSeconds);

        gapHeight = (TaskHeight * 1.1f * scalePreserverance) - 50 * scalePreserverance;

        if (TimeElapsed >= timeThresholds[CurrentTaskIndex] && CurrentTaskIndex < Tasks.Count)
        {
            NotificationSystem.instance.Notify();
            Tasks[CurrentTaskIndex].OnFinished();

            CurrentTaskIndex++;
            if (CurrentTaskIndex >= Tasks.Count) return;

            TimeBar.transform.localPosition += Vector3.down * gapHeight;
        }
    }

    private void GenerateTasksGUI()
    {
        for (int i = 0; i < TasksInfo.Count; i++)
        {
            CreateTask();
        }
    }

    public void CreateTask(string title, uint duration)
    {
        CreateTask();
        Task t = Tasks[Tasks.Count - 1];
        t.SetTitle(title);
        t.SetDuration(duration);
    }

    public void CreateTask()
    {
        //scalePreserverance = _Canvas.GetComponent<CanvasScaler>().referenceResolution.x / _Canvas.GetComponent<RectTransform>().rect.width;
        scalePreserverance = _Canvas.GetComponent<CanvasScaler>().scaleFactor;

        float position = origin.y + Tasks.Count * (-TaskHeight * 1.1f * scalePreserverance);

        Task task = Instantiate(PrefabTask, Vector3.zero, Quaternion.identity, TasksHandler).GetComponent<Task>();
        task.transform.localPosition = new Vector3(origin.x, position, origin.z);
        task.TaskInfo = new TaskStruct();
        task.OnSpawned();

        Tasks.Add(task);
        TasksInfo.Add(task.TaskInfo);

        TasksAmountTMPro.text = string.Format(TasksAmountTextTemplate, Tasks.Count);

        SetUpAddTaskButton();

        SetUpTimeBar();

        CalculateTimeThresholds();
    }
    private void SetUpAddTaskButton()
    {
        AddTaskButton.transform.localPosition = new Vector3(0, origin.y + Tasks.Count * (-TaskHeight * 1.1f * scalePreserverance), 0);
    }
    private void SetUpTimeBar()
    {
        if (Tasks.Count == 0)
        {
            TimeBarParent.sizeDelta = Vector2.zero;
            return;
        }

        //TimeBarParent.localPosition = Tasks[0].transform.localPosition + Vector3.right * -200;

        gapHeight = (TaskHeight * 1.1f * scalePreserverance) - 50 * scalePreserverance;
        float height = Tasks.Count * 50 * scalePreserverance + gapHeight * (Tasks.Count - 1);
        float y = (Tasks[0].transform.localPosition.y + Tasks[Tasks.Count - 1].transform.localPosition.y) / 2f;
        // y - средняя позиция между первой и последней задачей

        TimeBarParent.localPosition = new Vector3(TimeBarShift, y, 0);
        TimeBarParent.sizeDelta = new Vector2(15, height);
        TimeBar.sizeDelta = new Vector2(15, height);

        // Сброс времени
        TimeBar.localPosition = Vector3.up * TimeBar.rect.height;
        TimeElapsed = 0;
    }
    public void RecalculateTasksPositions()
    {
        for (int i = 0; i < Tasks.Count; i++)
        {
            scalePreserverance = _Canvas.GetComponent<CanvasScaler>().scaleFactor;

            float position = origin.y + i * (-TaskHeight * 1.1f * scalePreserverance);

            Tasks[i].transform.localPosition = new Vector3(origin.x, position, origin.z);
        }
        SetUpAddTaskButton();
    }
    public void Flush()
    {
        for (int i = Tasks.Count-1; i >= 0; i--)
            Tasks[i].DeleteTask();

        Tasks.Clear();
        TasksInfo.Clear();
        timeThresholds.Clear();
    }
    public void Restart()
    {
        TimeElapsedTMPro.text = "0 mins elapsed";

        TimeElapsed = 0;
        TimeBar.localPosition = Vector3.up * TimeBar.rect.height;
        CurrentTaskIndex = 0;

        for (int i = 0; i < Tasks.Count; i++)
        {   
            Tasks[i].ResetTask();
        }
    }
}
