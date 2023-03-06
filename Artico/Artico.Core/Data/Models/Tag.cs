﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artico.Core.Data.Models
{
    public class Tag
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = null!;
    }
}
