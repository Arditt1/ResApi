using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ResApi.Dtos
{
    public partial class CustomerDto
    {
        public int OperatorId { get; set; }
        /// <summary>
        /// 2	Person juridik
        /// 3	Person fizik
        /// </summary>
        [StringLength(1)]
        public string Tipkind { get; set; }
        [StringLength(100)]
        public string Nipt { get; set; }
        [StringLength(100)]
        public string Clientno { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Lastname { get; set; }
        [StringLength(1)]
        public string Sex { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(100)]
        public string Mob { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
        [StringLength(250)]
        public string Place { get; set; }
        public DateTime? Birthdate { get; set; }
        public Guid Id { get; set; }

    }
}
