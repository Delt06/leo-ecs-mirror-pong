namespace Composition
{
    public interface INetworkingSetUp
    {
        bool IsServer { get; }
        bool IsClient { get; }
    }
}