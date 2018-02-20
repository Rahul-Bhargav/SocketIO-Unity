using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WatchButton : MonoBehaviour
{

  public delegate void OnDirectionPress(GameObject direction, bool state);
  public static event OnDirectionPress directionPress;

  public void OnPointDown()
  {
    if(directionPress != null)
      directionPress(this.gameObject, true);
  }

  public void OnPointUp()
  {
    if(directionPress != null)
      directionPress(this.gameObject, false);
  }
}
