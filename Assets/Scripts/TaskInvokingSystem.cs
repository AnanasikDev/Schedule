using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TaskInvokingSystem : MonoBehaviour
{
    [SerializeField] private Task PrefabTask;
    [SerializeField] private Transform TasksHandler;

    [SerializeField] private List<TaskStruct> TasksInfo;
    [SerializeField] private List<Task> Tasks;
    [SerializeField, Tooltip("Debug-view-only")] private int CurrentTaskIndex = 0;

    [SerializeField] private Vector3 origin = new Vector3(0, 200, 0);
    private float TaskHeight = 65;

    [SerializeField] private Canvas _Canvas;
    [SerializeField] private Button AddTaskButton;

    private void Start()
    {
        Application.runInBackground = true;

        GenerateTasksGUI();

        StartCoroutine(LoopTasks());
    }

    private void GenerateTasksGUI()
    {
        for (int i = 0; i < TasksInfo.Count; i++)
        {
            CreateTask();
        }
    }

    private IEnumerator LoopTasks()
    {
        if (TasksInfo.Count == 0 || CurrentTaskIndex >= TasksInfo.Count) yield break;

        yield return new WaitForSeconds(TasksInfo[CurrentTaskIndex].DurationSeconds);
        NotificationSystem.instance.Notify();
        Tasks[CurrentTaskIndex].OnFinished();

        CurrentTaskIndex++;

        yield return LoopTasks();
    }

    public void CreateTask()
    {
        float scalePreserverance = _Canvas.GetComponent<CanvasScaler>().referenceResolution.x / _Canvas.GetComponent<RectTransform>().rect.width;

        float position = origin.y + Tasks.Count * (-TaskHeight * 1.1f * scalePreserverance);

        Task task = Instantiate(PrefabTask, Vector3.zero, Quaternion.identity, TasksHandler) as Task;
        task.transform.localPosition = new Vector3(origin.x, position, origin.z);
        task.TaskInfo = TasksInfo[Tasks.Count];
        task.OnSpawned();

        Tasks.Add(task);

        AddTaskButton.transform.localPosition = new Vector3(0, origin.y + Tasks.Count * (-TaskHeight * 1.1f * scalePreserverance), 0);
    }

    public void SetTimeSpeed(int speed)
    {
        Time.timeScale = speed;
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