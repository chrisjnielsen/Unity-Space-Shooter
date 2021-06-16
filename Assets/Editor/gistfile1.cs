using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class gistfile1 : EditorWindow
{
    [SerializeField] private TextAsset xmlAsset;

    public SpriteAlignment spriteAlignment = SpriteAlignment.Center;

    public Vector2 customOffset = new Vector2(0.5f, 0.5f);

    public void OnGUI()
    {
        xmlAsset = EditorGUILayout.ObjectField("XML Source", xmlAsset, typeof(TextAsset), false) as TextAsset;

        spriteAlignment = (SpriteAlignment)EditorGUILayout.EnumPopup("Pivot", spriteAlignment);

        bool enabled = GUI.enabled;
        if(spriteAlignment != SpriteAlignment.Custom)
        {
            GUI.enabled = false;
        }

        EditorGUILayout.Vector2Field("Custom Offset", customOffset);

        GUI.enabled = enabled;

        if (xmlAsset == null)
        {
            GUI.enabled = false;
        }

        if (GUILayout.Button("Slice"))
        {
            //PerformSlice();
        }

        GUI.enabled = enabled;




    }


}
