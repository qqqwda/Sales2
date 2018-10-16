
namespace Sales.Common.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        //Clave foránea
        public int CategoryId { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }


        [Required]
        [StringLength(50)]
        public string Description { get; set; }


        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public Decimal Price { get; set; }


        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }


        [Display(Name = "Published on")]
        [DataType(DataType.Date)]
        public DateTime PublishOn { get; set; }


        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [NotMapped]// no hace parte de la BD, hace parte del modelo
        public byte[] ImageArray { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))

                {
                    return "noproduct";
                }

                return $"http://10.0.4.113/Salesweb/{this.ImagePath.Substring(1)}";

            }
        }
        
    }
}
