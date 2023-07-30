using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ModelSEQ : Document
    {
        public string BTS { get => GetString(nameof(BTS)); set => Push(nameof(BTS), value); }
        public string BEC { get => GetString(nameof(BEC)); set => Push(nameof(BEC), value); }
        public string BEV { get => GetString(nameof(BEV)); set => Push(nameof(BEV), value); }
        public string BWS { get => GetString(nameof(BWS)); set => Push(nameof(BWS), value); }
        public string BAP { get => GetString(nameof(BAP)); set => Push(nameof(BAP), value); }
        public string BAF { get => GetString(nameof(BAF)); set => Push(nameof(BAF), value); }
        public string BAC { get => GetString(nameof(BAC)); set => Push(nameof(BAC), value); }
        public string BPR { get => GetString(nameof(BPR)); set => Push(nameof(BPR), value); }
        public DateTime? Time { get => GetDateTime(nameof(Time)); set => Push(nameof(Time), value); }
    }
}
