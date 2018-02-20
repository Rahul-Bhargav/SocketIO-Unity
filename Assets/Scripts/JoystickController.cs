using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{

  public bool leftClicked;
  public bool rightClicked;
  public bool upClicked;
  public bool downClicked;

  public bool movement;
  public PlayerController player;

  public delegate void OnPlayerMove(string name, string position);
  public static event OnPlayerMove onPlayerMoveEvent;
  // Use this for initialization
  void Start()
  {
    WatchButton.directionPress += OnPress;
    LoginPanel.onPlayEvent += EnableJoystick;
    this.gameObject.SetActive(false);
  }

  private void EnableJoystick(string name)
  {
    this.gameObject.SetActive(true);
  }

  void OnPress(GameObject direction, bool state)
  {
    Debug.Log(direction.name);
    movement = true;
    switch (direction.name)
    {
      case "Left":
        leftClicked = state;
        break;
      case "Right":
        rightClicked = state;
        break;
      case "Up":
        upClicked = state;
        break;
      case "Down":
        downClicked = state;
        break;
      default:
        movement = false;
        break;
    }
  }

  /// <summary>
  /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
  /// </summary>
  void FixedUpdate()
  {
    movement = Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Vertical") !=0;

    if (leftClicked || Input.GetAxis("Horizontal") < 0)
      player.MoveLeft();

    if (rightClicked || Input.GetAxis("Horizontal") > 0)
      player.MoveRight();

    if (upClicked || Input.GetAxis("Vertical") > 0)
      player.MoveUp();

    if (downClicked || Input.GetAxis("Vertical") < 0)
      player.MoveDown();

    if(movement)
      onPlayerMoveEvent(player.playerName, player.GetPosition());
  }
}
