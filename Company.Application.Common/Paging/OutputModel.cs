using System.Collections.Generic;

namespace Company.Application.Common.Paging
{
    /// <summary>
    /// Geriye bu model ile dönüş yapacağız. Header, links ve Items olmak üzere 3 ana bölümden oluşacak.
    /// </summary>
    /// <typeparam name="T">Hangi entity üzerinden işlem yapılacaksa (örn: Customer, AppResource, Organizations .. vs)</typeparam>
    public class OutputModel<T>
    {
        /// <summary>
        /// Sayfalama header bilgileri. Kaç sayfa var, kaçıncı sayfa, sayfada kaç satır var, toplam kaç satır var .. vs
        /// </summary>
        public PagingHeader Paging { get; set; }

        /// <summary>
        /// Önceki , kendisi ve sonraki sayfalara ait linkler. İstemciye kolaylık olması açısından bu linkleri generate ederek göndereceğiz.
        /// </summary>
        public List<LinkInfo> Links { get; set; }

        /// <summary>
        /// Sayfalama parametrelerine uygun olarak çekilmiş satırları içeren liste
        /// </summary>
        public List<T> Items { get; set; }
    }
}