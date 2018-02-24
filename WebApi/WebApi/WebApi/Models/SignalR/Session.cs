using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.SignalR
{
    [Table("Sessions")]
    public class Session
    {
        public int SessionID { get; set; }
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ConnectionID { get; set; }
        public string Email { get; set; }
    }
}
