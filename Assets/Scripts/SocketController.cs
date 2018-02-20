using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.UI;
public class SocketController : MonoBehaviour
{

  public SocketIOComponent socket;
  public Text debug;
  public GameObject playerPrefab;
  public List<PlayerController> players;

  /// <summary>
  /// This function is called when the object becomes enabled and active.
  /// </summary>
  void OnEnable()
  {
    PlayerController.onPlayerPlayEvent += PlayerPlay;
    JoystickController.onPlayerMoveEvent += PlayerMove;
  }

  // Use this for initialization
  void Start()
  {
    players = new List<PlayerController>();
    debug.text = "";
    StartCoroutine(ConnectToServer());
    socket.On("PLAYER_LOGGED", OnPlayerLogged);
    socket.On("PLAY", OnUserPlay);
    socket.On("PLAYER_MOVED", OnUserMove);
    socket.On("USER_DISCONNECTED", OnUserDisconnect);
  }

  IEnumerator ConnectToServer()
  {
    yield return new WaitForSeconds(0.5f);
    socket.Emit("USER_CONNECT");
  }

  private static JSONObject getJsonObject(string name, string position)
  {
    Dictionary<string, string> data = new Dictionary<string, string>();
    data["name"] = name;
    data["position"] = position;
    return new JSONObject(data);
  }


  private void PlayerMove(string name, string position)
  {
    JSONObject dataObj = getJsonObject(name, position);
    socket.Emit("MOVE", dataObj);
  }

  private void PlayerPlay(string name, string position, PlayerController player)
  {
    JSONObject dataObj = getJsonObject(name, position);
    this.players.Add(player);
    socket.Emit("PLAY", dataObj);
  }

  private void OnPlayerLogged(SocketIOEvent obj)
  {
    PlayerData playerData = JsonUtility.FromJson<PlayerData>(obj.data.ToString());
    GameObject newPlayer = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    PlayerController playerController = newPlayer.GetComponent<PlayerController>();
    playerController = (playerController == null) ? newPlayer.AddComponent<PlayerController>() : playerController;
    playerController.externalPlayer = true;
    playerController.SetPosition(playerData.position);
    newPlayer.name = playerData.name;
    playerController.playerName = playerData.name;
    this.players.Add(playerController);
  }

  private void OnUserPlay(SocketIOEvent obj)
  {
    // debug.text += "On Play " + evt.data;
  }

  private void OnUserMove(SocketIOEvent obj)
  {
    PlayerData playerData = JsonUtility.FromJson<PlayerData>(obj.data.ToString());
    PlayerController movedPlayer = players.Find(player => player.playerName == playerData.name);
    movedPlayer.SetPosition(playerData.position);
  }

  private void OnUserDisconnect(SocketIOEvent obj)
  {
    PlayerData playerData = JsonUtility.FromJson<PlayerData>(obj.data.ToString());
    PlayerController disconnectedPlayer = players.Find(player => player.playerName == playerData.name);
    GameObject disconnectedPlayerObject  = disconnectedPlayer.gameObject;
    Destroy(disconnectedPlayer);
    Destroy(disconnectedPlayerObject);
  }
}
