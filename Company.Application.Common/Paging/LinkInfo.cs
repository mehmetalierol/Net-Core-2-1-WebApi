namespace Company.Application.Common.Paging
{
    /// <summary>
    /// Paging yaparken kullanıcıya önceki,sonraki ve mevcut sayfalama için gerekli api get linkini göndereceğiz
    /// Bu sınıf bizim için bu linklerin modeli görevini görecek
    /// </summary>
    public class LinkInfo
    {
        /// <summary>
        /// Linkin yolu
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// Link hangi sayfaya ait, previous,next,self ... vs
        /// </summary>
        public string Rel { get; set; }

        /// <summary>
        /// Hangi HTTP metot kullanılacak GET,POST,PUT ... vs
        /// </summary>
        public string Method { get; set; }
    }
}
