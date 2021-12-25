using UnityEngine;
using UnityEngine.UI;

public class ViewportManager : MonoBehaviour
{
  private RawImage viewport;
  private AspectRatioFitter fitter;

  public bool mirrored
  {
    set
    {
      var scaleY = value ? -1f : 1f;
      viewport.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
    }
  }

  public int orient
  {
    set
    {
      viewport.rectTransform.localEulerAngles = new Vector3(0, 0, value);
    }
  }

  void Start()
  {
    viewport = GetComponentInChildren<RawImage>();
    fitter = GetComponentInChildren<AspectRatioFitter>();
  }

  public void SetTexture(Texture2D tex)
  {
    viewport.texture = tex;
    var ratio = (float)tex.width / (float)tex.height;
    fitter.aspectRatio = ratio;
  }

  public Rect rect
  {
    get
    {
      return viewport.rectTransform.rect;
    }
  }
}
