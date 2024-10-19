namespace Masterpiece.DTO
{
    public class UserUpdateModel
    {
        public string? FirstName { get; set; } // جعلها اختيارية
        public string? LastName { get; set; } // جعلها اختيارية
        public string? UserName { get; set; } // جعلها اختيارية
        public string? Email { get; set; } // جعلها اختيارية
        public string? PhoneNumber { get; set; } // جعلها اختيارية
        public IFormFile? Image { get; set; } // لا حاجة للتعديل هنا، هي اختيارية بالفعل
        public string? Address { get; set; } // جعلها اختيارية
   
        public string? Gender { get; set; } // جعلها اختيارية
    }
}
