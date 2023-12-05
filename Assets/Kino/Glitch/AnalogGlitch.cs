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

namespace Kino
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Kino Image Effects/Analog Glitch")]
    public class AnalogGlitch : MonoBehaviour
    {
        #region Public Properties

        [Header("Glitch Settings")]
        [SerializeField, Range(0, 1)] private float scanLineJitter = 0;
        [SerializeField, Range(0, 1)] private float verticalJump = 0;
        [SerializeField, Range(0, 1)] private float horizontalShake = 0;
        [SerializeField, Range(0, 1)] private float colorDrift = 0;

        #endregion

        #region Private Properties

        [SerializeField] private Shader glitchShader;
        private Material glitchMaterial;

        private float verticalJumpTime;

        #endregion

        #region MonoBehaviour Functions

        void OnEnable()
        {
            FindOrCreateMaterial();
        }

        void OnDisable()
        {
            if (glitchMaterial != null)
            {
                DestroyImmediate(glitchMaterial);
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            UpdateMaterialProperties();
            Graphics.Blit(source, destination, glitchMaterial);
        }

        #endregion

        #region Private Methods

        private void FindOrCreateMaterial()
        {
            if (!glitchMaterial)
            {
                glitchMaterial = new Material(glitchShader);
                glitchMaterial.hideFlags = HideFlags.DontSave;
            }
        }

        private void UpdateMaterialProperties()
        {
            FindOrCreateMaterial();

            verticalJumpTime += Time.deltaTime * verticalJump * 11.3f;

            float scanLineThreshold = Mathf.Clamp01(1.0f - scanLineJitter * 1.2f);
            float scanLineDisplacement = 0.002f + Mathf.Pow(scanLineJitter, 3) * 0.05f;
            glitchMaterial.SetVector("_ScanLineJitter", new Vector2(scanLineDisplacement, scanLineThreshold));

            Vector2 verticalJumpVector = new Vector2(verticalJump, verticalJumpTime);
            glitchMaterial.SetVector("_VerticalJump", verticalJumpVector);

            glitchMaterial.SetFloat("_HorizontalShake", horizontalShake * 0.2f);

            Vector2 colorDriftVector = new Vector2(colorDrift * 0.04f, Time.time * 606.11f);
            glitchMaterial.SetVector("_ColorDrift", colorDriftVector);
        }

        #endregion
    }
}
