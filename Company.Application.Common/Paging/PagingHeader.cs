using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Company.Application.Common.Paging
{
    /// <summary>
    /// Paging modelde geri döneceğimiz paging header modeli
    /// Bu model içerisinde toplam listelenebilecek eleman sayısı, toplam sayfa sayısı , mevcut sayfanın numarası gibi bilgiler içerecek
    /// </summary>
    public class PagingHeader
    {

        #region Constructor

        /// <summary>
        /// Yapıcı metot ile bu modeldeki propertyler set ediliyor
        /// </summary>
        /// <param name="totalItems">Toplam kaç adet satır olduğu</param>
        /// <param name="pageNumber">Mevcut sayfanın numarası</param>
        /// <param name="pageSize">Mevcut sayfanın kaç satır veri taşıdığı</param>
        /// <param name="totalPages">Toplam bu sayfa dahil kaç sayfa olduğu</param>
        public PagingHeader(
           int totalItems, int pageNumber, int pageSize, int totalPages)
        {
            this.TotalItems = totalItems;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalPages = totalPages;
        }

        #endregion

        /// <summary>
        /// Toplam kaç adet satır olduğu bilgisini tutar
        /// </summary>
        public int TotalItems { get; }

        /// <summary>
        /// Mevcut sayfanın numarasını gösterir (kaçıncı sayfa olduğu bilgisi)
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Mevcut sayfada kaç satır olduğunu gösterir
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// İstenen sayfa uzunluğunda kaç sayfa olduğu bilgisini barındırır
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// PagignHeader modelini daha sonra bu metotu çağırarak JSON formatına çevirip headera ekleyeceğiz
        /// Dikkat ederseniz SerializeObject metoduna value olarak this yani bu model gönderiliyor ve jsona dönüştürme işlemi yapılıyor
        /// </summary>
        /// <returns></returns>
        public string ToJson() => JsonConvert.SerializeObject(this,
                                    new JsonSerializerSettings
                                    {
                                        ContractResolver = new
                    CamelCasePropertyNamesContractResolver()
                                    });

    }
}
