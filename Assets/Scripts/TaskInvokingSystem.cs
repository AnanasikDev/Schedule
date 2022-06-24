using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TaskInvokingSystem : MonoBehaviour
{
    [SerializeField] private Task PrefabTask;
    [SerializeField] private Transform TasksHandler;

    [SerializeField] private List<TaskStruct> TasksInfo;

    [SerializeField] private Vector3 origin = new Vector3(0, 0, 0);
    private float TaskHeight = 65;

    [SerializeField] private Canvas _Canvas;
    [SerializeField] private Button AddTaskButton;
    [SerializeField] private RectTransform TimeBarParent;
    [SerializeField] private RectTransform TimeBar;

    [Header("Read-only")]
    [SerializeField] private List<Task> Tasks;
    [SerializeField] private List<uint> timeThresholds; // Массив точек времени i, когда i-ая задача заканчивается и начинается i+1
    [SerializeField] private int CurrentTaskIndex = 0;
    [SerializeField] private uint TimeElapsed = 0;
    [SerializeField] private float scalePreserverance = 1f;

    public static TaskInvokingSystem instance { get; private set; }

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
    public void SetTimeSpeed(int speed)
    {
        Time.timeScale = speed;
    }

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

    private void TickUpdate()
    {
        Debug.Log(TimeElapsed);

        if (timeThresholds.Count == 0 || TimeElapsed >= timeThresholds[timeThresholds.Count - 1]) return;

        TimeElapsed++;  

        TimeBar.transform.localPosition += Vector3.down * (50 * scalePreserverance / TasksInfo[CurrentTaskIndex].DurationSeconds);

        float gapHeight = (TaskHeight * 1.1f * scalePreserverance) - 50 * scalePreserverance;
        EasyDebug.Log("gap height =", gapHeight);

        if (TimeElapsed >= timeThresholds[CurrentTaskIndex] && CurrentTaskIndex < Tasks.Count)
        {
            EasyDebug.Log($"Task {TasksInfo[CurrentTaskIndex].Title} : {Tasks[CurrentTaskIndex].TaskInfo.Title} finished on time {TimeElapsed}");

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

        AddTaskButton.transform.localPosition = new Vector3(0, origin.y + Tasks.Count * (-TaskHeight * 1.1f * scalePreserverance), 0);
        TimeBarParent.localPosition = Tasks[0].transform.localPosition + Vector3.right * -200;
        
        float gapHeight = (TaskHeight * 1.1f * scalePreserverance) - 50 * scalePreserverance;
        float height = Tasks.Count * 50 * scalePreserverance + gapHeight * (Tasks.Count - 1);
        //float height = gapHeight * (Tasks.Count - 1) + Tasks.Count * 50;

        //EasyDebug.Log("scale = ", scale, "gap height = ", gapHeight, "height = ", height);

        float y = (Tasks[0].transform.localPosition.y + Tasks[Tasks.Count - 1].transform.localPosition.y) / 2f;

        TimeBarParent.transform.localPosition = new Vector3(-200, y, 0); //gapHeight * (Tasks.Count - 1) + Tasks.Count * 50
        //TimeBarParent.localScale = new Vector3(1, scale, 1);
        TimeBarParent.sizeDelta = new Vector2(15, height);
        TimeBar.sizeDelta = new Vector2(15, height);
        
        
        TimeBar.localPosition = Vector3.up * TimeBar.rect.height;
        TimeElapsed = 0;


        CalculateTimeThresholds();

        //TimeBar.SetScaleForwardRelative(Vector3.down * scale + Vector3.right + Vector3.forward);

    }
    public void Restart()
    {
        TimeElapsed = 0;
        TimeBar.localPosition = Vector3.up * TimeBar.rect.height;
        TimeElapsed = 0;

        for (int i = 0; i < Tasks.Count; i++)
        {
            Tasks[i].ResetTask();
        }
    }
}



/*class LiveTileHelper
{
    [DllExport(CallingConvention.StdCall)]
    public static string UpdatePrimaryTile(string text, int durationSeconds = 10)
    {
        var template = Windows.UI.Notifications.TileTemplateType.TileSquare150x150Block;
        var tileXml = Windows.UI.Notifications.TileUpdateManager.GetTemplateContent(template);

        var tileTextAttributes = tileXml.GetElementsByTagName("text");
        tileTextAttributes[0].AppendChild(tileXml.CreateTextNode(text));

        var tileNotification = new Windows.UI.Notifications.TileNotification(tileXml);

        tileNotification.ExpirationTime = DateTime.Now.AddSeconds(durationSeconds);
        Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        return "Ok";
    }
}*/