using System;
using System.Collections.Generic;

namespace SalesManagement.Model;

public partial class MDivision
{
    public int MDivisionId { get; set; }

    public string DivisionName { get; set; } = null!;

    public bool DspFlg { get; set; }

    public string Comments { get; set; } = null!;
}
