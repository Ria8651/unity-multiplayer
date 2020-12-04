﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        MapGenerator mapGenerator = (MapGenerator)target;

        if (GUILayout.Button("GenerateMap")) {
            mapGenerator.GenerateMap();
        }

        base.OnInspectorGUI();
    }
}