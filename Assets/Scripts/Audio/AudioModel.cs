using System;

public class AudioModel
{
  public float[] volumes;
  public float duration;

  public AudioModel(float[] volumes, float duration)
  {
    this.volumes = new float[volumes.Length];
    Array.Copy(volumes, this.volumes, volumes.Length);
    this.duration = duration;
  }
}
