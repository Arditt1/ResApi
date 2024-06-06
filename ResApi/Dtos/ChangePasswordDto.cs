using System;
using System.Collections.Generic;
using System.Text;

namespace ResApi.Dtos
{
    public partial class ChangePasswordDto
    {
        public int Id { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmNewPassword { get; set; }
    }
}
