using Actuator;
using Unity.Profiling;
using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEngine;

[InitializeOnLoad]
public class ProfileRecorderAlerts
{
    [System.Serializable]
    public class ThresholdValues
    {
        public int WarningThreshold;
        public int ErrorThreshold;
        public bool HardError;
    }

    [UserSetting()]
    private static UserSetting<ThresholdValues> SetPassLimit = new UserSetting<ThresholdValues>(ClarityEditorSettings.Instance, $"warningLimits.{nameof(SetPassLimit)}", new ThresholdValues(), SettingsScope.Project);
    [UserSetting()]
    private static UserSetting<ThresholdValues> DrawCallLimit = new UserSetting<ThresholdValues>(ClarityEditorSettings.Instance, $"warningLimits.{nameof(DrawCallLimit)}", new ThresholdValues(), SettingsScope.Project);
    [UserSetting()]
    private static UserSetting<ThresholdValues> TotalBatchesLimit = new UserSetting<ThresholdValues>(ClarityEditorSettings.Instance, $"warningLimits.{nameof(TotalBatchesLimit)}", new ThresholdValues(), SettingsScope.Project);
    [UserSetting()]
    private static UserSetting<ThresholdValues> ShadowCasterLimit = new UserSetting<ThresholdValues>(ClarityEditorSettings.Instance, $"warningLimits.{nameof(ShadowCasterLimit)}", new ThresholdValues(), SettingsScope.Project);

    private static ProfilerRecorder _setPassCallsRecorder;
    private static ProfilerRecorder _drawCallsRecorder;
    private static ProfilerRecorder _totalBatchesRecorder;
    private static ProfilerRecorder _shadowCastersRecorder;

    static ProfileRecorderAlerts()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            EnsureReleased();
            return;
        }

        EnsureAcquired();
        if(EditorApplication.isPaused) return;
        ProcessThresholdLimits(_setPassCallsRecorder, SetPassLimit, "SetPass Calls Count");
        ProcessThresholdLimits(_drawCallsRecorder, DrawCallLimit, "Draw Calls Count");
        ProcessThresholdLimits(_totalBatchesRecorder, TotalBatchesLimit, "Total Batches Count");
        ProcessThresholdLimits(_shadowCastersRecorder, ShadowCasterLimit, "Shadow Casters Count");
    }

    private static void ProcessThresholdLimits(ProfilerRecorder setPassCallsRecorder, UserSetting<ThresholdValues> limit, string warningPrefix)
    {
        if (limit.value.WarningThreshold <= 0 
            && limit.value.ErrorThreshold <= 0) return;

        if (setPassCallsRecorder.Valid
            && setPassCallsRecorder.IsRunning)
        {
            if (setPassCallsRecorder.LastValue > limit.value.ErrorThreshold)
            {
                Debug.LogError($"{warningPrefix} limit exceeded: {setPassCallsRecorder.LastValue} > {limit.value}");
                if (limit.value.HardError)
                {
                    Debug.Break();
                    EditorUtility.DisplayDialog(
                        "Clarity Hard Error Threshold", 
                        $"{warningPrefix} has reached {setPassCallsRecorder.LastValue}, it's error threshold is {limit.value.ErrorThreshold}." +
                        $"\n\nHard errors can be toggled off in Clarity Project Settings.", 
                        "Understood");
                }
            }
            else if (setPassCallsRecorder.LastValue > limit.value.WarningThreshold)
            {
                Debug.LogWarning($"{warningPrefix} limit exceeded: {setPassCallsRecorder.LastValue} > {limit.value}");
            }
        }
    }

    private static void EnsureAcquired()
    {
        if(!_setPassCallsRecorder.Valid) _setPassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
        if(!_drawCallsRecorder.Valid) _drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
        if(!_totalBatchesRecorder.Valid) _totalBatchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Total Batches Count");
        if(!_shadowCastersRecorder.Valid) _shadowCastersRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Shadow Casters Count");
    }

    private static void EnsureReleased()
    {
        if (_setPassCallsRecorder.Valid) _setPassCallsRecorder.Dispose();
        if(_totalBatchesRecorder.Valid) _drawCallsRecorder.Dispose();
        if(_totalBatchesRecorder.Valid) _totalBatchesRecorder.Dispose();
        if(_shadowCastersRecorder.Valid) _shadowCastersRecorder.Dispose();
    }


    [UserSettingBlock("ProfileRecorderAlerts Settings")]
    static void SettingsUI(string searchContext)
    {
        EditorGUI.BeginChangeCheck();

        DoThresholdUI(SetPassLimit, "Set Pass", searchContext);
        DoThresholdUI(DrawCallLimit, "Draw Calls", searchContext);
        DoThresholdUI(TotalBatchesLimit, "Total Batches", searchContext);
        DoThresholdUI(ShadowCasterLimit, "Shadow Casters", searchContext);

        if (EditorGUI.EndChangeCheck())
            ClarityEditorSettings.Instance.Save();
    }

    private static void DoThresholdUI(UserSetting<ThresholdValues> thresholdValues, string name, string searchContext)
    {
        using (new SettingsGUILayout.IndentedGroup(name))
        {
            EditorGUI.BeginChangeCheck();
            thresholdValues.value.WarningThreshold = SettingsGUILayout.SearchableIntField("Warning Threshold", thresholdValues.value.WarningThreshold, searchContext);
            thresholdValues.value.ErrorThreshold = SettingsGUILayout.SearchableIntField("Error Threshold", thresholdValues.value.ErrorThreshold, searchContext);
            thresholdValues.value.HardError = SettingsGUILayout.SearchableToggle("Hard Error", thresholdValues.value.HardError, searchContext);
            if (EditorGUI.EndChangeCheck())
                thresholdValues.ApplyModifiedProperties();
        }

        SettingsGUILayout.DoResetContextMenuForLastRect(thresholdValues);
    }
}