using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Commons
{
    public class Enums
    {
        public enum ProfileStatus
        {
            Draft = 0,
            New = 1,
            Input = 2,
            Expertise = 3,
            Deny = 4,
            Additional = 5,
            Release = 6,
            Compared = 7,
            Cancel = 8,
            PCB = 9,
            Processing = 10,
            Accept = 11,
            Finish = 12,
        }
        public enum ProfileType
        {
            Common = 1,
            Courier = 2,
            MCredit = 3,
            Company = 4,
            RevokeDebt = 5
        }
        public enum NoteType
        {
            Common = 1,
            HosoCourrier = 2,
            Company = 3,
            MCreditTemp = 4,
            RevokeDebt = 5
        }
    }
}
