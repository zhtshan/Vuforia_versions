/*===============================================================================
Copyright (c) 2017-2019 PTC Inc. All Rights Reserved.

Confidential and Proprietary - Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/

using System.Linq;
using UnityEditor;
using UnityEngine;
#if UNITY_2020_1_OR_NEWER
using UnityEngine.XR;
#endif
using Vuforia;
using Vuforia.EditorClasses;

/// <summary>
/// Creates connection between open source files and the Vuforia library.
/// Do not modify.
/// </summary>
[InitializeOnLoad]
public static class OpenSourceInitializer
{
    static IUnityEditorFacade sFacade;

    static OpenSourceInitializer()
    {
        InitializeFacade();
        GameObjectFactory.SetDefaultBehaviourTypeConfiguration(new DefaultBehaviourAttacher());
        ReplacePlaceHolders();
    }

    static void ReplacePlaceHolders()
    {
        var trackablePlaceholders = Object.FindObjectsOfType<DefaultTrackableBehaviourPlaceholder>().ToList();
        var initErrorsPlaceholders = Object.FindObjectsOfType<DefaultInitializationErrorHandlerPlaceHolder>().ToList();
        
        trackablePlaceholders.ForEach(ReplaceTrackablePlaceHolder);
        initErrorsPlaceholders.ForEach(ReplaceInitErrorPlaceHolder);
    }
    
    static void ReplaceTrackablePlaceHolder(DefaultTrackableBehaviourPlaceholder placeHolder)
    {
        var go = placeHolder.gameObject;
        var dteh = go.AddComponent<DefaultTrackableEventHandler>();
        SetDefaultTrackableHandlerSettings(dteh);

        Object.DestroyImmediate(placeHolder);
    }

    static void ReplaceInitErrorPlaceHolder(DefaultInitializationErrorHandlerPlaceHolder placeHolder)
    {
        var go = placeHolder.gameObject;
        go.AddComponent<DefaultInitializationErrorHandler>();

        Object.DestroyImmediate(placeHolder);
    }

    class DefaultBehaviourAttacher : IDefaultBehaviourAttacher
    {
        public void AddDefaultTrackableBehaviour(GameObject go)
        {
            var dteh = go.AddComponent<DefaultTrackableEventHandler>();
            SetDefaultTrackableHandlerSettings(dteh);
        }

        public void AddDefaultInitializationErrorHandler(GameObject go)
        {
            go.AddComponent<DefaultInitializationErrorHandler>();
        }
    }

    static void SetDefaultTrackableHandlerSettings(DefaultTrackableEventHandler dteh)
    {
        if (dteh.gameObject.GetComponent<AnchorBehaviour>() != null)
        {
            // render anchors in LIMITED mode
            dteh.StatusFilter = DefaultTrackableEventHandler.TrackingStatusFilter.Tracked_ExtendedTracked_Limited;
        }
        else
        {
            // the default for all other targets is not to consider LIMITED poses
            dteh.StatusFilter = DefaultTrackableEventHandler.TrackingStatusFilter.Tracked_ExtendedTracked;
        }
    }

    static void InitializeFacade()
    {
        if (sFacade != null) return;

        sFacade = new OpenSourceUnityEditorFacade();
        UnityEditorFacade.Instance = sFacade;
    }

    class OpenSourceUnityEditorFacade : IUnityEditorFacade
    {
        public bool IsTargetingHoloLens()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WSAPlayer)
                return false;
#if UNITY_2020_1_OR_NEWER
            const string UNITY_WINDOWSMR_IDENTIFIER = "Windows Mixed Reality";
            return XRSettings.supportedDevices.Any(xrDevice => xrDevice.Contains(UNITY_WINDOWSMR_IDENTIFIER));
#else
            const string UNITY_WINDOWSMR_IDENTIFIER = "WindowsMR";
            if (!PlayerSettings.GetVirtualRealitySupported(BuildTargetGroup.WSA)) return false;
            foreach (var vrSdkName in PlayerSettings.GetVirtualRealitySDKs(BuildTargetGroup.WSA))
            {
                if (vrSdkName.Equals(UNITY_WINDOWSMR_IDENTIFIER))
                {
                    return true;
                }
            }
            return false;
#endif
        }
    }

}
