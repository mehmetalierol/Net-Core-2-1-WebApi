using System;

namespace Company.Application.Common.Data
{
    public class EntityBase
    {
        /// <summary>
        /// Tüm entitylerimizde bulunması gereken propertyleri bu base class ile tanımlıyoruz.
        /// Bu sınıfımızı inherit alacak derived class'ımız dğrudan aşağıdaki propertylere sahip olacak ve biz tekrarlı kod yazmak zorunda kalmayacağız
        /// Dto ve data arasındaki farklılık eminim dikkatinizi çekmiştir burada örnek olması açısından küçük bir business kod ekledim
        /// Eğer bu sınıf türetildiğinde Id null ise default olarak yeni bir guid oluşturularak atanacak ve aynı şey createDate alanı içinde geçerli olacak
        /// Data sınıfları business içerebilir ancak dto sınıfları sadece propertyler içermektedir, çünkü amaçları datayı taşımaktır.
        /// Data ve dto arasındaki value transferlerini auto Mapper kullanarak yapacağız.
        /// </summary>
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Creator { get; set; }
    }
}
