﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keller.SPM.ProtocolGeneration.Protocol.Serial
{
    public class InstantCurrLogicalName
    {
        public byte[] HDLCLen { get; set; } = { 0x2C };
        public byte[] RequestBlock { get; set; } = { 0xD0, 0x1E, 0x30, };
        public byte[] InvocationCounter { get; set; } = { 0x00, 0x00, 0x00, 0x00 };
        public byte[] PlainText { get; set; } = { 0xC0, 0x01, 0xC1, 0x00, 0x03, 0x01, 0x00, 0x5E, 0x5B, 0x0E, 0xFF, 0x02, 0x00 };
        public byte[] FixedBytes { get; set; } = { 0x03, 0x61, 0xDC, 0xDC, 0xB2, 0xE6, 0xE6, 0x00 };
        public byte[] FixedBytesFT { get; set; } = { 0x03, 0x61, 0x10, 0xBC, 0xBE, 0xE6, 0xE6, 0x00 };
    }
}
