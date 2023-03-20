using Unity.Collections;
using Unity.Netcode;

public struct LobbyPlayerState : INetworkSerializable, System.IEquatable<LobbyPlayerState>
{
    // public NetworkPlayer networkPlayer;
    public ulong PlayerId;
    public FixedString64Bytes PlayerName;
    public bool IsReady;

    public LobbyPlayerState(ulong playerId, FixedString64Bytes playerName, bool isReady)
    {
        this.PlayerId = playerId;
        this.PlayerName = playerName;
        this.IsReady = isReady;
    }

    public bool Equals(LobbyPlayerState other)
    {
        if (PlayerId != other.PlayerId) return false;
        if (!PlayerName.Equals(other.PlayerName)) return false;
        if (IsReady != other.IsReady) return false;
        return true;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PlayerId);
        serializer.SerializeValue(ref PlayerName);
        serializer.SerializeValue(ref IsReady);
    }
}
