using System;
using System.Collections.Generic;
using System.Linq;

namespace Company.Application.Common.Paging
{
    /// <summary>
    /// Pagignli listemize ait class.
    /// </summary>
    /// <typeparam name="T">Hangi entity paging ile oluşturulacaksa buraya gönderiliyor</typeparam>
    public class PagedList<T>
    {
        #region Constructor

        /// <summary>
        /// Gelen entity içerisinde eleman sayısı dığarıdan gönderilen sayfa sırası ve sayfa sınır parametrelerini kendi içindeki local variablelara dolduran yapıcı metodumuz
        /// </summary>
        /// <param name="source">IQueryaple tipinde entitymiz</param>
        /// <param name="pageNumber">hangi sayfa</param>
        /// <param name="pageSize">sayfadaki eleman sayısı</param>
        public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            //Quaryable üzerinden toplam kaç satır olduğu bilgisini count  ile alıyoruz
            this.TotalItems = source.Count();

            //hangi sayfa talep edildi ise local değişkene aktarıyoruz
            this.PageNumber = pageNumber;

            //sayfada kaç adet kayıt istendi ise local değişkene aktarıyoruz
            this.PageSize = pageSize;

            //sayfa sayısı ve kaç adet satır içereceği bilgilerine istinaden bir liste oluşturup local değişkene atıyoruz
            this.List = source
                            //sayfalama mantığına göre hangi kayıtların getirileceğini hesaplama için skip kullandık. kaç adet satır atlanarak seçme işlemi yapılacağını basit bir yöntemle hesaplıyoruz
                            .Skip(pageSize * (pageNumber - 1))
                            //sayfa içinde istenen satır asyısı kadar take ile seçme işlemi yapıyoruz. BU işlem sql sorgulamalarında kullanılan TOP ve LIMIT ile aynı mantıktadır diyebiliriz.
                            .Take(pageSize)
                            .ToList();
        }

        #endregion Constructor

        /// <summary>
        /// Toplam databasedeki kayıt sayısı
        /// </summary>
        public int TotalItems { get; }

        /// <summary>
        /// Şuanda istemcinin talep ettiği sayfanın sırası (1. sayfa, 2. sayfa ... hangisi isteniyorsa)
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// İstenen sayfanın kaç eleman barındırdığı bilgisi
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Sayfa içerisindeki satırlar yani veritabanındaki kayıtların kendileri
        /// </summary>
        public List<T> List { get; }

        /// <summary>
        /// Toplam sayfa sayısı hesaplanıyor ceilin yukarı yuvarlama işlemidir, yani sayfa sayısı hesaplama sonucu ondalıklı bir sayı çıkacak olursa en yakın kendisinden büyük tam sayıya yuvarlanacak
        /// </summary>
        public int TotalPages =>
              (int)Math.Ceiling(this.TotalItems / (double)this.PageSize);

        /// <summary>
        /// Mevcut sayfanın kendisinden önce başka sayfa olup olmadığının bilgisi
        /// </summary>
        public bool HasPreviousPage => this.PageNumber > 1;

        /// <summary>
        /// Mevcut sayfanın kendisinden son başka sayfa olup olmadığı bilgisi
        /// </summary>
        public bool HasNextPage => this.PageNumber < this.TotalPages;

        /// <summary>
        /// Kendisinden sonra sayfa varsa hangi sayfa olduğu bilgisi
        /// </summary>
        public int NextPageNumber =>
               this.HasNextPage ? this.PageNumber + 1 : this.TotalPages;

        /// <summary>
        /// Kendisinden önce sayfa varsa hangi sayfa olduğu bilgisi
        /// </summary>
        public int PreviousPageNumber =>
               this.HasPreviousPage ? this.PageNumber - 1 : 1;

        /// <summary>
        /// Daha önce eklediğimiz headerı dahil ediyoruz
        /// </summary>
        /// <returns></returns>
        public PagingHeader GetHeader()
        {
            return new PagingHeader(
                 this.TotalItems, this.PageNumber,
                 this.PageSize, this.TotalPages);
        }
    }
}