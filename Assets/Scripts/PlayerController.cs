using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float playerSpeed = 1;
  public int playerID;
  public string playerName;
  public string position;

  public delegate void OnPlayerPlay(string name, string position, PlayerController player);
  public static event OnPlayerPlay onPlayerPlayEvent;

  public bool externalPlayer;

  /// <summary>
  /// Start is called on the frame when a script is enabled just before
  /// any of the Update methods is called the first time.
  /// </summary>
  void Start()
  {
    LoginPanel.onPlayEvent += PlayClicked;
  }

  private void PlayClicked(string name)
  {
    if (!this.externalPlayer)
    {
      this.playerName = name;
      this.gameObject.transform.position = new Vector3(0, 0.5f, 0);
      this.position = this.GetPosition();
      this.gameObject.name = name;
      onPlayerPlayEvent(this.playerName, this.position, this);
    }
  }

  public string GetPosition()
  {
    return this.transform.position.x + "," + this.transform.position.z;
  }

  public void SetPosition(string position)
  {
    this.position = position;
    string[] positions = position.Split(',');
    float x = float.Parse(positions[0], CultureInfo.InvariantCulture.NumberFormat);
    float z = float.Parse(positions[1], CultureInfo.InvariantCulture.NumberFormat);
    this.transform.position = new Vector3(x, 0.5f, z);
  }

  public void MoveLeft()
  {
    this.transform.Translate(Vector3.left * Time.fixedDeltaTime * playerSpeed);
  }

  public void MoveRight()
  {
    this.transform.Translate(Vector3.right * Time.fixedDeltaTime * playerSpeed);
  }

  public void MoveUp()
  {
    this.transform.Translate(Vector3.forward * Time.fixedDeltaTime * playerSpeed);
  }

  public void MoveDown()
  {
    this.transform.Translate(Vector3.back * Time.fixedDeltaTime * playerSpeed);
  }
}
