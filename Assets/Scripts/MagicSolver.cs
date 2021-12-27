using UnityEngine;

public class MagicSolver : ISolver
{
  [SerializeField]
  private SlideshowController controller;
  private int counter = 0;
  private float threshold;

  void Start()
  {
    threshold = PlayerPrefs.GetFloat(CalibrationManager.CALIBRATE_KEY, 0.5f);
  }
  public override void OnProcess(AudioModel model)
  {
    if (model.Mean > threshold)
    {
      if (counter <= 0) controller.StartRecord();
      counter = 2;
    }
    else counter--;
    if (counter <= 0) controller.StopRecord();
  }
}
