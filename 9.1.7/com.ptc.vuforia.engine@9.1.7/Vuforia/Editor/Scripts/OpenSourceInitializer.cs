/*===============================================================================
Copyright (c) 2017-2019 PTC Inc. All Rights Reserved.

Confidential and Proprietary - Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/

using System.Linq;
using UnityEditor;
using UnityEngine;
using Vuforia;
using Vuforia.EditorClasses;

/// <summary>
/// Creates connection between open source files and the Vuforia library.
/// Do not modify.
/// </summary>
[InitializeOnLoad]
public static class OpenSourceInitializer
{
    static OpenSourceInitializer()
    {
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
}
