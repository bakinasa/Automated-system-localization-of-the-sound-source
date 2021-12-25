using UnityEngine;
using UnityEngine.UI;

public class RTWebcam : MonoBehaviour
{
  private bool available = false;
  private WebCamTexture cam;

  [SerializeField]
  private RawImage viewport;
  [SerializeField]
  private AspectRatioFitter fitter;

  void setDevice(WebCamDevice device)
  {
    var rect = viewport.GetComponentInParent<RectTransform>();
    cam = new WebCamTexture(device.name, (int)rect.rect.height, (int)rect.rect.width);
    available = true;
    viewport.texture = cam;
    cam.Play();
  }

  void Start()
  {
    foreach (var device in WebCamTexture.devices)
    {
      if (!device.isFrontFacing)
      {
        setDevice(device);
        break;
      }
    }
    if (!available)
    {
      if (WebCamTexture.devices.Length == 0)
      {
        Debug.LogError("No back camera found");
        Application.Quit();
      }
      else
      {
        setDevice(WebCamTexture.devices[0]);
      }
    }
  }

  void Update()
  {
    if (!available)
    {
      return;
    }

    var ratio = (float)cam.width / (float)cam.height;
    fitter.aspectRatio = 1 / ratio;

    var scaleY = cam.videoVerticallyMirrored ? -1f : 1f;
    viewport.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

    var orient = -cam.videoRotationAngle;
    viewport.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
  }
}
