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

        // Scan line jitter

        [SerializeField, Range(0, 1)]
        float _scanLineJitter = 0;

        public float scanLineJitter {
            get { return _scanLineJitter; }
            set { _scanLineJitter = value; }
        }

        // Vertical jump

        [SerializeField, Range(0, 1)]
        float _verticalJump = 0;

        public float verticalJump {
            get { return _verticalJump; }
            set { _verticalJump = value; }
        }

        // Horizontal shake

        [SerializeField, Range(0, 1)]
        float _horizontalShake = 0;

        public float horizontalShake {
            get { return _horizontalShake; }
            set { _horizontalShake = value; }
        }

        // Color drift

        [SerializeField, Range(0, 1)]
        float _colorDrift = 0;

        public float colorDrift {
            get { return _colorDrift; }
            set { _colorDrift = value; }
        }

        #endregion

        #region Private Properties

        [SerializeField] Shader _shader;

        Material _material;

        float _verticalJumpTime;

        #endregion

        #region MonoBehaviour Functions

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            _verticalJumpTime += Time.deltaTime * _verticalJump * 11.3f;

            var sl_thresh = Mathf.Clamp01(1.0f - _scanLineJitter * 1.2f);
            var sl_disp = 0.002f + Mathf.Pow(_scanLineJitter, 3) * 0.05f;
            _material.SetVector("_ScanLineJitter", new Vector2(sl_disp, sl_thresh));

            var vj = new Vector2(_verticalJump, _verticalJumpTime);
            _material.SetVector("_VerticalJump", vj);

            _material.SetFloat("_HorizontalShake", _horizontalShake * 0.2f);

            var cd = new Vector2(_colorDrift * 0.04f, Time.time * 606.11f);
            _material.SetVector("_ColorDrift", cd);

            Graphics.Blit(source, destination, _material);
        }

        #endregion
    }
}
