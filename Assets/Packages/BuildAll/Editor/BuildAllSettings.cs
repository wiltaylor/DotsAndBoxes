using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildAllSettings 
{

    public static string GetSetting(string name)
    {
        var settings = ReadHashTable(GetSettingFileName());
        return settings[name];
    }

    public static void SetSetting(string name, string value)
    {
        var settings = ReadHashTable(GetSettingFileName());
        settings[name] = value;

        WriteHashTable(settings, GetSettingFileName());
    }

    private static string GetSettingFileName()
    {
        return (from a in AssetDatabase.GetAllAssetPaths()
                where a.EndsWith("BuildAll.config")
                select a).First();
    }

    private static void WriteHashTable(Dictionary<string, string> settings, string filename)
    {
        try
        {
            if (File.Exists(filename))
                File.Delete(filename);

            var file = File.CreateText(filename);

            foreach (var k in settings.Keys)
            {
                file.WriteLine("[" + k + "]");
                file.WriteLine(settings[k]);
            }

            file.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Unable to write settings file for BuildAll. This may results in a corrupt file. Error Details: " + e.ToString() );
            
        }

    }

    private static Dictionary<string, string> ReadHashTable(string filename)
    {
        
        var returnData = new Dictionary<string, string>();
        var currentSetting = "";
        var currentValue = "";

        try
        {

        var file = File.OpenText(filename);

        while (!file.EndOfStream)
        {
            var currentLine = file.ReadLine();

            if (currentLine == null)
                break;

            if (currentLine.StartsWith("[") && currentLine.EndsWith("]"))
            {
                if (currentSetting != "")
                {
                    currentValue = currentValue.TrimEnd(Environment.NewLine.ToCharArray());
                    returnData.Add(currentSetting, currentValue);
                }

                currentSetting = currentLine.Substring(1, currentLine.Length - 2);
                currentValue = "";
            }
            else
            {
                currentValue += currentLine + Environment.NewLine;
            }
        }

        file.Close();

        if (currentSetting == "") return returnData;

        currentValue = currentValue.TrimEnd(Environment.NewLine.ToCharArray());
        returnData.Add(currentSetting, currentValue);

        return returnData;

        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return returnData;
        }
    }
}
