namespace FX.Core.Models.Base
{
    public interface ICta : ISitecoreItem
    {
        bool EnableCtaPanel { get; set; }

        string CtaDataSource { get; set; }
    }
}
