using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Enums
{
    public enum NFeCreditType
    {
        PenaltyAndInterest = 1,

        PresumedIBSCreditZFM = 2,

        ReturnDueToTotalDeliveryRefusalOrRecipientNotFound = 3,

        ValueReduction = 4,

        CreditTransferBySuccession = 5,

        ReturnDueToPartialDeliveryRefusal = 6
    }
}
