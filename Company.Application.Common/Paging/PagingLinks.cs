using Company.Application.Common.Paging.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Company.Application.Common.Paging
{
    public class PagingLinks<T> : IPagingLinks<T>
    {
        #region Variables

        /// <summary>
        /// Link oluşturmak için urlHelper dan yararlanacağız
        /// </summary>
        private readonly IUrlHelper _urlHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Bu yapıcı metot sayesinde dependency injection alt yapısını kullanarak urlHelper oluşturuyor ve yukarıda tanımladığımız local değişkene aktarıyoruz.
        /// </summary>
        /// <param name="urlHelper"></param>
        public PagingLinks(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        #endregion

        #region BusinessSection

        /// <summary>
        /// Create link ile tek tek oluşturulan linkleri listeye ekleyerek geri dönecek olan metot
        /// </summary>
        /// <param name="list">sayfalanmış liste sınıfını beklemektedir.</param>
        /// <returns></returns>
        public List<LinkInfo> GetLinks(PagedList<T> list)
        {
            //Yeni bir liste oluşturuluyor
            var links = new List<LinkInfo>();

            //PagedList içerisinde tanımladığımız HasPreviousPage bool tipini kontrol ediyoruz ve true ise önceki sayfa için link oluşturarak listeye ekliyoruz
            if (list.HasPreviousPage)
                links.Add(CreateLink("default", list.PreviousPageNumber,
                           list.PageSize, "previousPage", "GET"));

            //Mevcut sayfa için link oluşturarak listeye ekliyoruz
            links.Add(CreateLink("default", list.PageNumber,
                           list.PageSize, "self", "GET"));

            //PagedList içerisinde tanımladığımız HasNextPage bool tipini kontrol ediyoruz ve true ise sonraki sayfa için link oluşturarak listeye ekliyoruz
            if (list.HasNextPage)
                links.Add(CreateLink("default", list.NextPageNumber,
                           list.PageSize, "nextPage", "GET"));


            return links;
        }

        /// <summary>
        /// urlHelper kullanarak önceki,sonraki ve mevcut sayfa için linkler üreten link
        /// </summary>
        /// <param name="routeName">Uygulamamızda kullandığımız ve bu link üretimi için bildireceğimiz route ismimiz</param>
        /// <param name="pageNumber">Hangi sayfa isteniyor bilgisi</param>
        /// <param name="pageSize">Sayfada kaç satır olarak bilgisi</param>
        /// <param name="rel">link hangi sayfayı gösteriyor? önceki, kendisi, sonraki</param>
        /// <param name="method">işlemin Get mi Post mu olduğunu tutacak</param>
        /// <returns>Eklenen linki LinkInfo tipinde döner</returns>
        public LinkInfo CreateLink(string routeName, int pageNumber, int pageSize, string rel, string method)
        {
            return new LinkInfo
            {
                Href = _urlHelper.Link(routeName,
                            new { PageNumber = pageNumber, PageSize = pageSize }),
                Rel = rel,
                Method = method
            };
        }

        #endregion
    }
}
