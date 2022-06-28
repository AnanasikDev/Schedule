using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SavingSystem : MonoBehaviour
{
    private string path = "Schedule_saving_file.txt";
    private char separator = ':';

    private string[] file;
    private int line = 0 - 1; // -1 for ++line been working

    /// <summary>
    /// If starts with -- then informational line with no sense
    /// </summary>
    /// <param name="title"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private string Encode(string title, string value = "")
    {
        return title + (value != "" ? separator + value : "");
    }

    public void SaveCase()
    {
        List<Task> tasks = TaskInvokingSystem.instance.Tasks;

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine(Encode("Tasks count", tasks.Count.ToString()));
            for (int i = 0; i < tasks.Count; i++)
            {
                writer.WriteLine(Encode($"--Task {i}"));

                writer.WriteLine(Encode("Task title", tasks[i].TaskInfo.Title));
                writer.WriteLine(Encode("Task duration seconds", tasks[i].TaskInfo.DurationSeconds.ToString()));
            }
        }
    }
    private string ReadNext()
    {
        return file[++line].Split(separator)[1];
    }
    private void SkipNext()
    {
        line++;
    }
    public void LoadCase()
    {
        TaskInvokingSystem.instance.Tasks.Clear();

        using (StreamReader reader = new StreamReader(path))
        {
            file = reader.ReadToEnd().Split('\n');

            /*EasyDebug.LogCollectionSep("; ", file);
            EasyDebug.Log(file.Length);*/

            string s = ReadNext();
            EasyDebug.LogCollection(s);
            int tasksCount = int.Parse(s);

            Task[] tasks = new Task[tasksCount];

            for (int i = 0; i < tasks.Length; i++)
            {
                SkipNext();
                string title = ReadNext();
                int duration = int.Parse(ReadNext());

                EasyDebug.Log(title, duration);

                TaskInvokingSystem.instance.CreateTask(title, (uint)duration);
            }
        }
    }
}
