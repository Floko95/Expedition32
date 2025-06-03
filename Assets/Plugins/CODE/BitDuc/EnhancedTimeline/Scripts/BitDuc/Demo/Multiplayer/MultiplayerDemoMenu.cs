#if MIRROR
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BitDuc.Demo.Multiplayer
{
    public class MultiplayerDemoMenu : MonoBehaviour
    {
        public static bool clientAuthority = true;

        [SerializeField] Button connect;
        [SerializeField] TMP_Text hostToConnect;
        [SerializeField] Button host;
        [SerializeField] Button serve;
        [SerializeField] Toggle clientAuthorityToggle;

        NetworkManager network;

        void Awake()
        {
            network = FindFirstObjectByType<NetworkManager>();
            connect.onClick.AddListener(Connect);
            host.onClick.AddListener(Host);
            serve.onClick.AddListener(Serve);
            clientAuthorityToggle.onValueChanged.AddListener(SetAuthority);
        }

        void Connect()
        {
            network.networkAddress = hostToConnect.text;
            network.StartClient();
        }

        void Host()
        {
            Debug.Log("Starting host.");
            network.StartHost();
            Debug.Log("Host started.");
        }

        void Serve()
        {
            Debug.Log("Starting server.");
            network.StartServer();
            Debug.Log("Server started.");
        }

        void SetAuthority(bool newClientAuthority)
        {
            clientAuthority = newClientAuthority;
        }
    }
}
#endif
