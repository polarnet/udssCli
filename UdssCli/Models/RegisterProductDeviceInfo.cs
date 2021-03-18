namespace UdssCli.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  public class RegisterProductDeviceInfo
  {
    public int BrandId { get; set; }

    public string BrandName { get; set; }

    public int EquipmentId { get; set; }

    public int EquipmentSubtypeId { get; set; }

    public string Model { get; set; }

    public string EquipmentTypeName { get; set; }

    public string EquipmentSubtypeName { get; set; }
    
    public DateTime? DatePurchase { get; set; }
  }
}
