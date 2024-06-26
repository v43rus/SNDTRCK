﻿using System;
using System.Collections.Generic;

namespace SNDTRCK.Models;

public partial class NewsletterSignup
{
    public int NewsletterId { get; set; }

    public string? UserId { get; set; }

    public bool? IsSignedUp { get; set; }

    public virtual AspNetUser? User { get; set; }
}
