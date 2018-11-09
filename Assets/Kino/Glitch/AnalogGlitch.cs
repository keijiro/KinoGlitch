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
using UnityEngine.Rendering.PostProcessing;

namespace Kino
{
  [System.Serializable]
  [UnityEngine.Rendering.PostProcessing.PostProcess(typeof(AnalogGlitchRenderer), PostProcessEvent.AfterStack, "Kino Image Effects/Analog Glitch")]
  public class AnalogGlitch : PostProcessEffectSettings
  {
    [Range(0, 1)]
    public FloatParameter scanLineJitter = new FloatParameter();

    [Range(0, 1)]
    public FloatParameter verticalJump = new FloatParameter();

    [Range(0, 1)]
    public FloatParameter horizontalShake = new FloatParameter();

    [Range(0, 1)]
    public FloatParameter colorDrift = new FloatParameter();
  }

  public class AnalogGlitchRenderer : PostProcessEffectRenderer<AnalogGlitch>
  {
    private float _verticalJumpTime;

    public override void Render(PostProcessRenderContext context)
    {
      var sheet = context.propertySheets.Get(Shader.Find("Hidden/Kino/Glitch/Analog"));

      _verticalJumpTime += Time.deltaTime * settings.verticalJump * 11.3f;

      var sl_thresh = Mathf.Clamp01(1.0f - settings.scanLineJitter * 1.2f);
      var sl_disp = 0.002f + Mathf.Pow(settings.scanLineJitter, 3) * 0.05f;
      sheet.properties.SetVector("_ScanLineJitter", new Vector2(sl_disp, sl_thresh));

      var vj = new Vector2(settings.verticalJump, _verticalJumpTime);
      sheet.properties.SetVector("_VerticalJump", vj);

      sheet.properties.SetFloat("_HorizontalShake", settings.horizontalShake * 0.2f);

      var cd = new Vector2(settings.colorDrift * 0.04f, Time.time * 606.11f);
      sheet.properties.SetVector("_ColorDrift", cd);

      context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }

  }
}
