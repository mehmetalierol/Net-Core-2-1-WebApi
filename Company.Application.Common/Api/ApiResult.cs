namespace Company.Application.Common.Api
{
    #region ObjectResult

    /// <summary>
    /// Tipi çalışma anında belli olacak ya da kontrol etmek istemediğimiz standart tipler olacak ise bu nesne ile dönüşümüzü yapacağız.
    /// Her bir standart ValueType için nesne oluşturmak yerine, object tipinde bir nesne oluşturup kutulama(boxing) yaparak dönüş yapacağız.
    /// Kutulama valueType'ların referenceType lara dönüştürülmesidir. Sistem bunu otomatik olarak yapar. Bu nesne oluşturulduğun data propertisine atanacak tip sistem tarafından otomatik olarak explicit boxing işlemine tabi tutulacaktır.
    /// </summary>
    public class ApiResult
    {
        public string Message { get; set; }

        public int StatusCode { get; set; }

        public object Data { get; set; }
    }

    #endregion

    #region GenericResult

    /// <summary>
    /// Web Api projemizden geriye dönüşleri(response to requester) generic olarak handle edecek kodumuz.
    /// Api result türünde geri döndürmek istediğimiz generic tipi message ve statusCode alanları ile birlikte istemciye dönüyoruz
    /// Yukarıdaki object alanını bizim için serbest dönüşler yapacak ancak biz bu generic class'ı tanımlayarak method dönüşünde ne tip dönmesi gerektiğiniz bildirmiş olacağız. 
    /// Bu sayede hem kodumuza bir zorunluluk getirmiş hem de tasarım sırasında ortaya çıkacak yanlış tip dönme gibi sorunların önüne geçmiş olacağız.
    /// </summary>
    /// <typeparam name="T">Geri dönemesini istediğimiz tip</typeparam>
    public class ApiResult<T>
    {
        public string Message { get; set; }

        public int StatusCode { get; set; }

        public T Data { get; set; }
    }

    #endregion
}
