using Actuator;
using Unity.Profiling;
using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEngine;

[InitializeOnLoad]
public class ProfileRecorderAlerts
{
    [UserSetting("Warning Limits, 0 <= means disabled", "Set Pass")]
    private static UserSetting<int> SetPassLimit = new UserSetting<int>(ClarityEditorSettings.Instance, $"warningLimits.{nameof(SetPassLimit)}", 0, SettingsScope.Project);
    [UserSetting("Warning Limits, 0 <= means disabled", "Draw Calls")]
    private static UserSetting<int> DrawCallLimit = new UserSetting<int>(ClarityEditorSettings.Instance, $"warningLimits.{nameof(DrawCallLimit)}", 0, SettingsScope.Project);
    [UserSetting("Warning Limits, 0 <= means disabled", "Total Batches")]
    private static UserSetting<int> TotalBatchesLimit = new UserSetting<int>(ClarityEditorSettings.Instance, $"warningLimits.{nameof(TotalBatchesLimit)}", 0, SettingsScope.Project);
    [UserSetting("Warning Limits, 0 <= means disabled", "Shadow Casters")]
    private static UserSetting<int> ShadowCasterLimit = new UserSetting<int>(ClarityEditorSettings.Instance, $"warningLimits.{nameof(ShadowCasterLimit)}", 0, SettingsScope.Project);

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
        WarnIf(_setPassCallsRecorder, SetPassLimit, "SetPass Calls Count");
        WarnIf(_drawCallsRecorder, DrawCallLimit, "Draw Calls Count");
        WarnIf(_totalBatchesRecorder, TotalBatchesLimit, "Total Batches Count");
        WarnIf(_shadowCastersRecorder, ShadowCasterLimit, "Shadow Casters Count");
    }

    private static void WarnIf(ProfilerRecorder setPassCallsRecorder, UserSetting<int> limit, string warningPrefix)
    {
        if (limit.value <= 0) return;

        if(setPassCallsRecorder.Valid 
            && setPassCallsRecorder.IsRunning 
            && setPassCallsRecorder.LastValue > limit.value)
        {
            Debug.LogWarning($"{warningPrefix} limit exceeded: {setPassCallsRecorder.LastValue} > {limit.value}");
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
}