using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CalibrationManager : ISolver
{
  public const string CALIBRATE_KEY = "calibrateLevel";
  [SerializeField]
  private RectTransform[] objects;
  [SerializeField]
  private RectTransform parent;
  [SerializeField]
  private Button calibrateButton;
  private float[] values;

  public override void OnProcess(AudioModel model)
  {
    for (int i = 1; i < values.Length; i++)
      values[i - 1] = values[i];
    values[values.Length - 1] = model.Max;
    Draw();
  }

  void Start()
  {
    calibrateButton.onClick.AddListener(PressCalibrate);
    var width = parent.rect.width;
    var height = parent.rect.height;
    var wone = width / objects.Length;
    var bias = -width / 2 + wone / 2;
    values = new float[objects.Length];
    for (int i = 0; i < objects.Length; i++)
    {
      var obj = objects[i];
      obj.localPosition = new Vector3(wone * i + bias, 0, 0);
      obj.sizeDelta = new Vector2(wone, height);
      obj.localScale = new Vector3(1, values[i], 1);
    }
  }

  void Draw()
  {
    for (int i = 0; i < objects.Length; i++)
    {
      var obj = objects[i];
      obj.localPosition = new Vector3(obj.localPosition.x, 0, 0);
      obj.localScale = new Vector3(1, values[i], 1);
    }
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      SceneManager.LoadScene("Main");
    }
  }

  void PressCalibrate()
  {
    print("here");
    var mx = values[0];
    foreach (var val in values)
    {
      if (mx < val) mx = val;
    }
    PlayerPrefs.SetFloat(CALIBRATE_KEY, mx);
    PlayerPrefs.Save();
  }
}
