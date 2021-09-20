using MLAPI;
using MLAPI.Transports.UNET;
using PopovRadio.Scripts.Tools.AppEvents;
using TMPro;
using UnityEngine;

public class ConnectionUI : NetworkBehaviour
{
    [SerializeField] private TMP_InputField ipInput;
    [SerializeField] private AppEvent OnConnected;

    private UNetTransport _networkTransport;

    public override void NetworkStart()
    {
        _networkTransport = NetworkManager.Singleton.gameObject.GetComponent<UNetTransport>();
    }

    public void OnHostClicked()
    {
        NetworkManager.Singleton.StartHost();

        OnConnected.Invoke();

        SceneLoader.Instance.LoadScene("SampleScene");
    }

    public void OnConnectClicked()
    {
        _networkTransport.ConnectAddress = ipInput.text;
        NetworkManager.Singleton.StartClient();

        OnConnected.Invoke();

        SceneLoader.Instance.LoadScene("SampleScene");
    }
}