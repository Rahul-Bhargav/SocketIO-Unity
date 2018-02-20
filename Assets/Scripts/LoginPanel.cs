using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{

  public InputField inputField;
  public delegate void OnPlay(string name);
  public static event OnPlay onPlayEvent;

  public void OnPlayClicked() {
    if((inputField.text != "" || inputField.text != null) && onPlayEvent != null){
      onPlayEvent(inputField.text);
      this.gameObject.SetActive(false);
    }
  }
}