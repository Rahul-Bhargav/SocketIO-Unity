using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LLATest : MonoBehaviour
{

  int myReliableChannelId;
  int maxConnections = 10;
  int socketId;
  int socketPort = 8888;
  int connectionId;
  public Text result;
  // Use this for initialization
  void Start()
  {
    NetworkTransport.Init();
    ConnectionConfig config = new ConnectionConfig();
    myReliableChannelId = config.AddChannel(QosType.Unreliable);
    HostTopology topology = new HostTopology(config, maxConnections);
    socketId = NetworkTransport.AddHost(topology, socketPort);
    Debug.Log("Socket Open. SocketId is: " + socketId);
  }

  public void Connect()
  {
    byte error;
    connectionId = NetworkTransport.Connect(socketId, "127.0.0.1", 33333, 0, out error);
    Debug.Log("Connected to server. ConnectionId: " + connectionId);
  }


  public void SendSocketMessage()
  {
    byte error;
    byte[] buffer = new byte[1024];
    Stream stream = new MemoryStream(buffer);
    BinaryFormatter formatter = new BinaryFormatter();
    formatter.Serialize(stream, "HelloServer");

    int bufferSize = 1024;

    NetworkTransport.Send(socketId, connectionId, myReliableChannelId, buffer, bufferSize, out error);
  }

  // Update is called once per frame
  void Update()
  {
    int recHostId;
    int recConnectionId;
    int recChannelId;
    byte[] recBuffer = new byte[1024];
    int bufferSize = 1024;
    int dataSize;
    byte error;
    NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, recBuffer, bufferSize, out dataSize, out error);
    switch (recNetworkEvent)
    {
      case NetworkEventType.Nothing:
        break;
      case NetworkEventType.ConnectEvent:
        Debug.Log("incoming connection event received");
        break;
      case NetworkEventType.DataEvent:
        Stream stream = new MemoryStream(recBuffer);
        BinaryFormatter formatter = new BinaryFormatter();
        string message = formatter.Deserialize(stream) as string;
        result.text = message;
        Debug.Log("incoming message event received: " + message);
        break;
      case NetworkEventType.DisconnectEvent:
        Debug.Log("remote client event disconnected");
        break;
    }
  }
}
