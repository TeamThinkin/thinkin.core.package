using Firesplash.UnityAssets.SocketIO;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSocketListener : MonoBehaviour
{
    public static event System.Action OnSocketConnected;
    public static event System.Action<UserDto> OnSetUser;
    public static event System.Action<string> OnAvatarUrlUpdated;

    public static SocketIOInstance Socket { get; private set; }

    private void Awake()
    {
        var communicator = GetComponent<SocketIOCommunicator>();
        Debug.Log("Connecting socket: " + Config.HomeServerAuthorityAddress);
        communicator.socketIOAddress = Config.HomeServerAuthorityAddress;
        communicator.Instance.Connect();
        Socket = communicator.Instance;
    }

    void Start()
    {
        
        Socket.On("connect", onSocketConnected);
        Socket.On("avatarUrlUpdated", onAvatarUrlUpdatedReceived);
        Socket.On("setUser", onSetUserReceived);
        Socket.On("userLocationChanged", onUserLocationChange);
    }

    private void OnDestroy()
    {
        Socket?.Off("connect", onSocketConnected);
        Socket?.Off("avatarUrlUpdated", onAvatarUrlUpdatedReceived);
        Socket?.Off("setUser", onSetUserReceived);
        Socket?.Off("userLocationChanged", onUserLocationChange);
    }

    private void onSocketConnected(string e)
    {
        OnSocketConnected?.Invoke();
    }

    private void onUserLocationChange(string e)
    {
        var roomUrl = e;
        DestinationPresenter.Instance.DisplayUrl(roomUrl);
    }

    private void onAvatarUrlUpdatedReceived(string e)
    {
        if (OnAvatarUrlUpdated == null) return;
        Debug.Log("Received avatar url: *" + e + "*");
        var url = e.StripQuotes();
        Debug.Log("Url: *" + url + "*");
        OnAvatarUrlUpdated(url);
    }

    private void onSetUserReceived(string e)
    {
        if (OnSetUser == null) return;
        var dto = JsonConvert.DeserializeObject<UserDto>(e);
        OnSetUser(dto);
    }
}
