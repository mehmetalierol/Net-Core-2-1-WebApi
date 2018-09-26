namespace Company.Application.Common.Paging
{
    /// <summary>
    /// Paging yaparken bu parametreleri dışarıdan alacağız
    /// Bu parametreler bize istemcinin hangi sayfayı istediğini ve o sayfada kaç adet kayıt görmek istediğini bildirecek
    /// </summary>
    public class PagingParams
    {
        /// <summary>
        /// Hangi sayfa isteniyor ise onun sıra numarası Örnek 1. sayfa için 1 2. safya için 2 gibi
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// İstenen sayfanın kaç adet kayıt içermesi gerektiği Örnek 1. sayfayı istiyorum 5 kayıt istiyorum gibi
        /// </summary>
        public int PageSize { get; set; } = 5;
    }
}