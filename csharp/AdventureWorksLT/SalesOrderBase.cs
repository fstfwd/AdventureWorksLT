﻿using DevZest.Data;
using DevZest.Data.Annotations;

namespace DevZest.Samples.AdventureWorksLT
{
    /**********************************************************************************************************
     *  Most of the code of this class is generated automatically via "Model Visualizer" tool window of
     *  free RDO.Tools Visual Studio extension.
     *  
     *  To install RDO.Tools:
     *  Select Visual Studio menu "Tools" -> "Extensions and Update...", then search for "DevZest.Data.Tools".
     *  The extension requires Visual Studio 15.3 or later.
     *********************************************************************************************************/
    [CustomValidator(nameof(VAL_LineCount))]
    [Computation(nameof(ComputeLineCount), ComputationMode.Aggregate)]
    [Computation(nameof(ComputeSubTotal), ComputationMode.Aggregate)]
    [InvisibleToDbDesigner]
    public abstract class SalesOrderBase<T> : SalesOrderHeader
        where T : SalesOrderDetail, new()
    {
        static SalesOrderBase()
        {
            RegisterColumn((SalesOrderBase<T> _) => _.LineCount);
            RegisterChildModel((SalesOrderBase<T> _) => _.SalesOrderDetails, (T _) => _.FK_SalesOrderHeader);
        }

        public _Int32 LineCount { get; private set; }

        [_CustomValidator]
        private CustomValidatorEntry VAL_LineCount
        {
            get
            {
                string Validate(DataRow dataRow)
                {
                    return LineCount[dataRow] > 0 ? null : UserMessages.Validation_SalesOrder_LineCount;
                }

                IColumns GetSourceColumns()
                {
                    return LineCount;
                }

                return new CustomValidatorEntry(Validate, GetSourceColumns);
            }
        }

        public T SalesOrderDetails { get; private set; }

        [_Computation]
        private void ComputeLineCount()
        {
            LineCount.ComputedAs(SalesOrderDetails.SalesOrderDetailID.CountRows());
        }

        [_Computation]
        private void ComputeSubTotal()
        {
            SubTotal.ComputedAs(SalesOrderDetails.LineTotal.Sum().IfNull(0), false);
        }
    }
}
