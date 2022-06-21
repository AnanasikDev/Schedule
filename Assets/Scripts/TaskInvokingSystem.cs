using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

public class TaskInvokingSystem : MonoBehaviour
{
    [SerializeField] private Task PrefabTask;
    [SerializeField] private Transform TasksHandler;

    [Header("Read-Only")]
    [SerializeField] private List<Task> Tasks;
    [SerializeField] private int CurrentTaskIndex = 0;

    private Vector3 origin = new Vector3(0, 0, 0);
    private const float TaskHeight = 80;

    private Action OnTaskAdded;

    private void Start()
    {
        Tasks = new List<Task>()
        {
            new Task(10, "��������", ""),
            new Task(15, "������ ��� �������� 1", ""),
            new Task(10, "��������", ""),
            new Task(12, "������ ��� �������� 2", "")
        };

        StartCoroutine(LoopTasks());
    }

    private IEnumerator LoopTasks()
    {
        if (Tasks.Count == 0) yield break;

        yield return new WaitForSecondsRealtime(Tasks[CurrentTaskIndex].DurationSeconds);
        CurrentTaskIndex++;

        NotificationSystem.instance.Notify();

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
