using System;
using System.Collections.Generic;
using System.Text;

namespace ResApi.Dtos
{
    public partial class RegisterUserDto
    {
        public string TipKind { get; set; }
        public string ClientNo { get; set; }
        public string Nipt { get; set; }
        public string Title { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mob { get; set; }
        public string Place { get; set; }
        public string Password { get; set; }
        public bool AutoRegister { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdateOn { get; set; }
    }
}
