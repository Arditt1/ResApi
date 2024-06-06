using System;
using System.Collections.Generic;
using System.Text;

namespace ResApi.Dtos
{
    public partial class AuthenticateUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TipKind { get; set; }
        public string Nipt { get; set; }
        public string Clientno { get; set; }
        public string Surname { get; set; }
        public string Sex { get; set; }
        public string Celphone { get; set; }
        public string Address { get; set; }
        public string Place { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public Guid CustomerId { get; set; }
        public List<NotificationDto> Notifications { get; set; }

        public AuthenticateUserDto(int id, string tipKind, string nipt, string clientNo, string name, string surname, string email, string sex, string celphone, string address, string place, DateTime? birthDate, string token, Guid customerId, List<NotificationDto> notifications)
        {
            Id = id;
            TipKind = tipKind;
            Nipt = nipt;
            Clientno = clientNo;
            Name = name;
            Surname = surname;
            Username = email;
            Sex = sex;
            Celphone = celphone;
            Address = address;
            Place = place;
            BirthDate = birthDate;
            Token = token;
            CustomerId = customerId;
            Notifications = notifications;
        }

        public AuthenticateUserDto()
        {
        }
    }
}
