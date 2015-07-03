//
// KinoGlitch - Video glitch effect
//
// Copyright (C) 2015 Keijiro Takahashi
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

namespace Kino
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AnalogGlitch))]
    public class AnalogGlitchEditor : Editor
    {
        SerializedProperty _scanLineJitter;
        SerializedProperty _verticalJump;
        SerializedProperty _horizontalShake;
        SerializedProperty _colorDrift;

        void OnEnable()
        {
            _scanLineJitter = serializedObject.FindProperty("_scanLineJitter");
            _verticalJump = serializedObject.FindProperty("_verticalJump");
            _horizontalShake = serializedObject.FindProperty("_horizontalShake");
            _colorDrift = serializedObject.FindProperty("_colorDrift");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_scanLineJitter);
            EditorGUILayout.PropertyField(_verticalJump);
            EditorGUILayout.PropertyField(_horizontalShake);
            EditorGUILayout.PropertyField(_colorDrift);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
