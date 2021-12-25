using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioRecorder : MonoBehaviour
{
  public const int FREQ = 44100;
  public const double DELTA_T = 1d / FREQ;
  const int CLIP_DURATION = 1;
  const int N = FREQ * CLIP_DURATION;

  private AudioSource source;
  [SerializeField]
  private ISolver[] solvers;

  void Start()
  {
    if (Microphone.devices.Length == 0)
    {
      Debug.LogError("Not found microphone");
      Application.Quit();
      return;
    }
    source = GetComponent<AudioSource>();
    StartCoroutine(MicroLoop());
  }

  IEnumerator MicroLoop()
  {
    while (true)
    {
      AudioClip clip = Microphone.Start(null, false, CLIP_DURATION, FREQ);
      float currentTime = Time.time;
      if (source.clip != null)
        ProcessClip(source.clip);
      float sleepTime = Time.time - currentTime + CLIP_DURATION;
      if (sleepTime > 0)
        yield return new WaitForSecondsRealtime(sleepTime);
      source.clip = clip;
    }
  }

  void ProcessClip(AudioClip clip)
  {
    const int WINDOW_SIZE = (int)(0.1 * FREQ);
    var window = new float[WINDOW_SIZE];
    var volumes = new float[clip.samples / WINDOW_SIZE];
    var j = 0;
    for (int i = 0; i < clip.samples; i += WINDOW_SIZE)
    {
      clip.GetData(window, i);
      var maxWindow = 0f;
      foreach (var val in window)
      {
        var squared = val * val;
        if (maxWindow < squared)
        {
          maxWindow = squared;
        }
      }
      volumes[j] = Mathf.Sqrt(maxWindow);
      j++;
    }
    var model = new AudioModel(volumes, clip.length);
    foreach (var solver in solvers)
      solver.OnProcess(model);
  }
}