﻿using System;
using System.Collections.Generic;

namespace SNDTRCK.Models;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }
}
