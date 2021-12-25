using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintSolver : ISolver
{
  public override void OnProcess(AudioModel model)
  {
    var values = model.volumes;
    float mean = 0, max = values[0], min = values[0];
    foreach (var v in values)
    {
      mean += v;
      if (max < v) max = v;
      if (min > v) min = v;
    }
    mean /= values.Length;
    Debug.LogFormat("max = {0}, min = {1}, mean = {2}", max, min, mean);
  }
}
