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

using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Kino
{

  [System.Serializable]
  [UnityEngine.Rendering.PostProcessing.PostProcess(typeof(DigitalGlitchRenderer), PostProcessEvent.AfterStack, "Kino Image Effects/DigitalGlitch")]
  public class DigitalGlitch : PostProcessEffectSettings
  {
    [SerializeField, Range(0, 1f)]
    public FloatParameter intensity = new FloatParameter() { value = 0 };
  }


  public class DigitalGlitchRenderer : PostProcessEffectRenderer<DigitalGlitch>
  {
    private Texture2D _noiseTexture;
    private RenderTexture _trashFrame1;
    private RenderTexture _trashFrame2;


    #region Private Functions

    public DigitalGlitchRenderer() : base()
    {
      if (_noiseTexture != null) return;

      _noiseTexture = new Texture2D(64, 32, TextureFormat.ARGB32, false);
      _noiseTexture.hideFlags = HideFlags.DontSave;
      _noiseTexture.wrapMode = TextureWrapMode.Clamp;
      _noiseTexture.filterMode = FilterMode.Point;

      _trashFrame1 = new RenderTexture(Screen.width, Screen.height, 0);
      _trashFrame2 = new RenderTexture(Screen.width, Screen.height, 0);
      _trashFrame1.hideFlags = HideFlags.DontSave;
      _trashFrame2.hideFlags = HideFlags.DontSave;

      UpdateNoiseTexture();
    }

    static Color RandomColor()
    {
      return new Color(Random.value, Random.value, Random.value, Random.value);
    }

    void UpdateNoiseTexture()
    {
      var color = RandomColor();

      for (var y = 0; y < _noiseTexture.height; y++)
      {
        for (var x = 0; x < _noiseTexture.width; x++)
        {
          if (Random.value > 0.89f)
          {
            color = RandomColor();
          }
          _noiseTexture.SetPixel(x, y, color);
        }
      }

      _noiseTexture.Apply();
    }

    #endregion

    public override void Render(PostProcessRenderContext context)
    {
      var sheet = context.propertySheets.Get(Shader.Find("Hidden/Kino/Glitch/Digital"));
      if (Random.value > Mathf.Lerp(0.9f, 0.5f, settings.intensity.value))
      {
        UpdateNoiseTexture();
      }

      // Update trash frames on a constant interval.
      var fcount = Time.frameCount;
      if (fcount % 13 == 0) context.command.Blit(context.source, _trashFrame1);

      if (fcount % 73 == 0) context.command.Blit(context.source, _trashFrame2);


      sheet.properties.SetFloat("_Intensity", settings.intensity);
      sheet.properties.SetTexture("_NoiseTex", _noiseTexture);
      var trashFrame = Random.value > 0.5f ? _trashFrame1 : _trashFrame2;
      sheet.properties.SetTexture("_TrashTex", trashFrame);

      context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }

  }
}
