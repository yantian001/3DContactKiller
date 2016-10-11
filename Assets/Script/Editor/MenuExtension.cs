using UnityEngine;
using System.Collections;
using UnityEditor;

public class MenuExtension : EditorWindow
{
    [MenuItem("FUG/Mission/Mission Editor")]
	private static void OpenMissionEditor()
    {
        MissionWindowEditor.Init();
    }
}
