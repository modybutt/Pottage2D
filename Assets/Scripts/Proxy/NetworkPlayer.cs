public struct NetworkPlayer
{
    public ulong playerId { get; private set; }
    public string playerName { get; private set; }

    public NetworkPlayer(ulong playerId, string playerName)
    {
        this.playerId = playerId;
        this.playerName = playerName;
    }
}
