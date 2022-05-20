﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Host.Models.Responses.PollResponse;

namespace Host.Models.Requests;
public class PollRequest
{
    public string Token { get; set; }
    public List<PollOption> Options { get; set; }
}