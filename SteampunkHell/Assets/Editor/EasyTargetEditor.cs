using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EasyTarget))] [CanEditMultipleObjects]
public class EasyTargetEditor : Editor
{
    private EasyTarget _target;

    private float movement;
    private bool activated;

    private void OnEnable()
    {
        _target = (EasyTarget)target;
    }

    private void OnSceneGUI()
    {

        Handles.BeginGUI();
        /*var v = EditorWindow.GetWindow<SceneView>().camera.pixelRect;

        if (GUI.Button(new Rect(v.width - v.width / 2 - 50, 20, 100, 50), "Hola"))
        { }

        if (GUI.Button(new Rect(v.width - 120, v.height - 70, 100, 50), "QueTal"))
        { }*/

        UpperLeftControlPanel();
        /*
        var p = Camera.current.WorldToScreenPoint(_target.transform.position);
        var r = new Rect(p.x - 50, Screen.height - p.y - 50, 100, 50);
        GUI.Button(r, "Encima");*/

        Handles.EndGUI();
    }
    private void UpperLeftControlPanel()
    {
        GUILayout.BeginArea(new Rect(20, 20, 300, 175));
        var rec = EditorGUILayout.BeginVertical();
        Colorize(_target.currentToolbarSelection, true);
        GUI.Box(rec, GUIContent.none);
        Texture[] icons = new Texture[3];
        icons[0] = (Texture)Resources.Load("search");
        icons[1] = (Texture)Resources.Load("eye");
        icons[2] = (Texture)Resources.Load("Target");
        GUILayout.BeginHorizontal();
        Colorize(ToolBarSelection.SetPositions, false);
        if (GUILayout.Button(new GUIContent(icons[0]), GUILayout.Width(95), GUILayout.Height(95)))
        {
            _target.currentToolbarSelection = ToolBarSelection.SetPositions;
        }
        Colorize(ToolBarSelection.FollowCamera, false);
        if (GUILayout.Button(new GUIContent(icons[1]), GUILayout.Width(95), GUILayout.Height(95)))
        {
            _target.currentToolbarSelection = ToolBarSelection.FollowCamera;
        }
        Colorize(ToolBarSelection.CameraTarget, false);
        if (GUILayout.Button(new GUIContent(icons[2]), GUILayout.Width(95), GUILayout.Height(95)))
        {
            _target.currentToolbarSelection = ToolBarSelection.CameraTarget;
        }
        GUILayout.EndHorizontal();

        GUI.color = Color.white;

        switch (_target.currentToolbarSelection)
        {
            case ToolBarSelection.SetPositions:
                _target.goTarget = (GameObject)EditorGUILayout.ObjectField(_target.goTarget, typeof(GameObject));
                if(_target.goTarget != null)
                {
                    _target.transform.forward = _target.goTarget.transform.position - _target.transform.position;
                }
                EditorGUILayout.EndVertical();
                break;
            case ToolBarSelection.FollowCamera:
                _target.transform.position = Camera.current.transform.position;
                _target.transform.localRotation = Camera.current.transform.localRotation;
                break;
            case ToolBarSelection.CameraTarget:
                _target.transform.forward = Camera.current.transform.position - _target.transform.position;
                break;
        }


        GUILayout.EndArea();
    }

    private void Colorize(ToolBarSelection mode, bool background)
    {
        if (_target.currentToolbarSelection != mode)
        {
            GUI.color = new Color(0.3f, 0.3f, 0.3f, 1);
        }
        else
        {
            switch (mode)
            {
                case ToolBarSelection.SetPositions:
                    GUI.color = new Color(background ? 0.6f : 1, 0, 0, background ? 0.6f : 1);
                    break;
                case ToolBarSelection.FollowCamera:
                    GUI.color = new Color(0, background ? 0.6f : 1, 0, background ? 0.6f : 1);
                    break;
                case ToolBarSelection.CameraTarget:
                    GUI.color = new Color(0, 0, background ? 0.6f : 1, background ? 0.6f : 1);
                    break;
            }
        }
    }
}

