using System;
using System.Collections.Generic;
using System.Text;

namespace UnivIntel.GBN.Core.Interfaces
{
    public interface IGBNDataAccessWorker
    {
        IGBNQueryExecution _GBNQueryExecutionSvc { get; set; }
    }
}
