using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ResApi.Models
{
    public partial class DefOperator
    {
        public int Id { get; set; }
        public string TipKind { get; set; }
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mob { get; set; }
        public string Place { get; set; }
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime? Birthdate { get; set; }
        public bool ConfirmedMail { get; set; }
        public string VerificationCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdateOn { get; set; }
    }
}