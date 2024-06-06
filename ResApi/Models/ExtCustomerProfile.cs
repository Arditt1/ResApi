using ResApi.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ResApi.Models
{
    public class ExtCustomerProfile
    {

        [StringLength(1)]
        public string Status { get; set; }
        /// <summary>
        /// 2	Person juridik
        /// 3	Person fizik
        /// </summary>
        [StringLength(1)]
        public string Tipkind { get; set; }
        [StringLength(30)]
        public string Nipt { get; set; }
        [StringLength(30)]
        public string Clientno { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(50)]
        public string Lastname { get; set; }
        [StringLength(1)]
        public string Sex { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Mob { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
        [StringLength(250)]
        public string Place { get; set; }
        public DateTime? Birthdate { get; set; }
        public List<NotificationDto> Notifications { get; set; }

    }
}
