using System;
using System.Collections.Generic;

namespace SalesManagement.Model;

public partial class MMessage
{
    public string MsgId { get; set; } = null!;

    public string MsgComments { get; set; } = null!;

    public string MsgTitle { get; set; } = null!;

    public int MsgButton { get; set; }

    public int MsgIcon { get; set; }
}
