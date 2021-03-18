using System.ComponentModel.DataAnnotations;
using Westwind.Globalization;

namespace UdssCli.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  public class RegisterProductData
  {
    [Required]
    public string Serial { get; set; }

    public int BrandId { get; set; }

    public int EquipmentTypeId { get; set; }

    public int EquipmentSubtypeId { get; set; }

    public string Model { get; set; }

    public string BrandName { get; set; }

    public string EquipmentTypeName { get; set; }

    public string EquipmentSubtypeName { get; set; }

    [Required]
    public DateTime? DatePurchase { get; set; }
  }
}
