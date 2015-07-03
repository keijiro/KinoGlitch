//
// Reaktion - An audio reactive animation toolkit for Unity.
//
// Copyright (C) 2013, 2014 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Reaktion {

[CustomEditor(typeof(JitterMotion)), CanEditMultipleObjects]
public class JitterMotionEditor : Editor
{
    SerializedProperty propPositionFrequency;
    SerializedProperty propRotationFrequency;

    SerializedProperty propPositionAmount;
    SerializedProperty propRotationAmount;

    SerializedProperty propPositionComponents;
    SerializedProperty propRotationComponents;

    SerializedProperty propPositionOctave;
    SerializedProperty propRotationOctave;

    GUIContent labelFrequency;
    GUIContent labelAmount;
    GUIContent labelOctave;

    void OnEnable()
    {
        propPositionFrequency = serializedObject.FindProperty("positionFrequency");
        propRotationFrequency = serializedObject.FindProperty("rotationFrequency");

        propPositionAmount = serializedObject.FindProperty("positionAmount");
        propRotationAmount = serializedObject.FindProperty("rotationAmount");

        propPositionComponents = serializedObject.FindProperty("positionComponents");
        propRotationComponents = serializedObject.FindProperty("rotationComponents");

        propPositionOctave = serializedObject.FindProperty("positionOctave");
        propRotationOctave = serializedObject.FindProperty("rotationOctave");

        labelFrequency = new GUIContent("Frequency");
        labelAmount    = new GUIContent("Noise Strength");
        labelOctave    = new GUIContent("Fractal Level");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Position");
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(propPositionAmount, labelAmount);
        EditorGUILayout.PropertyField(propPositionComponents, GUIContent.none);
        EditorGUILayout.PropertyField(propPositionFrequency, labelFrequency);
        EditorGUILayout.IntSlider(propPositionOctave, 1, 8, labelOctave);
        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("Rotation");
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(propRotationAmount, labelAmount);
        EditorGUILayout.PropertyField(propRotationComponents, GUIContent.none);
        EditorGUILayout.PropertyField(propRotationFrequency, labelFrequency);
        EditorGUILayout.IntSlider(propRotationOctave, 1, 8, labelOctave);
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}

} // namespace Reaktion
