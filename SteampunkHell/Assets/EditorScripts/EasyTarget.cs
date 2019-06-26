using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyTarget : MonoBehaviour
{
    public GameObject goTarget;
    public ToolBarSelection currentToolbarSelection;
}

public enum ToolBarSelection
{
    SetPositions,
    FollowCamera,
    CameraTarget
}