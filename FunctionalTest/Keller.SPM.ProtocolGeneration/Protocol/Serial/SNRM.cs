﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProcotolGeneration.Protocol.Serial
{
    public class SNRM
    {
        public byte[] SNRMCommandBytes = new byte[34];
        public byte[] SNRMLen { get; set; } = { 0x20 };
        public byte[] SNRMFixedBytes { get; set; } = { 0x03, 0x61, 0x93, 0x1B, 0x9F };
        public byte[] SNRMInfoField { get; set; } = { 0x81, 0x80, 0x14, 0x05, 0x02, 0x02, 0x00, 0x06, 0x02, 0x02, 0x00, 0x07, 0x04, 0x00, 0x00, 0x00, 0x01, 0x08, 0x04, 0x00, 0x00, 0x00, 0x01 };
        public byte[] SNRMFCS { get; set; } = { 0x6F, 0xEF };
        public byte[] SNRMPCData { get; set; } = { 0x81, 0x80, 0x14, 0x05, 0x02, 0x03, 0x40, 0x06, 0x02, 0x03, 0x40, 0x07, 0x04, 0x00, 0x00, 0x00, 0x01, 0x08, 0x04, 0x00, 0x00, 0x00, 0x01 };
    }
}
