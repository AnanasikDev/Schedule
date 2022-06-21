using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

public class TaskInvokingSystem : MonoBehaviour
{
    [SerializeField] private Task PrefabTask;
    [SerializeField] private Transform TasksHandler;

    //[Header("Read-Only")]
    [SerializeField] private List<TaskStruct> TasksInfo;
    [SerializeField] private List<Task> Tasks;
    [SerializeField, Tooltip("Debug-view-only")] private int CurrentTaskIndex = 0;

    [SerializeField] private Vector3 origin = new Vector3(0, 200, 0);
    private float TaskHeight = 90;

    [SerializeField] private Canvas _Canvas;

    private void Start()
    {
        Application.runInBackground = true;

        /*Tasks = new List<Task>()
        {
            new Task(15, "Разминка", ""),
            new Task(20, "Работа над проектом 1", ""),
            new Task(10, "Разминка", ""),
            new Task(12, "Работа над проектом 2", "")
        };*/

        GenerateTasksGUI();

        StartCoroutine(LoopTasks());
    }

    private void GenerateTasksGUI()
    {
        for (int i = 0; i < TasksInfo.Count; i++)
        {
            float scalePreserverance = 800.0f / _Canvas.GetComponent<RectTransform>().rect.width;

            float position = origin.y + i * (-TaskHeight * 1.1f * scalePreserverance);

            Task task = Instantiate(PrefabTask, Vector3.zero, Quaternion.identity, TasksHandler) as Task;
            task.transform.localPosition = new Vector3(origin.x, position, origin.z);
            task.TaskInfo = TasksInfo[i];
            task.OnSpawned();

            Tasks.Add(task);
        }
    }

    private IEnumerator LoopTasks()
    {
        if (TasksInfo.Count == 0 || CurrentTaskIndex >= TasksInfo.Count) yield break;

        yield return new WaitForSecondsRealtime(TasksInfo[CurrentTaskIndex].DurationSeconds);
        NotificationSystem.instance.Notify();
        Tasks[CurrentTaskIndex].OnFinished();

        CurrentTaskIndex++;

        yield return LoopTasks();
    }

    /*public void CreateTask()
    {
        float position = Tasks.Count * (TaskHeight * 1.1f);

        Task task = Instantiate(PrefabTask, new Vector3(origin.x, position, origin.z), Quaternion.identity, TasksHandler);
        Tasks.Add(task);
        OnTaskAdded?.Invoke();
    }*/
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