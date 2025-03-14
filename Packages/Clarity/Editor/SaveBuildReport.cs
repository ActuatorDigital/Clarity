﻿using System;
using System.Collections.Generic;
using System.IO;
using Actuator;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SettingsManagement;
using UnityEngine;

public class SaveBuildReport : IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    [UserSetting("Build Reports", "Enabled")]
    private static UserSetting<bool> BuildReportSaveEnabled = new UserSetting<bool>(ClarityEditorSettings.Instance, $"saveBuildReport.{nameof(BuildReportSaveEnabled)}", true, SettingsScope.Project);
    [UserSetting("Build Reports", "Location")]
    private static UserSetting<string> BuildReportFolder = new UserSetting<string>(ClarityEditorSettings.Instance, $"saveBuildReport.{nameof(BuildReportFolder)}", "../BuildReports", SettingsScope.Project);

    public void OnPostprocessBuild(BuildReport report)
    {
        if (BuildReportSaveEnabled)
            GenerateAndSave(report);
    }

#if UNITY_6000_0_OR_NEWER
    [MenuItem("Edit/Clarity/Save Last Build Report")]
    public static void SaveReport()
    {
        GenerateAndSave(BuildReport.GetLatestReport());
    }
#endif

    public static void GenerateAndSave(BuildReport report)
    {
        string path = Path.Combine(Application.dataPath, BuildReportFolder);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        var fileExt = ".json";
        var fileName = Path.Combine(path, "BuildReport_" + report.summary.buildEndedAt.ToString("yyyy-MM-dd_HH-mm-ss") + fileExt);
        var contents = ConvertReportToFile(report);

        File.WriteAllText(fileName, contents);
    }

    private static string ConvertReportToFile(BuildReport report)
    {
        var serialisableReport = new SerialisableBuildReport();

        //convert the summary to a table
        serialisableReport.summary = new SerialisableBuildReportSummary(report.summary);

        //convert the steps to a table
        for (int i = 0; i < report.steps.Length; i++)
        {
            var step = report.steps[i];
            serialisableReport.steps.Add(new SerialisableBuildStep(i, step));
            var messages = step.messages;
            for (int j = 0; j < messages.Length; j++)
            {
                var buildStepMessage = new SerialisableBuildStepMessage(i, messages[j]);
                serialisableReport.stepMessages.Add(buildStepMessage);
            }
        }

        //convert the packed assets to a table
        for (int i = 0; i < report.packedAssets.Length; i++)
        {
            var packedAsset = report.packedAssets[i];
            serialisableReport.packedAssets.Add(new SerialisablePackedAssets(i, packedAsset));
            var packedAssetInfo = report.packedAssets[i].contents;
            for (int j = 0; j < packedAssetInfo.Length; j++)
            {
                var content = packedAssetInfo[j];
                serialisableReport.packedAssetInfo.Add(new SerialisablePackedAssetInfo(i, content));
            }
        }

#if UNITY_6000_0_OR_NEWER
        //convert the files to a table
        var files = report.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            var file = files[i];
            serialisableReport.files.Add(new SerialisableBuildFile(file));
        }
#endif

        //convert all to json
        var jsonString = JsonUtility.ToJson(serialisableReport, true);

        return jsonString;
    }

    [Serializable]
    //and then use something like https://products.aspose.app/cells/conversion/json-to-xlsx to convert it to a spreadsheet
    public class SerialisableBuildReport
    {
        public SerialisableBuildReportSummary summary;
        public List<SerialisablePackedAssets> packedAssets = new();
        public List<SerialisablePackedAssetInfo> packedAssetInfo = new();
        public List<SerialisableBuildStep> steps = new();
        public List<SerialisableBuildStepMessage> stepMessages = new();
        public List<SerialisableBuildFile> files = new();
    }

    [Serializable]
    public class SerialisableBuildReportSummary
    {
        public DateTime StartTime;
        public DateTime EndedTime;
        public string BuildType;
        public string GUID;
        public string Options;
        public string OutputPath;
        public string Platform;
        public string PlatformGroup;
        public string Result;
        public int TotalErrors;
        public ulong TotalSize;
        public TimeSpan TotalTime;
        public int TotalWarnings;

        public SerialisableBuildReportSummary(BuildSummary summary)
        {
            StartTime = summary.buildStartedAt;
            EndedTime = summary.buildEndedAt;
#if UNITY_6000_0_OR_NEWER
            BuildType = summary.buildType.ToString();
#endif
            GUID = summary.guid.ToString();
            Options = summary.options.ToString();
            OutputPath = summary.outputPath;
            Platform = summary.platform.ToString();
            PlatformGroup = summary.platformGroup.ToString();
            Result = summary.result.ToString();
            TotalErrors = summary.totalErrors;
            TotalSize = summary.totalSize;
            TotalTime = summary.totalTime;
            TotalWarnings = summary.totalWarnings;
        }
    }

    [Serializable]
    public class SerialisablePackedAssets
    {
        public int index;
        public ulong overhead;
        public string path;

        public SerialisablePackedAssets(int i, PackedAssets packedAssets)
        {
            index = i;
            overhead = packedAssets.overhead;
            path = packedAssets.shortPath;
        }
    }

    [Serializable]
    public class SerialisablePackedAssetInfo
    {
        public int PackedAssetsIndex;
        public long id;
        public ulong offset;
        public ulong size;
        public string Guid;
        public string path;
        public string Type;

        public SerialisablePackedAssetInfo(int i, PackedAssetInfo content)
        {
            PackedAssetsIndex = i;
            id = content.id;
            offset = content.offset;
            size = content.packedSize;
            Guid = content.sourceAssetGUID.ToString();
            path = content.sourceAssetPath;
            Type = content.type.ToString();
        }
    }

    [Serializable]
    public class SerialisableBuildStep
    {
        public int index;
        public string name;
        public int depth;
        public TimeSpan duration;

        public SerialisableBuildStep(int i, BuildStep step)
        {
            index = i;
            name = step.name;
            depth = step.depth;
            duration = step.duration;
        }
    }

    [Serializable]
    public class SerialisableBuildStepMessage
    {
        public int BuildStepIndex;
        public string content;
        public string type;

        public SerialisableBuildStepMessage(int i, BuildStepMessage message)
        {
            BuildStepIndex = i;
            content = message.content;
            type = message.type.ToString();
        }
    }

    [Serializable]
    public class SerialisableBuildFile
    {
        public long id;
        public string Path;
        public ulong Size;
        public string role;

        public SerialisableBuildFile(BuildFile file)
        {
            id = file.id;
            Path = file.path;
            Size = file.size;
            role = file.role;
        }
    }
}