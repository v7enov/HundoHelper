namespace HundoHelper.Models
{
    public interface ICollectible
    {
        string Name { get; set; }
        void Update();
        int OrderIndex { get; set; }
    }
}
