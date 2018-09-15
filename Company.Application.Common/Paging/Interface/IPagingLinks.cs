using System.Collections.Generic;

namespace Company.Application.Common.Paging.Interface
{
    /// <summary>
    /// Interfaceler aslında kurallar bütünüdür ve içeriğinde belirtilen tüm kurallar kendisini implemente eden sınıflar tarafından uyulması zorunlu kurallardır.
    /// </summary>
    /// <typeparam name="T">Hangi entity ise onu gönderiyoruz.</typeparam>
    public interface IPagingLinks<T>
    {
        /// <summary>
        /// Bu sınıfı imlemente eden sınıfların içerisinde getlinks isminde geriye List<LinkInfo> tipinde değer dönen ve içerisine PagedList<T> tipinde parametre bekleyen bir metot olması gerektiğini belirtiyoruz.
        /// </summary>
        /// <param name="list">Sayfalama yapılmış listeyi bekliyor</param>
        /// <returns></returns>
        List<LinkInfo> GetLinks(PagedList<T> list);

        /// <summary>
        /// CreateLink isminde bir metot olması gerektiğini ve aşağıdaki parametreleri alarak geriye LinkInfo dönmesi gerektiğini belirtiyoruz.
        /// </summary>
        /// <param name="routeName">Uygulamamızda kullandığımız ve bu link üretimi için bildireceğimiz route ismimiz</param>
        /// <param name="pageNumber">Hangi sayfa isteniyor bilgisi</param>
        /// <param name="pageSize">Sayfada kaç satır olarak bilgisi</param>
        /// <param name="rel">link hangi sayfayı gösteriyor? önceki, kendisi, sonraki</param>
        /// <param name="method">işlemin Get mi Post mu olduğunu tutacak</param>
        /// <returns>Eklenen linki LinkInfo tipinde döner</returns>
        LinkInfo CreateLink(string routeName, int pageNumber, int pageSize, string rel, string method);
    }
}
