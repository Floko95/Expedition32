#if UNITY_EDITOR
using System;
using R3;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;

namespace BitDuc.EnhancedTimeline.Timeline
{
    /// @cond EXCLUDE
    internal static class SeekBarCursorFix
    {
        public static async void Play(
            GameObject enhancedTimelinePlayer,
            GameObject playableDirector,
            Observable<Unit> timeline
        ) {
            if (
                !IsEnhancedTimelinePlayer(enhancedTimelinePlayer) &&
                !IsChildPlayableDirector(enhancedTimelinePlayer)
            )
                return;

            var timelineWindow = EditorWindow.GetWindow<EditorWindow>("Timeline", false);
            EditorUtility.SetDirty(timelineWindow);

            Selection.activeObject = playableDirector;

            await timeline.WaitAsync();

            if (Selection.activeObject != playableDirector)
                return;

            Selection.activeObject = enhancedTimelinePlayer;
        }

        static bool IsEnhancedTimelinePlayer(GameObject enhancedTimelinePlayer) =>
            Selection.activeObject == enhancedTimelinePlayer;

        static bool IsChildPlayableDirector(GameObject enhancedTimelinePlayer) =>
            Selection.activeObject is GameObject selectedGameObject &&
            selectedGameObject.GetComponent<PlayableDirector>() &&
            selectedGameObject.transform.parent.gameObject == enhancedTimelinePlayer;
    }
}
#endif