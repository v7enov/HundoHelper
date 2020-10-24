namespace HundoHelper.Models
{
    public delegate void NameChanged();
    public interface ICollectible
    {
        event NameChanged OnNameChanged;
        string Name { get; set; }
        void Update();
        int OrderIndex { get; set; }
    }
}
