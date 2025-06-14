using System.ComponentModel.DataAnnotations;

namespace ChatMail.Models
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage ="Lütfen geçerli bir e-posta adresi giriniz!")]
        public string Email { get; set; }

        [Display(Name ="Telefon Numarası")]
        [Phone(ErrorMessage ="Lütfen geçerli bir telefon numarası giriniz!")]
        public string PhoneNumber { get; set; }

        [Display(Name ="Ad")]
        public string Name { get; set; }

        [Display(Name ="Soyad")]
        public string Surname { get; set; }

        [Display(Name ="Profil Resmi Url")]
        public string ProfileImageUrl { get; set; }
    }
}
