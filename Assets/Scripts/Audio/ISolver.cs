using UnityEngine;

public abstract class ISolver : MonoBehaviour
{
  public abstract void OnProcess(AudioModel model);
}