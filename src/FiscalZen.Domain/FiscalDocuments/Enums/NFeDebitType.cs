using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.FiscalDocuments.Enums
{
    public enum NfeDebitType
    {
        CreditTransferToCooperatives = 1,

        CreditCancellationForTaxExemptOrImmuneSales = 2,

        UnprocessedInvoiceDebitInTaxAssessment = 3,

        PenaltyAndInterest = 4,

        CreditTransferBySuccession = 5,

        AdvancePayment = 6,

        InventoryLoss = 7,

        SimplesNacionalDisqualification = 8
    }
}
