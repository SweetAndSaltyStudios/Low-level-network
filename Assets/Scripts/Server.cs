using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    #region VARIABLES

    private const int MAX_CONNECTION = 100;
    private int port = 5701;
    private int hostID;
    private int webHostID;

    private int reliableChannel;
    private int unreliableChannel;

    private bool isStarted = false;
    private byte error;

    #endregion VARIABLES

    private void Start()
    {
        NetworkTransport.Init();
        ConnectionConfig connectionConfig = new ConnectionConfig();

        reliableChannel = connectionConfig.AddChannel(QosType.Reliable);
        unreliableChannel = connectionConfig.AddChannel(QosType.Unreliable);

        HostTopology hostTopology = new HostTopology(connectionConfig, MAX_CONNECTION);

        hostID = NetworkTransport.AddHost(hostTopology, port, null);
        webHostID = NetworkTransport.AddWebsocketHost(hostTopology, port, null);

        isStarted = true;
    }

    private void Update()
    {
        if (!isStarted)
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

                break;
            case NetworkEventType.DataEvent:

                break;
            case NetworkEventType.DisconnectEvent:

                break;
        }
    }
}
