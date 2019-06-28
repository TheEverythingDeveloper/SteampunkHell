using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HoudiniEngineUnity;

[CustomEditor(typeof(HoudiniExtensorScript))] [CanEditMultipleObjects]
public class HoudiniExtensor : Editor
{
    public HoudiniExtensorScript scriptTarget;
    public HEU_HoudiniAssetRoot[] allHoudiniAssets;

    private void OnEnable()
    {
        StylePresets.CreateStyles();
        scriptTarget = (HoudiniExtensorScript)target;
    }

    private void OnSceneGUI()
    {
        if (!scriptTarget.turnOn) return;


        allHoudiniAssets = (HEU_HoudiniAssetRoot[])FindObjectsOfType<HEU_HoudiniAssetRoot>() as HEU_HoudiniAssetRoot[];

        Handles.BeginGUI();
        var addValue = 40 / Vector3.Distance(Camera.current.transform.position, scriptTarget.transform.position);

        foreach (var item in allHoudiniAssets)
        {
            DrawButton(item.transform.position + addValue * Vector3.up, (Texture)Resources.Load("HoudiniIcon"));
        }


        Handles.EndGUI();
    }

    private void DrawButton(Vector3 position, Texture loadTexture)
    {
        var p = Camera.current.WorldToScreenPoint(position);
        var size = 1500 / Vector3.Distance(Camera.current.transform.position, position);
        var r = new Rect(p.x - size / 2, Screen.height - p.y - size, size, size * 1f);
        //var r = new Rect(p.x, p.y, size, size * 4);

        GUI.DrawTexture(r, loadTexture);
    }
}
