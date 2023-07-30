using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ModelS10 : Document
    {
        public string MTI { get => GetString(nameof(MTI)); set => Push(nameof(MTI), value); }
        public string MHU { get => GetString(nameof(MHU)); set => Push(nameof(MHU), value); }
        public string MFL { get => GetString(nameof(MFL)); set => Push(nameof(MFL), value); }
        public string MTS { get => GetString(nameof(MTS)); set => Push(nameof(MTS), value); }
        public string MEC { get => GetString(nameof(MEC)); set => Push(nameof(MEC), value); }
        public string MEV { get => GetString(nameof(MEV)); set => Push(nameof(MEV), value); }
        public string MAV { get => GetString(nameof(MAV)); set => Push(nameof(MAV), value); }
        public string MFX { get => GetString(nameof(MFX)); set => Push(nameof(MFX), value); }
        public string MFD { get => GetString(nameof(MFD)); set => Push(nameof(MFD), value); }
        public string MSM { get => GetString(nameof(MSM)); set => Push(nameof(MSM), value); }
        public string MDM { get => GetString(nameof(MDM)); set => Push(nameof(MDM), value); }
        public string MDH { get => GetString(nameof(MDH)); set => Push(nameof(MDH), value); }
        public string MSS { get => GetString(nameof(MSS)); set => Push(nameof(MSS), value); }
        public string MDS { get => GetString(nameof(MDS)); set => Push(nameof(MDS), value); }
        public string MHR { get => GetString(nameof(MHR)); set => Push(nameof(MHR), value); }
        public string MRT { get => GetString(nameof(MRT)); set => Push(nameof(MRT), value); }
        public string MRH { get => GetString(nameof(MRH)); set => Push(nameof(MRH), value); }
        public string MRC { get => GetString(nameof(MRC)); set => Push(nameof(MRC), value); }
        public string MRB { get => GetString(nameof(MRB)); set => Push(nameof(MRB), value); }
        public string MVC { get => GetString(nameof(MVC)); set => Push(nameof(MVC), value); }
        public DateTime? Time { get => GetDateTime(nameof(Time)); set => Push(nameof(Time), value); }
    }

    
}
