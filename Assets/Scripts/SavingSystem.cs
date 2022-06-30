using UnityEngine;
using System.Collections.Generic;
using System.IO;
using TMPro;
public class SavingSystem : MonoBehaviour
{
    private string path = "sd_cache";

    private string path1 = "sd_cache/Schedule_saving_file_1.txt";
    private string path2 = "sd_cache/Schedule_saving_file_2.txt";
    private string path3 = "sd_cache/Schedule_saving_file_3.txt";
    private char separator = ':';

    private string[] file;
    private int line = 0 - 1; // -1 for ++line been working

    public TextMeshProUGUI[] CasesTitleTexts;

    private void Start()
    {
        bool exists = System.IO.Directory.Exists(path);

        if (!exists)
            System.IO.Directory.CreateDirectory(path);
    }

    private string GetPath(int index)
    {
        if (index == 0) return path1;
        if (index == 1) return path2;
        else return path3;
    }
    

    /// <summary>
    /// If starts with -- then informational line with no sense
    /// </summary>
    /// <param name="title"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private string Encode(string title, string value = "")
    {
        return title.FormatString() + (value != "" ? separator + value : "").FormatString();
    }

    public void SaveCase(int index)
    {
        List<Task> tasks = TaskInvokingSystem.instance.Tasks;

        using (StreamWriter writer = new StreamWriter(GetPath(index)))
        {
            //writer.WriteLine(Encode("Case", CaseTitleInputFields[index].text));
            writer.WriteLine(Encode("Title", TaskInvokingSystem.instance.Title));
            CasesTitleTexts[index].text = TaskInvokingSystem.instance.Title;
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
    private void Flush()
    {
        line = -1;
    }
    public void LoadCase(int index)
    {
        Flush();

        TaskInvokingSystem.instance.Flush();

        using (StreamReader reader = new StreamReader(GetPath(index)))
        {
            file = reader.ReadToEnd().Split('\n');

            //CaseTitleInputFields[index].text = ReadNext();

            string doc_title = ReadNext();
            TaskInvokingSystem.instance.SetTitle(doc_title);
            CasesTitleTexts[index].text = doc_title;

            int tasksCount = int.Parse(ReadNext());

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
public static class Utils
{
    public static string FormatString(this string s) => s.Replace("\n", "").Replace("\r", "");
}