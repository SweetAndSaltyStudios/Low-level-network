using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    #region VARIABLES

    private const int MAX_CONNECTION = 100;

    private float connectionTime;

    private int port = 5701;
    private int hostID;
    private int webHostID;
    private int connectionID;
    private int reliableChannel;
    private int unreliableChannel;

    private bool isConnected = false;
    private bool isStarted = false;

    private byte error;

    private string clientName;

    #endregion VARIABLES

    public void Connect()
    {
        var clientName = GameObject.Find("NameInputField").GetComponent<InputField>().text;
        if (clientName == null || clientName == "")
        {
            Debug.LogWarning("You must give a name so we know 'who' is trying to connect.");
            return;
        }

        this.clientName = clientName;

        NetworkTransport.Init();
        ConnectionConfig connectionConfig = new ConnectionConfig();

        reliableChannel = connectionConfig.AddChannel(QosType.Reliable);
        unreliableChannel = connectionConfig.AddChannel(QosType.Unreliable);

        HostTopology hostTopology = new HostTopology(connectionConfig, MAX_CONNECTION);

        hostID = NetworkTransport.AddHost(hostTopology, 0/*, port, null*/);

        // Can not be used on a browser. Linux or Windows works well?
        //webHostID = NetworkTransport.AddWebsocketHost(hostTopology, port, null);

        connectionID = NetworkTransport.Connect(hostID, "127.0.0.1", port, 0, out error);

        connectionTime = Time.time;
        isConnected = true;
    }

    private void Update()
    {
        if (!isConnected)
            return;

        int recHostID;
        int connectionID;
        int channelID;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;

        NetworkEventType recData = NetworkTransport.Receive
            (
            out recHostID,
            out connectionID,
            out channelID,
            recBuffer,
            bufferSize,
            out dataSize,
            out error
            );

        switch (recData)
        {
            case NetworkEventType.Nothing:

                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log("Client: " + connectionID + " has conneted.");
                break;
            case NetworkEventType.DataEvent:
                string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                Debug.Log("Client: " + connectionID + " has sent:. " + message);
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("Client: " + connectionID + " has disconnected.");
                break;
        }
    }
}
