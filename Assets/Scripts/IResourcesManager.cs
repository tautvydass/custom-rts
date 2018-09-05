public interface IResourcesManager
{
    bool TryPay(Resources price);
    void Refund(Resources resources);
}
